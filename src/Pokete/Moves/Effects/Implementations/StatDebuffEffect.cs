using Pokete.Models;
using Pokete.Moves.Effects.Interfaces;

namespace Pokete.Moves.Effects.Implementations;

public class StatDebuffEffect(int accuracy, StatType affectedStat, int debuffPercentage) : IMoveEffect, IStatusOnEnemy
{
    
    public int Accuracy { get; init; } = accuracy;
    public StatType AffectedStat { get; init; } = affectedStat;
    public int DebuffPercentage { get; init; } = debuffPercentage;
    
    public MoveResult Execute(PosharpInstance user, PosharpInstance enemy)
    {
        int rollForAtack = Random.Shared.Next(0, 100);
        if (rollForAtack > Accuracy)
        {
            return new MissDamageResult(MoveOutcome.Missed, user, enemy);
        }

        int appliedPercentage = CalculateDebuffPercentage(user, enemy);
        enemy.ApplyStatDebuff(AffectedStat, appliedPercentage);

        return new StatusResult(MoveOutcome.Success, AffectedStat, appliedPercentage, user, enemy);
    }

    public int CalculateDebuffPercentage(PosharpInstance user, PosharpInstance enemy) => DebuffPercentage;
}
