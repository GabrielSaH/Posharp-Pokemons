using Pokete.Models;

namespace Pokete.Moves.Effects.Interfaces;

public interface IHealSelfEffect
{
    int CalculateHealAmount(PosharpInstance user);
}