using Pokete.Moves.Effects.Interfaces;

namespace Pokete.Moves;

public enum MoveCategory { Physical, Special, Status }

public class Move(
    string id,
    string name,
    MoveCategory category,
    int basePower,
    int accuracy,
    int basePp,
    int minimumLevel,
    string description = "")
{
    public string Id { get; init; } = id;
    public string Name { get; init;} = name;
    public MoveCategory Category { get; init;} = category;
    public int BasePower { get; init;} = basePower;
    public int Accuracy { get; init;} = accuracy;
    public int BasePp { get; init;} = basePp;
    public string Description { get; init;} = description;
    public int MinimumLevel { get; init;} = minimumLevel;

    public required IMoveEffect Effect { get; init; }
}