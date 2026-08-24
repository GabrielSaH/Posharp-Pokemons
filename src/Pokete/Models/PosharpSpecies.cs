namespace Pokete.Models;

public class PosharpSpecies(
    string id,
    string name,
    int healthPoints,
    int attack,
    int defense,
    int specialAttack,
    int specialDefense,
    string[] possibleNaturalMovesIds,
    int xpGainWhenDefeated,
    string[] types,
    int initiative,
    string description = "",
    string? idPosharpEvolvesInto = null,
    int evolveLvl = 0,
    string icon = "")
{
    
    public string Id { get; init; } = id;
    public string Name { get; init; } = name;
    
    
    public int HealthPoints { get; init; } = healthPoints;
    public int Initiative { get; init; } = initiative;
    public int Attack { get; init; } = attack;
    public int Defense { get; init; } = defense;
    public int SpecialAttack { get; init; } = specialAttack;
    public int SpecialDefense { get; init; } = specialDefense;
    
    
    public string[] PossibleNaturalMovesIds { get; init; } = possibleNaturalMovesIds;
    public string Description { get; init; } = description;
    
    
    public int XpGainWhenDefeated { get; init; } = xpGainWhenDefeated;
    public string[] Types { get; init; } = types;
    public string MainType { get; init; } = types[0];
    
    
    public string? IdPosharpEvolvesInto { get; init; } = idPosharpEvolvesInto;
    public int EvolveLvl { get; init; } = evolveLvl;

    /// <summary>Raw ASCII-art icon text.</summary>
    public string Icon { get; init; } = icon;

    
    public bool CanEvolve => !string.IsNullOrEmpty(IdPosharpEvolvesInto) && EvolveLvl > 0;
    
}






