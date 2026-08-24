using Pokete.Models;
using Pokete.Moves.Effects;

namespace Pokete.Moves;

public enum MoveOutcome
{
    Success,
    Missed,
    Immune,
    Failed
}

public abstract record MoveResult(MoveOutcome Outcome)
{
    public bool IsSuccess => Outcome == MoveOutcome.Success;

    public abstract string GetLogMessage();
}

public record DamageResult(
    MoveOutcome Outcome,
    int DamageDealt,
    bool IsCriticalHit,
    PosharpInstance User,
    PosharpInstance Target
) : MoveResult(Outcome)
{
    public override string GetLogMessage() =>
        $"{(IsCriticalHit ? "Critical Hit!" : "")}{User.Name} Dealt : {DamageDealt} damage to {Target.Name}";
}


public record MissDamageResult(
    MoveOutcome Outcome,
    PosharpInstance User,
    PosharpInstance Target
) : MoveResult(Outcome)
{
    public override string GetLogMessage() =>
        $"Its a miss! {User.Name} Missed {Target.Name} and dealt 0 Damage!";
}


public record HealResult(
    MoveOutcome Outcome,
    int HealthRecovered,
    PosharpInstance User
) : MoveResult(Outcome)
{
    public override string GetLogMessage() =>
        $"{User.Name} tidied up its allocations and recovered {HealthRecovered} HP!";
}

public record StatusResult(
    MoveOutcome Outcome,
    StatType AffectedStat,
    int Percentage,
    PosharpInstance User,
    PosharpInstance Target
) : MoveResult(Outcome)
{
    public override string GetLogMessage() =>
        $"{Target.Name}'s {AffectedStat} fell by {Percentage}%!";
}





