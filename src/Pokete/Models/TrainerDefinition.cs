namespace Pokete.Models;

/// <summary>
/// Static blueprint for an NPC trainer: who they are, where they stand, what team
/// they field, and what they say before/after the fight. This is the trainer
/// equivalent of <see cref="PosharpSpecies"/> - adding a new trainer to the game is
/// just adding one entry to <c>GeneratedTrainers.All</c>, the same way adding a new
/// Posharp is just adding one entry to <c>GeneratedPosharpEspecies.All</c>.
/// <see cref="NpcTrainer.FromDefinition"/> turns this blueprint into a live,
/// battle-ready <see cref="NpcTrainer"/>.
/// </summary>
public class TrainerDefinition(
    string id,
    string name,
    string mapId,
    int x,
    int y,
    char symbol,
    int money,
    (string SpeciesId, int Level)[] team,
    string[] preFightDialogue,
    string[]? postFightDialogue = null)
{
    public string Id { get; } = id;
    public string Name { get; } = name;

    /// <summary>Which map this trainer stands on, and where.</summary>
    public string MapId { get; } = mapId;
    public int X { get; } = x;
    public int Y { get; } = y;
    public char Symbol { get; } = symbol;

    /// <summary>Awarded to the player after a win.</summary>
    public int Money { get; } = money;

    /// <summary>The trainer's team, as (species id, level) pairs - built into real Posharp on demand.</summary>
    public (string SpeciesId, int Level)[] Team { get; } = team;

    public string[] PreFightDialogue { get; } = preFightDialogue;
    public string[] PostFightDialogue { get; } = postFightDialogue ?? [];
}
