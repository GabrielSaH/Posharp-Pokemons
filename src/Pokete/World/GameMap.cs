namespace Pokete.World;

using Pokete.Models;

/// <summary>
/// Um único mapa/área do mundo do jogo (ex: "Route 1", "Flowy Town").
/// Carregado a partir de um layout de texto simples mais uma legenda, no
/// mesmo espírito de como o maps.py do projeto original descreve mapas
/// como layouts ASCII.
/// </summary>
public class GameMap
{
    public required string Id { get; init; }
    public required string DisplayName { get; init; }
    public int Width { get; init; }
    public int Height { get; init; }

    private readonly MapObject?[,] _tiles;
    public List<MapObject> Objects { get; } = new();

    private readonly Dictionary<(int X, int Y), DoorInfo> _doors = new();
    private readonly HashSet<(int X, int Y)> _balls = new();

    public void AddDoor(DoorInfo door) => _doors[(door.X, door.Y)] = door;
    public DoorInfo? GetDoor(int x, int y) => _doors.GetValueOrDefault((x, y));

    public void AddBall(int x, int y) => _balls.Add((x, y));
    public bool HasBall(int x, int y) => _balls.Contains((x, y));
    public void CollectBall(int x, int y) => _balls.Remove((x, y));

    [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
    public GameMap(string id, string displayName, int width, int height)
    {
        Id = id;
        DisplayName = displayName;
        Width = width;
        Height = height;
        _tiles = new MapObject?[width, height];
    }

    public void Set(int x, int y, MapObject obj)
    {
        obj.X = x;
        obj.Y = y;
        _tiles[x, y] = obj;
        Objects.Add(obj);
    }

    public MapObject? At(int x, int y)
    {
        if (x < 0 || y < 0 || x >= Width || y >= Height) return null;
        return _tiles[x, y];
    }

    public bool IsWalkable(int x, int y)
    {
        if (x < 0 || y < 0 || x >= Width || y >= Height) return false;
        var obj = _tiles[x, y];
        return obj is null || !obj.IsSolid;
    }

    /// <summary>Carrega um layout ASCII retangular usando uma legenda de símbolos, ex: '#' = parede, ';' = grama alta.</summary>
    public static GameMap FromLayout(string id, string displayName, string[] rows, Dictionary<char, Func<MapObject>> legend)
    {
        int height = rows.Length;
        int width = rows.Max(r => r.Length);
        var map = new GameMap(id, displayName, width, height);

        for (int y = 0; y < height; y++)
        {
            var row = rows[y];
            for (int x = 0; x < row.Length; x++)
            {
                char c = row[x];
                if (legend.TryGetValue(c, out var factory))
                {
                    map.Set(x, y, factory());
                }
            }
        }

        return map;
    }
}
