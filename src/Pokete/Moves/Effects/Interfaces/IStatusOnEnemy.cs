using Pokete.Models;

namespace Pokete.Moves.Effects.Interfaces;

public interface IStatusOnEnemy
{
    int CalculateDebuffPercentage(PosharpInstance user, PosharpInstance enemy);
}
