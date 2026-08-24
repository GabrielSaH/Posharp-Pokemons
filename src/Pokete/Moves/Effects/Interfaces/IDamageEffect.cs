using Pokete.Models;

namespace Pokete.Moves.Effects.Interfaces;

public interface IDamageEffect
{
    int CalculateBaseDamage(PosharpInstance user, PosharpInstance enemy);
}