using Pokete.Models;

namespace Pokete.Battle;

/// <summary>
/// Decides whether a thrown ball catches a wild Posharp. Neither ported project had a
/// catch formula that matched the Posharp stat system, so this is new: the lower the
/// wild Posharp's current HP fraction and the stronger the ball, the better the odds,
/// clamped so a catch is never a sure thing nor truly hopeless.
/// </summary>
public static class CatchSystem
{
    public static readonly Dictionary<string, double> BallMultiplier = new()
    {
        ["poketeball"] = 1.0,
        ["superball"] = 2.5,
        ["hyperball"] = 6.0,
    };

    private const double BaseResistance = 3.0;
    private const double MinChance = 0.03;
    private const double MaxChance = 0.95;

    public static double GetCatchChance(PosharpInstance wild, double ballMultiplier)
    {
        if (wild.CurrentHealthPoints <= 0) return MaxChance;

        double hpFactor = (double)wild.MaxHealthPoints / Math.Max(wild.CurrentHealthPoints, 1);
        double weightTrue = hpFactor * ballMultiplier;
        double chance = weightTrue / (weightTrue + BaseResistance);

        return Math.Clamp(chance, MinChance, MaxChance);
    }

    public static bool AttemptCatch(PosharpInstance wild, double ballMultiplier) =>
        Random.Shared.NextDouble() < GetCatchChance(wild, ballMultiplier);
}
