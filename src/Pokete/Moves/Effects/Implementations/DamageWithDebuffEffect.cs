using Pokete.Models;
using Pokete.Moves.Effects.Interfaces;

namespace Pokete.Moves.Effects.Implementations;

public class DamageWithDebuffEffect(
    int accuracy,
    int basePower,
    bool isSpecial,
    StatType affectedStat,
    int debuffPercentage,
    int debuffChance
    ) : IMoveEffect, IDamageEffect, IStatusOnEnemy
{
    
    public int Accuracy { get; init; } = accuracy;
    public int BasePower { get; init; } = basePower;
    public bool IsSpecial { get; init; } = isSpecial;
    public StatType AffectedStat { get; init; } = affectedStat;
    public int DebuffPercentage { get; init; } = debuffPercentage;
    public int DebuffChance { get; init; } = debuffChance;
    
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

        int rollForDebuff = Random.Shared.Next(0, 100);
        if (rollForDebuff < DebuffChance)
        {
            int appliedPercentage = CalculateDebuffPercentage(user, enemy);
            enemy.ApplyStatDebuff(AffectedStat, appliedPercentage);
        }

        return new DamageResult(MoveOutcome.Success, damage, isCrit, user, enemy);
    }

    public int CalculateBaseDamage(PosharpInstance user, PosharpInstance enemy)
    {
        int levelBasedDamage = (int)Math.Round(user.Level * 2.0 / 5 + 2);
        double attackStat = IsSpecial ? user.CurrentSpecialAttack : user.CurrentAttack;
        double defenseStat = IsSpecial ? enemy.CurrentSpecialDefense : enemy.CurrentDefense;
        int attackAndDefenseDamage = (int)Math.Round(attackStat / defenseStat);
        int minimumDamage = 2;
        
        return levelBasedDamage * BasePower * attackAndDefenseDamage / 50 + minimumDamage;
    }

    public int CalculateDebuffPercentage(PosharpInstance user, PosharpInstance enemy) => DebuffPercentage;
}
