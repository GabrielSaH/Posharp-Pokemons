using Pokete.Data.Generated;
using Pokete.Moves;
using Pokete.Moves.Effects;

namespace Pokete.Models;

/// <summary>
/// A concrete, owned Posharp. Stats are derived from the species' base stats, a
/// per-instance set of random "individual values" (0-31, rolled once at creation
/// or restored from a save) and the current level, using the same style of
/// formula real creature-collector games use:
///   stat = ((2 * base + iv) * level / 100) + 5   (+ level, +10 for HP)
/// </summary>
public class PosharpInstance
{
    public PosharpSpecies Species { get; private set; }
    public string Name { get; set; }
    public int Xp { get; set; }
    public List<MoveInstance> Moves { get; set; }


    public int Level { get; set; }


    public int MaxHealthPoints { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Initiative { get; set; }
    public int SpecialAttack { get; set; }
    public int SpecialDefense { get; set; }


    public int AttackIndividualValue { get; init; }
    public int SpecialAttackIndividualValue { get; init; }
    public int DefenseIndividualValue { get; init; }
    public int SpecialDefenseIndividualValue { get; init; }
    public int InitiativeIndividualValue { get; init; }
    public int HealthIndividualValue { get; init; }


    public int CurrentHealthPoints { get; set; }
    public double CurrentMissChance { get; set; }
    public double CurrentAttack { get; set; }
    public double CurrentDefense { get; set; }
    public double CurrentInitiative { get; set; }
    public double CurrentSpecialAttack { get; set; }
    public double CurrentSpecialDefense { get; set; }

    /// <summary>Highest level a Posharp can reach, mirroring the traditional level cap.</summary>
    public const int MaxLevel = 100;

    public PosharpInstance(
        PosharpSpecies species,
        string? name = null,
        int level = 1,
        int xp = 0,
        int? attackIv = null,
        int? defenseIv = null,
        int? specialAttackIv = null,
        int? specialDefenseIv = null,
        int? initiativeIv = null,
        int? healthIv = null)
    {
        Species = species;
        Name = name ?? species.Name;
        Level = Math.Clamp(level, 1, MaxLevel);
        Moves = new(4);
        Xp = xp;

        // Individual values (0-31) give two Posharp of the same species slightly different
        // stats, the same way real creature-collector games do it. When restoring a saved
        // Posharp, the exact saved values are passed in instead of being re-rolled.
        AttackIndividualValue = attackIv ?? Random.Shared.Next(0, 32);
        SpecialAttackIndividualValue = specialAttackIv ?? Random.Shared.Next(0, 32);
        DefenseIndividualValue = defenseIv ?? Random.Shared.Next(0, 32);
        SpecialDefenseIndividualValue = specialDefenseIv ?? Random.Shared.Next(0, 32);
        InitiativeIndividualValue = initiativeIv ?? Random.Shared.Next(0, 32);
        HealthIndividualValue = healthIv ?? Random.Shared.Next(0, 32);

        RemakeStats();
        ResetCurrentModifiers();
        SetMovesByLevel();
    }


    public bool IsFainted => CurrentHealthPoints <= 0;
    public void TakeDamage(int damage) => CurrentHealthPoints = Math.Clamp(CurrentHealthPoints - damage, 0, MaxHealthPoints);
    public void HealHealth(int heal) => CurrentHealthPoints = Math.Clamp(CurrentHealthPoints + heal, 0, MaxHealthPoints);
    public void FullHeal() => CurrentHealthPoints = MaxHealthPoints;
    public void MoveRecoverPP(int moveIndex, int ppRecover) => Moves[moveIndex].RecoverPP(ppRecover);
    public void MoveFullRecoverPP(int moveIndex) => Moves[moveIndex].RecoverFullPP();
    public void AllMovesRecoverPP(int ppRecover) => Moves.ForEach(move => move.RecoverPP(ppRecover));
    public void AllMovesFullRecoverPP() => Moves.ForEach(move => move.RecoverFullPP());
    public void FullRecoverHealthAndPP()
    {
        FullHeal();
        AllMovesFullRecoverPP();
    }

    /// <summary>Used only when evolving (swaps the species definition in place).</summary>
    public void SetSpecies(PosharpSpecies species) => Species = species;

    public void ApplyStatDebuff(StatType stat, int percentage)
    {
        double multiplier = Math.Clamp((100 - percentage) / 100.0, 0, 1);
        switch (stat)
        {
            case StatType.Attack:
                CurrentAttack *= multiplier;
                break;
            case StatType.Defense:
                CurrentDefense *= multiplier;
                break;
            case StatType.SpecialAttack:
                CurrentSpecialAttack *= multiplier;
                break;
            case StatType.SpecialDefense:
                CurrentSpecialDefense *= multiplier;
                break;
            case StatType.Initiative:
                CurrentInitiative *= multiplier;
                break;
            case StatType.Accuracy:
                CurrentMissChance = Math.Clamp(CurrentMissChance + percentage / 100.0, 0, 1);
                break;
        }
    }

    public void RemakeStats()
    {
        Attack = ((2 * Species.Attack + AttackIndividualValue) * Level / 100) + 5;
        Defense = ((2 * Species.Defense + DefenseIndividualValue) * Level / 100) + 5;
        Initiative = ((2 * Species.Initiative + InitiativeIndividualValue) * Level / 100) + 5;
        SpecialAttack = ((2 * Species.SpecialAttack + SpecialAttackIndividualValue) * Level / 100) + 5;
        SpecialDefense = ((2 * Species.SpecialDefense + SpecialDefenseIndividualValue) * Level / 100) + 5;

        MaxHealthPoints = ((2 * Species.HealthPoints + HealthIndividualValue) * Level / 100) + Level + 10;
    }

    private void ResetCurrentModifiers()
    {
        CurrentHealthPoints = MaxHealthPoints;
        CurrentMissChance = 0;
        CurrentAttack = Attack;
        CurrentDefense = Defense;
        CurrentInitiative = Initiative;
        CurrentSpecialAttack = SpecialAttack;
        CurrentSpecialDefense = SpecialDefense;
    }

    private void SetMovesByLevel()
    {
        string[] moveList = Species.PossibleNaturalMovesIds;

        for (var moveIndex = 0; moveIndex < moveList.Length; moveIndex++)
        {
            string moveId = moveList[moveIndex];
            if (GeneratedMoves.All[moveId].MinimumLevel > Level)
            {
                Moves = GetFourLastMovesByIndex(moveIndex);
                return;
            }
        }

        Moves = GetFourLastMovesByIndex(moveList.Length);
    }

    private List<MoveInstance> GetFourLastMovesByIndex(int index)
    {
        string[] moveList = Species.PossibleNaturalMovesIds;
        List<MoveInstance> output = [];

        for (int i = 1; i < 5 && index - i >= 0; i++)
        {
            string moveId = moveList[index - i];
            output.Add(new MoveInstance(GeneratedMoves.All[moveId]));
        }

        return output;
    }

    // ---- XP / leveling ----------------------------------------------------------
    // The original Posharp prototype tracked Xp but had no formula relating it to
    // Level. For the overworld game we need battles to actually grant progress, so
    // this uses a standard cubic growth curve (xpForLevel = level^3), the same
    // shape as the "Medium Fast" curve from mainstream creature-collector games.

    public static int XpForLevel(int level) => level * level * level;

    public static int LevelForXp(int xp) => Math.Clamp((int)Math.Cbrt(Math.Max(xp, 0)), 1, MaxLevel);

    /// <summary>
    /// Adds XP and, if it crosses into a new level, recalculates stats (keeping the same
    /// amount of "damage taken" rather than healing back to full) and learns any newly
    /// eligible moves - straight into a free slot if there's room, otherwise reported
    /// back as pending so the caller can ask the player what to forget.
    /// </summary>
    public LevelUpOutcome GainXp(int amount)
    {
        Xp += Math.Max(amount, 0);
        int newLevel = LevelForXp(Xp);
        if (newLevel <= Level) return new LevelUpOutcome(false, []);

        Level = newLevel;
        int oldMax = MaxHealthPoints;
        int oldAttack = Attack, oldDefense = Defense, oldInit = Initiative, oldSpA = SpecialAttack, oldSpD = SpecialDefense;

        RemakeStats();

        int hpGained = MaxHealthPoints - oldMax;
        CurrentHealthPoints = IsFainted ? CurrentHealthPoints : Math.Clamp(CurrentHealthPoints + hpGained, 1, MaxHealthPoints);

        // Current* stats carry temporary in-battle modifiers; nudge them by the same
        // delta the base stat grew by instead of clobbering an active debuff/buff.
        CurrentAttack += Attack - oldAttack;
        CurrentDefense += Defense - oldDefense;
        CurrentInitiative += Initiative - oldInit;
        CurrentSpecialAttack += SpecialAttack - oldSpA;
        CurrentSpecialDefense += SpecialDefense - oldSpD;

        List<string> pending = LearnNewlyEligibleMoves();
        return new LevelUpOutcome(true, pending);
    }

    /// <summary>Adds every move now unlocked by the current level that isn't already known - straight into a free slot if there's room, otherwise returns its id as pending.</summary>
    private List<string> LearnNewlyEligibleMoves()
    {
        var knownIds = Moves.Select(m => m.BaseMove.Id).ToHashSet();
        var pending = new List<string>();

        foreach (string moveId in Species.PossibleNaturalMovesIds)
        {
            if (knownIds.Contains(moveId)) continue;
            if (GeneratedMoves.All[moveId].MinimumLevel > Level) continue;

            if (Moves.Count < 4)
            {
                Moves.Add(new MoveInstance(GeneratedMoves.All[moveId]));
                knownIds.Add(moveId);
            }
            else
            {
                pending.Add(moveId);
            }
        }

        return pending;
    }

    /// <summary>Learns a pending move, replacing whatever currently sits in <paramref name="replaceIndex"/>.</summary>
    public void LearnMove(string moveId, int replaceIndex) =>
        Moves[replaceIndex] = new MoveInstance(GeneratedMoves.All[moveId]);
}
