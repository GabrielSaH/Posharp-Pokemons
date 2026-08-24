namespace Pokete.Models;

/// <summary>
/// Result of <see cref="PosharpInstance.GainXp"/>. A move whose level requirement was
/// just reached is learned automatically while there's a free move slot; once all 4
/// slots are full, its id shows up in <see cref="PendingNewMoveIds"/> instead, and the
/// caller (the battle UI) is expected to ask the player what to do about it via
/// <see cref="PosharpInstance.LearnMove"/>.
/// </summary>
public record LevelUpOutcome(bool LeveledUp, List<string> PendingNewMoveIds);
