using Pokete.Battle;
using Pokete.Data;
using Pokete.Data.Generated;
using Pokete.Menu;
using Pokete.Models;
using Pokete.World;

namespace Pokete.Core;

/// <summary>
/// Orquestra o loop do mundo aberto: renderização, entrada, movimento, encontros
/// aleatórios com Posharp selvagens na grama alta, e treinadores parados no mapa
/// (bump para conversar/lutar). Toda batalha - selvagem ou de treinador - é
/// totalmente resolvida por <see cref="Pokete.Battle.BattleInstance"/>: HUD,
/// animações, ataques, captura, troca de Posharp ativo, XP, level up e evolução.
/// </summary>
public class GameEngine
{
    private readonly Renderer _renderer = new();
    private readonly InputHandler _input = new();
    private readonly Random _rng = new();

    private GameMap _map;
    private readonly Player _player;
    private bool _running = true;
    private string? _pendingMessage;

    public GameEngine(Player player, GameMap map)
    {
        _player = player;
        _map = map;
    }

    public void Run()
    {
        while (_running)
        {
            _renderer.DrawMap(_map, _player.X, _player.Y);
            string status = $"{_player.Name} | Deck: {_player.Deck.Count}/6 | ${_player.Money} | [1] Deck  [E] Settings  [Esc] Quit";
            _renderer.ShowMessage(_pendingMessage is not null ? $"{status} | {_pendingMessage}" : status);
            _pendingMessage = null;

            HandleAction(_input.ReadAction());
        }
    }

    private void HandleAction(GameAction action)
    {
        (int dx, int dy) = action switch
        {
            GameAction.MoveUp => (0, -1),
            GameAction.MoveDown => (0, 1),
            GameAction.MoveLeft => (-1, 0),
            GameAction.MoveRight => (1, 0),
            _ => (0, 0)
        };

        if (dx != 0 || dy != 0)
        {
            TryMove(dx, dy);
            return;
        }

        switch (action)
        {
            case GameAction.OpenDeck:
                ShowDeck();
                _renderer.Invalidate();
                break;
            case GameAction.Cancel:
                _running = false;
                break;
        }
    }

    private void TryMove(int dx, int dy)
    {
        int newX = _player.X + dx;
        int newY = _player.Y + dy;

        var target = _map.At(newX, newY);

        // Trainers stand as solid tiles; walking into one starts a conversation
        // (and, the first time, a battle) instead of just being blocked like a wall.
        if (target is { TrainerId: { } trainerId })
        {
            InteractWithTrainer(trainerId);
            return;
        }

        // The Center's nurse heals the whole team on bump - no battle, no menu.
        if (target is { IsHealer: true })
        {
            foreach (var posharp in _player.Deck) posharp.FullRecoverHealthAndPP();
            DialogueBar.Show(["Welcome to the Posharp Center!", "There we go - your team is fully healed and ready to go!"]);
            return;
        }

        // A door can sit on open ground (every outdoor route/cave transition ported
        // from the original map data: walk onto it and you're through) or on a solid
        // tile, like a building's own front door (the Posharp Center: walk up next to
        // it, then press toward the building once more to bump your way in, without
        // ever actually standing on the building itself).
        var door = _map.GetDoor(newX, newY);
        if (door is not null && GeneratedMaps.All.ContainsKey(door.TargetMap))
        {
            if (_map.IsWalkable(newX, newY))
            {
                _player.X = newX;
                _player.Y = newY;
            }
            ChangeMap(door.TargetMap, door.TargetX, door.TargetY);
            return;
        }

        if (!_map.IsWalkable(newX, newY)) return;

        _player.X = newX;
        _player.Y = newY;

        if (_map.HasBall(newX, newY))
        {
            _map.CollectBall(newX, newY);
            _player.Inventory.Add("poketeball", 1);
            _pendingMessage = "You found a Poketeball!";
        }

        var tile = _map.At(newX, newY);
        // "Ao entrar na grama alta (;), você pode ser atacado por um Posharp selvagem."
        if (tile is { IsTallGrass: true } && _player.HasUsablePosharp && _rng.NextDouble() < 0.15)
        {
            StartWildEncounter();
        }
    }

    /// <summary>Teleporta o jogador para outro mapa, reconstruindo-o a partir do layout real portado.</summary>
    private void ChangeMap(string mapId, int x, int y)
    {
        _map = SampleMapBuilder.Build(mapId);
        _player.CurrentMapId = mapId;
        _player.X = x;
        _player.Y = y;
        _pendingMessage = $"Entered {_map.DisplayName}";
        _renderer.Invalidate();
    }

    private void StartWildEncounter()
    {
        var roll = SampleMapBuilder.RollWildEncounter(_map.Id, _rng);
        if (roll is null) return;
        var (speciesId, level) = roll.Value;

        if (!GeneratedPosharpEspecies.All.TryGetValue(speciesId, out var species)) return;

        var wild = new PosharpInstance(species, level: level, xp: PosharpInstance.XpForLevel(level));
        BattleResult result = RunBattle(new WildEncounter(wild), isWildBattle: true);

        _pendingMessage = result switch
        {
            BattleResult.PlayerWon => "You won the battle!",
            BattleResult.PlayerCaught => $"You caught {wild.Name}!",
            BattleResult.PlayerFled => "You got away safely.",
            BattleResult.PlayerLost => "You blacked out and hurried home to recover...",
            _ => null
        };
    }

    private void InteractWithTrainer(string trainerId)
    {
        var definition = GeneratedTrainers.All[trainerId];

        if (_player.DefeatedTrainerIds.Contains(trainerId))
        {
            DialogueBar.Show(definition.PostFightDialogue is [.., var lastLine] ? lastLine : $"{definition.Name}: ...");
            return;
        }

        DialogueBar.Show(definition.PreFightDialogue);

        var npc = NpcTrainer.FromDefinition(definition);
        BattleResult result = RunBattle(npc, isWildBattle: false);

        if (result == BattleResult.PlayerWon)
        {
            _player.DefeatedTrainerIds.Add(trainerId);
            DialogueBar.Show(definition.PostFightDialogue);
        }

        _pendingMessage = result switch
        {
            BattleResult.PlayerWon => $"You defeated {definition.Name}!",
            BattleResult.PlayerLost => "You blacked out and hurried home to recover...",
            _ => null
        };
    }

    /// <summary>Runs one battle to completion, healing the team on a loss so an all-fainted team never soft-locks the player (no Pokete Center is wired into the world yet).</summary>
    private BattleResult RunBattle(TrainerBase opponent, bool isWildBattle)
    {
        ConsoleScreen.ClearScreen(); // entering a battle is a real screen change

        BattleResult result = new BattleInstance(_player, opponent, isWildBattle).Start();

        if (result == BattleResult.PlayerLost)
        {
            foreach (var posharp in _player.Deck) posharp.FullRecoverHealthAndPP();
        }

        ConsoleScreen.ClearScreen(); // and so is leaving one, back to the world map
        _renderer.Invalidate();
        return result;
    }

    private void ShowDeck()
    {
        var lines = _player.Deck.Count == 0
            ? new List<string> { "(empty)" }
            : _player.Deck.Select(p => $"{p.Name} (Lv.{p.Level}) HP {p.CurrentHealthPoints}/{p.MaxHealthPoints}").ToList();
        lines.Add("Back");
        MenuSystem.Choose("Your deck", lines);
    }
}
