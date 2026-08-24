using Pokete.Models;
using Pokete.Moves.Effects.Interfaces;

namespace Pokete.Moves.Effects.Implementations;

public class HealSelfEffect(int healPercentage) : IMoveEffect, IHealSelfEffect
{
    
    public int HealPercentage { get; init; } = healPercentage;
    
    public MoveResult Execute(PosharpInstance user, PosharpInstance enemy)
    {
        int healAmount = CalculateHealAmount(user);
        user.HealHealth(healAmount);
        return new HealResult(MoveOutcome.Success, healAmount, user);
    }

    public int CalculateHealAmount(PosharpInstance user) =>
        (int)Math.Round(user.MaxHealthPoints * (HealPercentage / 100.0));
}
