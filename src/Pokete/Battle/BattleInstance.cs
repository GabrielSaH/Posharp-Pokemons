using Pokete.Core;
using Pokete.Data.Generated;
using Pokete.Models;
using Pokete.Moves;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;

namespace Pokete.Battle;

/// <summary>
/// Runs a full battle in the console: showing the HUD, reading the player's choices
/// and resolving each turn, until one side has no usable Posharp left, the opponent's
/// (wild-only) Posharp is caught, or the player flees successfully. Turn feedback
/// plays automatically - no keypress is needed between messages.
/// <para>
/// Wild encounters and trainer battles are the same code path: <see cref="WildEncounter"/>
/// wraps a single wild Posharp as a one-member opponent, so both sides just keep
/// sending out their next usable Posharp until someone runs out.
/// </para>
/// <para>Usage: <c>BattleResult result = new BattleInstance(player, opponent, isWildBattle).Start();</c></para>
/// <para>The full move-by-move log can be exported afterward via <see cref="ExportLogToJson"/>.</para>
/// </summary>
public class BattleInstance
{
    private const int MinEscapeChancePercent = 10;
    private const int MaxEscapeChancePercent = 95;
    private const int BaseEscapeChancePercent = 50;

    private const int MessageDelayMs = 900;
    private const int AnimationFrameDelayMs = 120;
    private const int PhysicalFrameDelayMs = 170;
    private const int LungeDistance = 4;

    private readonly Player _trainer;
    private readonly TrainerBase _opponent;
    private readonly bool _isWildBattle;
    private PosharpInstance player;
    private PosharpInstance enemy;

    private readonly List<BattleLogEntry> _log = [];
    private string _lastMessage = "";
    private int _turnCounter;

    public IReadOnlyList<BattleLogEntry> Log => _log;

    public BattleInstance(Player trainer, TrainerBase opponent, bool isWildBattle = true)
    {
        _trainer = trainer;
        _opponent = opponent;
        _isWildBattle = isWildBattle;
        player = trainer.UsablePosharps.First();
        enemy = opponent.UsablePosharps.First();
    }

    public BattleResult Start()
    {
        ConsoleScreen.EnsureSize();
        PlayMessage(_isWildBattle ? $"A wild {enemy.Name} appeared!" : $"{_opponent.Name} wants to battle!");

        while (_trainer.HasUsablePosharp && _opponent.HasUsablePosharp)
        {
            if (player.IsFainted)
            {
                if (!ForceSwitchPlayer()) break;
                continue;
            }

            BattleAction action = BattleMenu.PromptMainMenu(player, enemy, _lastMessage);

            if (action == BattleAction.Attack) TryPlayerAttack();
            else if (action == BattleAction.Run && TryToFlee()) return BattleResult.PlayerFled;
            else if (action == BattleAction.Inventory && TryUseItem() == ItemOutcome.Caught) return BattleResult.PlayerCaught;
            else if (action == BattleAction.Deck) TrySwitchPlayer();

            if (enemy.IsFainted)
            {
                AwardKnockoutXp();
                if (_opponent.HasUsablePosharp) ForceSwitchEnemy();
            }
        }

        return FinishBattle();
    }

    /// <summary>Serializes the full move-by-move log to a JSON file.</summary>
    public void ExportLogToJson(string filePath)
    {
        JsonSerializerOptions options = new()
        {
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter() }
        };

        File.WriteAllText(filePath, JsonSerializer.Serialize(_log, options));
    }

    // ---- Attacking -------------------------------------------------------------------

    private void TryPlayerAttack()
    {
        List<MoveInstance> availableMoves = player.Moves.Where(move => move.IsAvailable()).ToList();
        if (availableMoves.Count == 0)
        {
            PlayMessage($"{player.Name} has no moves left to use!");
            return;
        }

        int? choice = BattleMenu.PromptMoveSelection(player, enemy, availableMoves);
        if (choice is null) return; // player backed out - no turn spent

        ResolveAttackRound(availableMoves[choice.Value]);
    }

    private void ResolveAttackRound(MoveInstance playerMove)
    {
        bool playerActsFirst = player.CurrentInitiative >= enemy.CurrentInitiative;

        if (playerActsFirst)
        {
            ExecuteMove(player, enemy, playerMove);
            if (!enemy.IsFainted) PerformEnemyAttackIfPossible();
        }
        else
        {
            PerformEnemyAttackIfPossible();
            if (!player.IsFainted) ExecuteMove(player, enemy, playerMove);
        }
    }

    private void PerformEnemyAttackIfPossible()
    {
        List<MoveInstance> availableMoves = enemy.Moves.Where(move => move.IsAvailable()).ToList();
        if (availableMoves.Count == 0)
        {
            PlayMessage($"{enemy.Name} has no moves left to use!");
            return;
        }

        MoveInstance move = availableMoves[Random.Shared.Next(availableMoves.Count)];
        ExecuteMove(enemy, player, move);
    }

    private void ExecuteMove(PosharpInstance user, PosharpInstance target, MoveInstance move)
    {
        bool userIsPlayer = ReferenceEquals(user, player);
        string announceMessage = $"{user.Name}({RoleOf(user)}) used {move.BaseMove.Name} against {target.Name}({RoleOf(target)})";

        PlayMoveAnimation(announceMessage, move.BaseMove.Category, userIsPlayer);

        MoveResult result = move.Execute(user, target);
        PlayMessage(result.GetLogMessage());

        _log.Add(BuildLogEntry(user, target, move, result));
    }

    // ---- Running away ------------------------------------------------------------

    private bool TryToFlee()
    {
        if (!_isWildBattle)
        {
            PlayMessage("You can't run from a trainer battle!");
            return false;
        }

        bool escaped = Random.Shared.Next(0, 100) < CalculateEscapeChancePercent();
        if (escaped)
        {
            PlayMessage("Got away safely!");
            return true;
        }

        PlayMessage("Couldn't get away!");
        PerformEnemyAttackIfPossible();
        return false;
    }

    private int CalculateEscapeChancePercent()
    {
        if (enemy.CurrentInitiative <= 0) return MaxEscapeChancePercent;

        double initiativeRatio = player.CurrentInitiative / enemy.CurrentInitiative;
        int chance = (int)Math.Round(BaseEscapeChancePercent * initiativeRatio);
        return Math.Clamp(chance, MinEscapeChancePercent, MaxEscapeChancePercent);
    }

    // ---- Items & catching -----------------------------------------------------------

    private enum ItemOutcome { Continue, Caught }

    private ItemOutcome TryUseItem()
    {
        var available = GeneratedItems.All.Values
            .Where(i => _trainer.Inventory.Count(i.Id) > 0)
            .ToList();

        if (available.Count == 0)
        {
            PlayMessage("You don't have any usable items!");
            return ItemOutcome.Continue;
        }

        var labels = available.Select(i => $"{i.PrettyName} x{_trainer.Inventory.Count(i.Id)}").ToList();
        labels.Add("Cancel");
        int? choice = BattleMenu.PromptSelection(player, enemy, "Use which item?", labels);
        if (choice is null || choice == available.Count) return ItemOutcome.Continue;

        ItemInfo item = available[choice.Value];

        switch (item.Fn)
        {
            case "poketeball" or "superball" or "hyperball":
                return AttemptCatchWithItem(item);
            case "heal_potion":
                _trainer.Inventory.TryUse(item.Id);
                player.HealHealth(5);
                PlayMessage($"{player.Name} recovered some HP!");
                PerformEnemyAttackIfPossible();
                return ItemOutcome.Continue;
            case "super_potion":
                _trainer.Inventory.TryUse(item.Id);
                player.HealHealth(15);
                PlayMessage($"{player.Name} recovered a lot of HP!");
                PerformEnemyAttackIfPossible();
                return ItemOutcome.Continue;
            case "ap_potion":
                _trainer.Inventory.TryUse(item.Id);
                player.AllMovesFullRecoverPP();
                PlayMessage($"{player.Name}'s moves were fully restored!");
                PerformEnemyAttackIfPossible();
                return ItemOutcome.Continue;
            default:
                PlayMessage("Nothing happened...");
                return ItemOutcome.Continue;
        }
    }

    private ItemOutcome AttemptCatchWithItem(ItemInfo ball)
    {
        if (!_isWildBattle)
        {
            PlayMessage("You can't catch another trainer's Posharp!");
            return ItemOutcome.Continue;
        }

        _trainer.Inventory.TryUse(ball.Id);
        PlayMessage($"You threw a {ball.PrettyName}!");

        double multiplier = CatchSystem.BallMultiplier.GetValueOrDefault(ball.Id, 1.0);
        if (CatchSystem.AttemptCatch(enemy, multiplier))
        {
            PlayMessage($"Gotcha! {enemy.Name} was caught!");
            _trainer.Storage.Add(enemy);
            _trainer.CaughtSpecies.Add(enemy.Species.Id);
            if (_trainer.Deck.Count < 6) _trainer.Deck.Add(enemy);
            return ItemOutcome.Caught;
        }

        PlayMessage($"{enemy.Name} broke free!");
        PerformEnemyAttackIfPossible();
        return ItemOutcome.Continue;
    }

    // ---- Switching active fighter -----------------------------------------------------

    private void TrySwitchPlayer()
    {
        var options = _trainer.Deck.Where(p => !p.IsFainted && !ReferenceEquals(p, player)).ToList();
        if (options.Count == 0)
        {
            PlayMessage("There's no one else able to fight!");
            return;
        }

        var labels = options.Select(p => $"{p.Name} (Lv.{p.Level}) HP {p.CurrentHealthPoints}/{p.MaxHealthPoints}").ToList();
        labels.Add("Cancel");
        int? choice = BattleMenu.PromptSelection(player, enemy, "Switch to which Posharp?", labels);
        if (choice is null || choice == options.Count) return;

        player = options[choice.Value];
        PlayMessage($"Go, {player.Name}!");
        PerformEnemyAttackIfPossible();
    }

    /// <summary>Called when the active Posharp faints but the trainer still has usable ones. Returns false if none remain.</summary>
    private bool ForceSwitchPlayer()
    {
        var usable = _trainer.Deck.Where(p => !p.IsFainted).ToList();
        if (usable.Count == 0) return false;

        PlayMessage($"{player.Name} fainted!");
        var labels = usable.Select(p => $"{p.Name} (Lv.{p.Level}) HP {p.CurrentHealthPoints}/{p.MaxHealthPoints}").ToList();
        int choice = BattleMenu.PromptSelection(player, enemy, "Choose your next Posharp!", labels, allowCancel: false)!.Value;

        player = usable[choice];
        PlayMessage($"Go, {player.Name}!");
        return true;
    }

    /// <summary>The opponent automatically sends out their next usable Posharp - no player input involved.</summary>
    private void ForceSwitchEnemy()
    {
        enemy = _opponent.Deck.First(p => !p.IsFainted);
        PlayMessage(_isWildBattle ? $"A wild {enemy.Name} appeared!" : $"{_opponent.Name} sent out {enemy.Name}!");
    }

    // ---- End-of-battle consequences ------------------------------------------------

    private void AwardKnockoutXp()
    {
        PlayMessage($"{enemy.Name} fainted!");

        int xpReward = enemy.Species.XpGainWhenDefeated + Math.Max(0, enemy.Level - player.Level);
        if (!_isWildBattle) xpReward *= 2;

        LevelUpOutcome outcome = player.GainXp(xpReward);
        PlayMessage($"{player.Name} gained {xpReward} XP!");

        if (outcome.LeveledUp)
        {
            PlayMessage($"{player.Name} grew to level {player.Level}!");
            foreach (string moveId in outcome.PendingNewMoveIds) PromptLearnMove(moveId);
            TryEvolve();
        }
    }

    private void PromptLearnMove(string newMoveId)
    {
        Move newMove = GeneratedMoves.All[newMoveId];
        var optionLabels = player.Moves.Select(m => $"Forget {m.BaseMove.Name}").ToList();
        optionLabels.Add($"Don't learn {newMove.Name}");

        string title = $"{player.Name} wants to learn {newMove.Name}!";
        string[] hint = ["It already knows 4 moves.", "Forget one, or give up learning it?"];

        int choice = BattleMenu.PromptSelection(player, enemy, title, optionLabels, hint, allowCancel: false)!.Value;

        if (choice < player.Moves.Count)
        {
            string forgotten = player.Moves[choice].BaseMove.Name;
            player.LearnMove(newMoveId, choice);
            PlayMessage($"{player.Name} forgot {forgotten} and learned {newMove.Name}!");
        }
        else
        {
            PlayMessage($"{player.Name} did not learn {newMove.Name}.");
        }
    }

    private void TryEvolve()
    {
        if (!player.Species.CanEvolve || player.Level < player.Species.EvolveLvl) return;
        if (!GeneratedPosharpEspecies.All.TryGetValue(player.Species.IdPosharpEvolvesInto!, out var evolved)) return;

        string oldName = player.Species.Name;
        player.SetSpecies(evolved);
        player.RemakeStats();
        PlayMessage($"What? {oldName} is evolving... Congratulations! It became {evolved.Name}!");
    }

    private BattleResult FinishBattle()
    {
        if (!_opponent.HasUsablePosharp)
        {
            PlayMessage(_isWildBattle ? "You won the battle!" : $"You defeated {_opponent.Name}!");

            if (_opponent is NpcTrainer { Money: > 0 } npc)
            {
                _trainer.Money += npc.Money;
                PlayMessage($"You got ${npc.Money} for winning!");
            }

            return BattleResult.PlayerWon;
        }

        PlayMessage("All your Posharp fainted! You blacked out...");
        return BattleResult.PlayerLost;
    }

    // ---- Presentation helpers (no input, auto-advancing) --------------------------

    private void PlayMessage(string message)
    {
        _lastMessage = message;
        BattleRenderer.RenderHud(player, enemy, message);
        Thread.Sleep(MessageDelayMs);
    }

    private void PlayMoveAnimation(string message, MoveCategory category, bool userIsPlayer)
    {
        switch (category)
        {
            case MoveCategory.Physical:
                PlayLungeAnimation(message, userIsPlayer);
                break;
            case MoveCategory.Status:
                PlaySparkleAnimation(message, userIsPlayer);
                break;
            case MoveCategory.Special:
                PlayFlashAnimation(message, userIsPlayer);
                break;
        }

        PlayMessage(message); // settle on a plain resting frame, held for the reader
    }

    private void PlayLungeAnimation(string message, bool userIsPlayer)
    {
        int direction = userIsPlayer ? 1 : -1; // player lunges right toward the enemy, enemy lunges left toward the player
        int chargeStep = Math.Max(1, LungeDistance / 2);
        int[] offsets = [direction * chargeStep, direction * LungeDistance, direction * LungeDistance, 0];

        foreach (int offset in offsets)
        {
            RenderWithAnimation(message, userIsPlayer, new IconAnimation(ColumnOffset: offset));
            Thread.Sleep(PhysicalFrameDelayMs);
        }
    }

    private void PlaySparkleAnimation(string message, bool userIsPlayer)
    {
        for (int i = 0; i < 2; i++)
        {
            RenderWithAnimation(message, userIsPlayer, new IconAnimation(Decoration: '*'));
            Thread.Sleep(AnimationFrameDelayMs);
            RenderWithAnimation(message, userIsPlayer, IconAnimation.None);
            Thread.Sleep(AnimationFrameDelayMs);
        }
    }

    private void PlayFlashAnimation(string message, bool userIsPlayer)
    {
        ConsoleColor[] colors = [ConsoleColor.Cyan, ConsoleColor.Magenta, ConsoleColor.Cyan];
        foreach (ConsoleColor color in colors)
        {
            RenderWithAnimation(message, userIsPlayer, new IconAnimation(Color: color));
            Thread.Sleep(AnimationFrameDelayMs);
        }
    }

    private void RenderWithAnimation(string message, bool userIsPlayer, IconAnimation animation)
    {
        if (userIsPlayer) BattleRenderer.RenderHud(player, enemy, message, playerAnimation: animation);
        else BattleRenderer.RenderHud(player, enemy, message, enemyAnimation: animation);
    }

    // ---- Small utilities -----------------------------------------------------------

    private BattleLogEntry BuildLogEntry(PosharpInstance user, PosharpInstance target, MoveInstance move, MoveResult result)
    {
        _turnCounter++;

        return new BattleLogEntry(
            TurnIndex: _turnCounter,
            UserName: user.Name,
            UserIsPlayer: ReferenceEquals(user, player),
            TargetName: target.Name,
            MoveName: move.BaseMove.Name,
            MoveCategory: move.BaseMove.Category,
            Outcome: result.Outcome,
            Message: result.GetLogMessage(),
            DamageDealt: (result as DamageResult)?.DamageDealt,
            IsCriticalHit: (result as DamageResult)?.IsCriticalHit,
            HealthRecovered: (result as HealResult)?.HealthRecovered,
            AffectedStat: (result as StatusResult)?.AffectedStat,
            StatPercentage: (result as StatusResult)?.Percentage
        );
    }

    private string RoleOf(PosharpInstance combatant) => ReferenceEquals(combatant, player) ? "you" : "enemy";
}
