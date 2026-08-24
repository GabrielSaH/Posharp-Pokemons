using Pokete.Data.Generated;

namespace Pokete.Models;

/// <summary>
/// Classe base compartilhada pelo jogador e pelos treinadores NPC: qualquer um
/// que tenha um deck de Posharp e possa entrar em batalhas.
/// </summary>
public abstract class TrainerBase
{
    public string Name { get; set; } = string.Empty;

    /// <summary>Até 6 Posharp ativamente utilizáveis em batalha (o "deck").</summary>
    public List<PosharpInstance> Deck { get; } = new(6);

    public IEnumerable<PosharpInstance> UsablePosharps => Deck.Where(p => !p.IsFainted);

    public bool HasUsablePosharp => UsablePosharps.Any();
}

/// <summary>
/// Um treinador NPC adversário, parado em um mapa até o jogador esbarrar nele.
/// Sempre construído a partir de um <see cref="TrainerDefinition"/> via
/// <see cref="FromDefinition"/> - nunca à mão - para garantir que o time seja
/// montado corretamente a partir da espécie/nível declarados.
/// </summary>
public class NpcTrainer : TrainerBase
{
    public required string MapId { get; init; }
    public bool Defeated { get; set; }
    public int Money { get; init; }

    /// <summary>Linhas de diálogo exibidas antes da luta começar.</summary>
    public List<string> PreFightDialogue { get; init; } = new();
    public List<string> PostFightDialogue { get; init; } = new();

    /// <summary>Builds a battle-ready trainer (with a freshly-built, full-health team) from a static blueprint.</summary>
    public static NpcTrainer FromDefinition(TrainerDefinition definition)
    {
        var trainer = new NpcTrainer
        {
            Name = definition.Name,
            MapId = definition.MapId,
            Money = definition.Money,
            PreFightDialogue = definition.PreFightDialogue.ToList(),
            PostFightDialogue = definition.PostFightDialogue.ToList(),
        };

        foreach (var (speciesId, level) in definition.Team)
        {
            var species = GeneratedPosharpEspecies.All[speciesId];
            trainer.Deck.Add(new PosharpInstance(species, level: level, xp: PosharpInstance.XpForLevel(level)));
        }

        return trainer;
    }
}

/// <summary>
/// O treinador controlado pelo jogador. Guarda a Posharp-dex completa, o
/// inventário, as conquistas e a posição atual, refletindo a estrutura do
/// arquivo de save pokete.json original.
/// </summary>
public class Player : TrainerBase
{
    /// <summary>Todo Posharp já capturado, no deck ou não (a "Posharp dex").</summary>
    public List<PosharpInstance> Storage { get; } = new();

    public Inventory Inventory { get; } = new();
    public HashSet<string> UnlockedAchievements { get; } = new();
    public HashSet<string> SeenSpecies { get; } = new();
    public HashSet<string> CaughtSpecies { get; } = new();

    /// <summary>Trainer NPCs the player has already beaten, so they don't fight again.</summary>
    public HashSet<string> DefeatedTrainerIds { get; } = new();

    public int Money { get; set; }

    public string CurrentMapId { get; set; } = "playmap_1";
    public int X { get; set; }
    public int Y { get; set; }

    public TimeSpan Playtime { get; set; } = TimeSpan.Zero;
    public DateTime StartupTime { get; set; } = DateTime.UtcNow;
}
