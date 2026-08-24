using Pokete.Models;

namespace Pokete.Battle;

/// <summary>
/// Wraps a single wild Posharp as a one-member <see cref="TrainerBase"/> so
/// <see cref="BattleInstance"/> can treat wild encounters and trainer battles through
/// the exact same "keep sending out Posharp until one side has none left" loop,
/// instead of duplicating that logic for a special "single enemy" case.
/// </summary>
public sealed class WildEncounter : TrainerBase
{
    public WildEncounter(PosharpInstance wild)
    {
        Name = "Wild";
        Deck.Add(wild);
    }
}
