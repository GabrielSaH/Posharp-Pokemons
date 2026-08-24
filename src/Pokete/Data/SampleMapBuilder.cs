using Pokete.Data.Generated;
using Pokete.Models;
using Pokete.World;

namespace Pokete.Data;

/// <summary>
/// Monta um GameMap jogável a partir dos dados reais de layout portados de
/// src/pokete/data/map_data.py do projeto original (paredes, casas, árvores,
/// grama alta, portas entre mapas, itens no chão), combinados com os
/// metadados reais de maps.py (nome de exibição, pool de poketes selvagens)
/// em GeneratedMaps.
/// </summary>
public static class SampleMapBuilder
{
    public static GameMap Build(string mapId)
    {
        if (!GeneratedMaps.All.TryGetValue(mapId, out var info))
            throw new ArgumentException($"Unknown map id '{mapId}'");

        if (!GeneratedMapLayouts.All.TryGetValue(mapId, out var layout))
            return BuildFallback(mapId, info);

        var map = new GameMap(info.Id, info.PrettyName, layout.Width, layout.Height);

        for (int y = 0; y < layout.Height; y++)
        {
            string hardRow = y < layout.HardRows.Length ? layout.HardRows[y] : string.Empty;
            string softRow = y < layout.SoftRows.Length ? layout.SoftRows[y] : string.Empty;

            for (int x = 0; x < layout.Width; x++)
            {
                char hardCh = x < hardRow.Length ? hardRow[x] : ' ';
                if (hardCh != ' ')
                {
                    map.Set(x, y, new MapObject { Symbol = hardCh, Color = HardColor(hardCh), IsSolid = true });
                    continue;
                }

                char softCh = x < softRow.Length ? softRow[x] : ' ';
                if (softCh != ' ')
                {
                    bool isGrass = softCh == ';';
                    map.Set(x, y, new MapObject
                    {
                        Symbol = softCh,
                        Color = isGrass ? ConsoleColor.Green : ConsoleColor.DarkYellow,
                        IsSolid = false,
                        IsTallGrass = isGrass
                    });
                }
            }
        }

        foreach (var door in layout.Doors) map.AddDoor(door);
        foreach (var (x, y) in layout.Balls) map.AddBall(x, y);

        AddCustomFeatures(map, mapId);
        AddTrainers(map, mapId);

        return map;
    }

    /// <summary>Stamps every trainer whose <see cref="TrainerDefinition.MapId"/> matches this map onto its tile, solid, tagged for <see cref="GameEngine"/> to recognize on bump.</summary>
    private static void AddTrainers(GameMap map, string mapId)
    {
        foreach (var trainer in GeneratedTrainers.All.Values.Where(t => t.MapId == mapId))
        {
            map.Set(trainer.X, trainer.Y, new MapObject
            {
                Symbol = trainer.Symbol,
                Color = ConsoleColor.Red,
                IsSolid = true,
                TrainerId = trainer.Id
            });
        }
    }

    /// <summary>
    /// Extras adicionados manualmente que não fazem parte do map_data.py original,
    /// incluídos a pedido. Mantenha essa lista pequena - é para retoques pontuais,
    /// não conteúdo real de mapa.
    /// </summary>
    private static void AddCustomFeatures(GameMap map, string mapId)
    {
        switch (mapId)
        {
            case "playmap_1":
                // Pequena lagoa em formato de losango no campo aberto perto do Pokete center.
                DrawPond(map, startX: 30, startY: 8, new[]
                {
                    "  ''\"\"''  ",
                    " ''\"\"\"\"'' ",
                    "''\"\"\"\"\"\"''",
                    "''\"\"\"\"\"\"''",
                    " ''\"\"\"\"'' ",
                    "  ''\"\"''  ",
                });

                // Entrance into the Posharp Center building: the "___" gap between the
                // two '#' pillars on the building's face (row y=3, x=24-26), directly
                // above the walkable notch in its base wall (x=25,y=4) where the player
                // actually stands to face it - matches the real ported building art, not
                // an arbitrary point in front of it. All three tiles bump-enter, since
                // approaching from slightly either side should still work.
                map.AddDoor(new DoorInfo(24, 3, "pokete_center_1", 7, 8));
                map.AddDoor(new DoorInfo(25, 3, "pokete_center_1", 7, 8));
                map.AddDoor(new DoorInfo(26, 3, "pokete_center_1", 7, 8));
                break;

            case "pokete_center_1":
                // The nurse who heals your whole team (HP and move PP) when you talk to her.
                map.Set(7, 3, new MapObject
                {
                    Symbol = 'N',
                    Color = ConsoleColor.Cyan,
                    IsSolid = true,
                    IsHealer = true
                });
                break;

            case "playmap_51":
                // Lagoa mais alongada e de formato orgânico/irregular no campo aberto de Route 0.
                DrawPond(map, startX: 10, startY: 3, new[]
                {
                    "   ''\"\"''      ",
                    "  ''\"\"\"\"'''    ",
                    " ''\"\"\"\"\"\"\"''   ",
                    "''\"\"\"\"\"\"\"\"\"''  ",
                    " '\"\"\"\"\"\"\"\"\"\"''",
                    "  ''\"\"\"\"\"\"\"\"'' ",
                    "   '''\"\"\"\"'''  ",
                    "      ''''     ",
                });

                // Trecho de barro dentro do campo de grama alta, para poketes do tipo Terra
                // aparecerem depois. Sobrescreve totalmente a grama aqui (sem células vazias).
                DrawMud(map, startX: 50, startY: 28, new[]
                {
                    ",.,.,.,.,.,.,.",
                    ".,.,.,.,.,.,.,",
                    ",.,.,.,.,.,.,.",
                    ".,.,.,.,.,.,.,",
                    ",.,.,.,.,.,.,.",
                    ".,.,.,.,.,.,.,",
                });
                break;

            case "playmap_3":
                // Lagoa larga e achatada (oval) no quadrado central de Sunnydale,
                // entre o Pokete Center, a Loja e as duas casas.
                DrawPond(map, startX: 32, startY: 7, new[]
                {
                    "  ''\"\"\"\"\"\"''   ",
                    " ''\"\"\"\"\"\"\"\"\"'' ",
                    "''\"\"\"\"\"\"\"\"\"\"\"''",
                    " ''\"\"\"\"\"\"\"\"\"'' ",
                });
                break;
        }
    }

    /// <summary>
    /// Estampa um formato de lagoa (aspas = água, espaço = intocado) no mapa em
    /// azul. Andável (água rasa) - não sólido, e marcado via MapObject.IsWater
    /// para que encontros com poketes aquáticos possam ser ligados depois.
    /// </summary>
    private static void DrawPond(GameMap map, int startX, int startY, string[] shape)
    {
        for (int y = 0; y < shape.Length; y++)
        {
            for (int x = 0; x < shape[y].Length; x++)
            {
                char c = shape[y][x];
                if (c == ' ') continue;
                map.Set(startX + x, startY + y, new MapObject
                {
                    Symbol = c,
                    Color = ConsoleColor.Blue,
                    IsSolid = false,
                    IsWater = true
                });
            }
        }
    }

    /// <summary>
    /// Estampa um trecho de barro/terra no mapa em marrom (DarkYellow - o mais
    /// próximo de marrom entre as cores nativas do ConsoleColor). Andável - não
    /// sólido, e marcado via MapObject.IsMud para que encontros com poketes do
    /// tipo Terra possam ser ligados depois. Diferente de DrawPond, toda célula
    /// do formato é preenchida (sem espaços transparentes), já que o objetivo é
    /// sobrescrever totalmente um trecho retangular de terreno.
    /// </summary>
    private static void DrawMud(GameMap map, int startX, int startY, string[] shape)
    {
        for (int y = 0; y < shape.Length; y++)
        {
            for (int x = 0; x < shape[y].Length; x++)
            {
                map.Set(startX + x, startY + y, new MapObject
                {
                    Symbol = shape[y][x],
                    Color = ConsoleColor.DarkYellow,
                    IsSolid = false,
                    IsMud = true
                });
            }
        }
    }

    /// <summary>
    /// Cor por glifo, seguindo a aparência do original (ver print de referência):
    /// copa de árvore '(' ')' em verde, troncos '|' em cinza/branco, o resto
    /// (cercas, construções) em cinza escuro.
    /// </summary>
    private static ConsoleColor HardColor(char c) => c switch
    {
        '(' or ')' => ConsoleColor.DarkGreen,
        '|' => ConsoleColor.Gray,
        _ => ConsoleColor.DarkGray
    };

    /// <summary>Fallback simples com borda, usado só se um id de mapa não tiver dados reais de layout.</summary>
    private static GameMap BuildFallback(string mapId, MapInfo info)
    {
        var map = new GameMap(info.Id, info.PrettyName, Math.Max(info.Width, 10), Math.Max(info.Height, 8));
        for (int x = 0; x < map.Width; x++)
        {
            map.Set(x, 0, new MapObject { Symbol = '#', Color = ConsoleColor.DarkGray, IsSolid = true });
            map.Set(x, map.Height - 1, new MapObject { Symbol = '#', Color = ConsoleColor.DarkGray, IsSolid = true });
        }
        for (int y = 0; y < map.Height; y++)
        {
            map.Set(0, y, new MapObject { Symbol = '#', Color = ConsoleColor.DarkGray, IsSolid = true });
            map.Set(map.Width - 1, y, new MapObject { Symbol = '#', Color = ConsoleColor.DarkGray, IsSolid = true });
        }
        return map;
    }

    /// <summary>Sorteia um id de espécie selvagem + nível para o mapa dado, ou null se ele não tiver spawns.</summary>
    public static (string speciesId, int level)? RollWildEncounter(string mapId, Random rng)
    {
        if (!GeneratedMaps.All.TryGetValue(mapId, out var info) || info.PokeArgs is null)
            return null;

        var pool = info.PokeArgs.Pokes;
        string species = pool[rng.Next(pool.Length)];
        int level = rng.Next(info.PokeArgs.MinLevel, info.PokeArgs.MaxLevel + 1);
        return (species, level);
    }

    /// <summary>
    /// Onde a player appears when they arrive at a map: the map's real "special door"
    /// (main entrance) from map_data.py if it exists and is walkable, else the first
    /// walkable tile found scanning outward from the map's center.
    /// </summary>
    public static (int X, int Y) FindSpawnPoint(string mapId)
    {
        var map = Build(mapId);

        if (GeneratedMapLayouts.All.TryGetValue(mapId, out var layout) && layout.SpecialDoors.Count > 0)
        {
            var (sx, sy) = layout.SpecialDoors[0];
            if (map.IsWalkable(sx, sy)) return (sx, sy);
        }

        int cx = map.Width / 2, cy = map.Height / 2;
        for (int radius = 0; radius < Math.Max(map.Width, map.Height); radius++)
        for (int dy = -radius; dy <= radius; dy++)
        for (int dx = -radius; dx <= radius; dx++)
        {
            int x = cx + dx, y = cy + dy;
            if (map.IsWalkable(x, y)) return (x, y);
        }

        return (1, 1);
    }

    /// <summary>Builds the player's current map, nudging them onto a valid spawn point first if their saved position is no longer walkable.</summary>
    public static GameMap BuildForPlayer(Player player)
    {
        var map = Build(player.CurrentMapId);
        if (!map.IsWalkable(player.X, player.Y))
        {
            (player.X, player.Y) = FindSpawnPoint(player.CurrentMapId);
        }
        return map;
    }
}
