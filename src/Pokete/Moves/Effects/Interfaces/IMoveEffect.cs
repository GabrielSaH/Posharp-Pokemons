using Pokete.Models;

namespace Pokete.Moves.Effects.Interfaces;

public interface IMoveEffect
{
    MoveResult Execute(PosharpInstance user, PosharpInstance enemy);
}