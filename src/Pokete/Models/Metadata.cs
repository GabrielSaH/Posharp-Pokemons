namespace Pokete.Models;

public class AchievementInfo
{
    public string Id { get; }
    public string Title { get; }
    public string Description { get; }

    public AchievementInfo(string id, string title, string description)
    {
        Id = id;
        Title = title;
        Description = description;
    }
}

/// <summary>Condição climática que aplica um multiplicador de dano por tipo de ataque, portada de weather.py.</summary>
public class WeatherInfo
{
    public string Id { get; }
    public string Info { get; }
    public Dictionary<string, double> Effected { get; }

    public WeatherInfo(string id, string info, Dictionary<string, double> effected)
    {
        Id = id;
        Info = info;
        Effected = effected;
    }

    /// <summary>Multiplicador de dano para um tipo de ataque dado sob esse clima (1.0 se não afetado).</summary>
    public double Effect(string attackType) => Effected.GetValueOrDefault(attackType, 1.0);
}

/// <summary>Quais poketes selvagens podem aparecer em um mapa, e sua faixa de nível. Portado de maps.py "poke_args".</summary>
public class PokeSpawnInfo
{
    public string[] Pokes { get; }
    public int MinLevel { get; }
    public int MaxLevel { get; }

    public PokeSpawnInfo(string[] pokes, int minLevel, int maxLevel)
    {
        Pokes = pokes;
        MinLevel = minLevel;
        MaxLevel = maxLevel;
    }
}

/// <summary>Metadados de mapa (dimensões, nome de exibição, música, clima, spawns selvagens), portados de maps.py.</summary>
public class MapInfo
{
    public string Id { get; }
    public string PrettyName { get; }
    public int Height { get; }
    public int Width { get; }
    public string? Song { get; }
    public string? Weather { get; }
    public PokeSpawnInfo? PokeArgs { get; }

    public MapInfo(string id, string prettyName, int height, int width, string? song, string? weather, PokeSpawnInfo? pokeArgs)
    {
        Id = id;
        PrettyName = prettyName;
        Height = height;
        Width = width;
        Song = song;
        Weather = weather;
        PokeArgs = pokeArgs;
    }
}

/// <summary>Uma célula de porta/teletransporte ligando a uma posição específica em outro mapa. Portada de map_data.py "dors".</summary>
public class DoorInfo
{
    public int X { get; }
    public int Y { get; }
    public string TargetMap { get; }
    public int TargetX { get; }
    public int TargetY { get; }

    public DoorInfo(int x, int y, string targetMap, int targetX, int targetY)
    {
        X = x;
        Y = y;
        TargetMap = targetMap;
        TargetX = targetX;
        TargetY = targetY;
    }
}

/// <summary>
/// O layout ASCII real de um mapa, portado de src/pokete/data/map_data.py:
///   - HardRows: obstáculos sólidos (paredes, casas, árvores) - bloqueiam movimento.
///   - SoftRows: decoração/grama alta - andável, ';' dispara encontros selvagens.
///   - Doors: células de teletransporte para outro mapa.
///   - SpecialDoors: o marcador de entrada principal/spawn do mapa.
///   - Balls: Poketeballs no chão para coletar.
/// </summary>
public class MapLayout
{
    public int Width { get; }
    public int Height { get; }
    public string[] HardRows { get; }
    public string[] SoftRows { get; }
    public List<DoorInfo> Doors { get; }
    public List<(int X, int Y)> SpecialDoors { get; }
    public List<(int X, int Y)> Balls { get; }

    public MapLayout(int width, int height, string[] hardRows, string[] softRows,
        List<DoorInfo> doors, List<(int, int)> specialDoors, List<(int, int)> balls)
    {
        Width = width;
        Height = height;
        HardRows = hardRows;
        SoftRows = softRows;
        Doors = doors;
        SpecialDoors = specialDoors;
        Balls = balls;
    }
}

public class ItemInfo
{
    public string Id { get; }
    public string PrettyName { get; }
    public string Description { get; }
    public int? Price { get; }
    /// <summary>Identificador da função de efeito que este item dispara (ex: "poketeball", "heal_potion").</summary>
    public string? Fn { get; }

    public ItemInfo(string id, string prettyName, string description, int? price, string? fn)
    {
        Id = id;
        PrettyName = prettyName;
        Description = description;
        Price = price;
        Fn = fn;
    }
}
