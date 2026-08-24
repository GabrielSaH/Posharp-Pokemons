using Pokete.Moves;
using Pokete.Moves.Effects;

namespace Pokete.Battle;

/// <summary>
/// A flat, serializable record of one resolved move. Deliberately decoupled from the
/// live <see cref="Pokete_Pokemon.Instances.PosharpInstance"/> objects (names/values are
/// copied out) so a full battle's log can be exported to JSON without dragging along
/// entire species/move graphs.
/// </summary>
public record BattleLogEntry(
    int TurnIndex,
    string UserName,
    bool UserIsPlayer,
    string TargetName,
    string MoveName,
    MoveCategory MoveCategory,
    MoveOutcome Outcome,
    string Message,
    int? DamageDealt,
    bool? IsCriticalHit,
    int? HealthRecovered,
    StatType? AffectedStat,
    int? StatPercentage
);
