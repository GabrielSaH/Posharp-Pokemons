using Pokete.Models;
using Pokete.Moves.Effects.Interfaces;

namespace Pokete.Moves.Effects.Implementations;

public class SpecialDamageEffect(int accuracy, int basePower) : IMoveEffect, IDamageEffect
{
    
    public int Accuracy { get; init; } =  accuracy;
    public int BasePower { get; init;} =  basePower;
    
    public MoveResult Execute(PosharpInstance user, PosharpInstance enemy)
    {
        int rollForAtack = Random.Shared.Next(0, 100);
        if (rollForAtack > Accuracy)
        {
            return new MissDamageResult(MoveOutcome.Missed, user, enemy);
        }

        int damage = CalculateBaseDamage(user, enemy);
        bool isCrit = Random.Shared.Next(0, 11) == 10;

        enemy.TakeDamage(isCrit ? (int)Math.Round((damage * 1.5)) : damage);
        return new DamageResult(MoveOutcome.Success, damage, isCrit,  user, enemy);
    }

    public int CalculateBaseDamage(PosharpInstance user, PosharpInstance enemy)
    {
        int levelBasedDamage = (int)Math.Round(user.Level * 2.0 / 5 + 2);
        int attackAndDefenseDamage = (int)Math.Round(user.CurrentSpecialAttack / enemy.CurrentSpecialDefense);
        int minimumDamage = 2;
        
        return levelBasedDamage * BasePower * attackAndDefenseDamage / 50 + minimumDamage;
    }
}
