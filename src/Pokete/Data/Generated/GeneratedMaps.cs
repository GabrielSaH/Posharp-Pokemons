// AUTO-GENERATED from src/pokete/data/maps.py - do not edit by hand.
// NOTE: only metadata is ported (dimensions, display name, wild-Pokete pool,
// weather, music). The original's pixel-perfect ASCII tile layouts live in
// src/pokete/data/map_data.py (~4200 lines) and were NOT ported - see
// PORTING_NOTES.md for why and how to add real layouts.
using System.Collections.Generic;
using Pokete.Models;

namespace Pokete.Data.Generated;

public static class GeneratedMaps
{
    public static readonly Dictionary<string, MapInfo> All = new()
    {
        ["intromap"] = new MapInfo("intromap", "Your home", 15, 30, "03 Chibi Ninja.mp3", null, null),
        ["playmap_1"] = new MapInfo("playmap_1", "Nice Town", 25, 91, "03 Chibi Ninja.mp3", null, new PokeSpawnInfo(new[] { "Pisharp", "Splashfin", "Duskowl" }, 2, 5)),
        ["playmap_51"] = new MapInfo("playmap_51", "Route 0", 40, 110, "xDeviruchi - Exploring The Unknown.mp3", null, new PokeSpawnInfo(new[] { "Sproutling", "Tadpaw", "Chitterwing", "Splashfin" }, 3, 6)),
        ["cave_1"] = new MapInfo("cave_1", "Nice Town Cave", 30, 90, "08 Ascending.mp3", null, new PokeSpawnInfo(new[] { "Cragmaw", "Wisp", "Pisharp", "Grimlatch" }, 4, 10)),
        ["playmap_2"] = new MapInfo("playmap_2", "Route 1", 30, 180, "03 Chibi Ninja.mp3", null, new PokeSpawnInfo(new[] { "EmberFang", "Voltcell", "Duskowl", "Pisharp", "Sproutling" }, 5, 10)),
        ["playmap_3"] = new MapInfo("playmap_3", "Sunnydale", 30, 90, "xDeviruchi - Exploring The Unknown.mp3", null, new PokeSpawnInfo(new[] { "Rootling", "Splashfin", "Vipertongue", "Voltcell", "Cragmaw" }, 8, 15)),
        ["playmap_4"] = new MapInfo("playmap_4", "Sunnydale Lake", 60, 60, "xDeviruchi - Exploring The Unknown.mp3", null, new PokeSpawnInfo(new[] { "Splashfin", "Tadpaw", "Duskowl", "Wisp" }, 12, 18)),
        ["playmap_5"] = new MapInfo("playmap_5", "Mysterious Cave", 60, 60, "02 Underclocked (underunderclocked mix).mp3", null, new PokeSpawnInfo(new[] { "Wisp", "Grimlatch", "Cragmaw" }, 12, 18)),
        ["playmap_6"] = new MapInfo("playmap_6", "Route 2", 60, 60, "01 A Night Of Dizzy Spells.mp3", null, new PokeSpawnInfo(new[] { "Vipertongue", "EmberFang", "Rootling", "Wyrmlet" }, 14, 20)),
        ["playmap_7"] = new MapInfo("playmap_7", "Dark Cave", 30, 60, "02 Underclocked (underunderclocked mix).mp3", null, new PokeSpawnInfo(new[] { "Grimlatch", "Wisp", "Cragmaw", "Mirage" }, 14, 20)),
        ["playmap_8"] = new MapInfo("playmap_8", "Abandoned Village", 20, 80, "xDeviruchi - Mysterious Dungeon.mp3", "foggy", new PokeSpawnInfo(new[] { "Wisp", "Grimlatch", "Duskowl", "Mirage" }, 16, 22)),
        ["playmap_9"] = new MapInfo("playmap_9", "Abandoned House", 15, 30, "xDeviruchi - Mysterious Dungeon.mp3", null, new PokeSpawnInfo(new[] { "Wisp", "Grimlatch" }, 16, 22)),
        ["playmap_10"] = new MapInfo("playmap_10", "Old House", 15, 30, "Map.mp3", null, null),
        ["playmap_11"] = new MapInfo("playmap_11", "Route 3", 20, 60, "xDeviruchi - Take some rest and eat some food!.mp3", null, new PokeSpawnInfo(new[] { "Rootling", "EmberFang", "Voltcell", "Duskowl" }, 16, 22)),
        ["playmap_12"] = new MapInfo("playmap_12", "Route 4", 15, 80, "xDeviruchi - Take some rest and eat some food!.mp3", null, new PokeSpawnInfo(new[] { "Vipertongue", "Sproutling", "Wyrmlet", "Mirage" }, 22, 32)),
        ["playmap_13"] = new MapInfo("playmap_13", "Deepest Forest", 35, 70, "xDeviruchi - Title Theme .mp3", null, new PokeSpawnInfo(new[] { "Rootling", "Sproutling", "Chitterwing", "Mirage", "Wyrmlet" }, 22, 32)),
        ["playmap_14"] = new MapInfo("playmap_14", "Arena", 15, 30, "xDeviruchi - Prepare for Battle! .mp3", null, null),
        ["playmap_15"] = new MapInfo("playmap_15", "Route 5", 25, 120, "xDeviruchi - Title Theme .mp3", null, new PokeSpawnInfo(new[] { "Voltcell", "Permafrost", "EmberFang", "Chitterwing" }, 28, 38)),
        ["playmap_16"] = new MapInfo("playmap_16", "Route 6", 17, 65, "xDeviruchi - Title Theme .mp3", null, new PokeSpawnInfo(new[] { "Permafrost", "Voltcell", "Wyrmlet", "Vipertongue" }, 32, 42)),
        ["playmap_17"] = new MapInfo("playmap_17", "Old House", 15, 30, "Map.mp3", null, null),
        ["playmap_18"] = new MapInfo("playmap_18", "Big Mountain Sea", 23, 98, "xDeviruchi - Title Theme .mp3", null, new PokeSpawnInfo(new[] { "Splashfin", "Cragmaw", "Permafrost", "Tadpaw" }, 36, 46)),
        ["playmap_19"] = new MapInfo("playmap_19", "Big Mountain Cave", 30, 60, "10 Arpanauts.mp3", null, new PokeSpawnInfo(new[] { "Cragmaw", "Permafrost", "Grimlatch", "Pisharp" }, 36, 46)),
        ["playmap_20"] = new MapInfo("playmap_20", "House", 15, 30, "Map.mp3", null, null),
        ["playmap_21"] = new MapInfo("playmap_21", "Rock-ville", 30, 150, "xDeviruchi - The Final of The Fantasy.mp3", null, null),
        ["playmap_22"] = new MapInfo("playmap_22", "Rocky Hotel", 15, 30, "Map.mp3", null, null),
        ["playmap_23"] = new MapInfo("playmap_23", "Rocky Hotel", 15, 30, "Map.mp3", null, null),
        ["playmap_24"] = new MapInfo("playmap_24", "House", 15, 30, "Map.mp3", null, null),
        ["playmap_25"] = new MapInfo("playmap_25", "Town Hall", 15, 30, "Map.mp3", null, null),
        ["playmap_26"] = new MapInfo("playmap_26", "Town Hall", 15, 30, "Map.mp3", null, null),
        ["playmap_27"] = new MapInfo("playmap_27", "Battle Cave", 15, 40, "xDeviruchi - The Icy Cave .mp3", null, null),
        ["playmap_28"] = new MapInfo("playmap_28", "Route 7", 55, 198, "xDeviruchi - Take some rest and eat some food!.mp3", "rain", new PokeSpawnInfo(new[] { "Rootling", "Vipertongue", "Tadpaw", "Chitterwing", "Splashfin" }, 42, 52)),
        ["playmap_29"] = new MapInfo("playmap_29", "House", 15, 30, "Map.mp3", null, null),
        ["playmap_30"] = new MapInfo("playmap_30", "Flowy Town", 63, 148, "xDeviruchi - Take some rest and eat some food!.mp3", null, null),
        ["playmap_31"] = new MapInfo("playmap_31", "Arena", 14, 40, "xDeviruchi - Prepare for Battle! .mp3", null, null),
        ["playmap_32"] = new MapInfo("playmap_32", "House", 15, 30, "Map.mp3", null, null),
        ["playmap_33"] = new MapInfo("playmap_33", "Mowcow Meadow", 44, 154, "xDeviruchi - Minigame .mp3", null, new PokeSpawnInfo(new[] { "Sproutling", "Rootling", "Chitterwing" }, 52, 62)),
        ["playmap_34"] = new MapInfo("playmap_34", "House", 15, 30, "Map.mp3", null, null),
        ["playmap_35"] = new MapInfo("playmap_35", "The Fields of Agrawos", 69, 144, "xDeviruchi - Minigame .mp3", null, new PokeSpawnInfo(new[] { "Rootling", "Sproutling", "Wyrmlet", "Mirage" }, 58, 72)),
        ["playmap_36"] = new MapInfo("playmap_36", "House", 15, 30, "Map.mp3", null, null),
        ["playmap_37"] = new MapInfo("playmap_37", "House", 15, 30, "Map.mp3", null, null),
        ["playmap_38"] = new MapInfo("playmap_38", "House", 15, 30, "Map.mp3", null, null),
        ["playmap_39"] = new MapInfo("playmap_39", "Agrawos", 70, 200, "xDeviruchi - And The Journey Begins .mp3", null, new PokeSpawnInfo(new[] { "Rootling", "Sproutling", "Wyrmlet", "Vipertongue" }, 62, 82)),
        ["playmap_40"] = new MapInfo("playmap_40", "Sunny Beach", 30, 140, "xDeviruchi - Exploring The Unknown.mp3", "sunny", new PokeSpawnInfo(new[] { "Splashfin", "Tadpaw", "Cragmaw" }, 78, 92)),
        ["playmap_41"] = new MapInfo("playmap_41", "House", 15, 60, "Map.mp3", null, null),
        ["playmap_42"] = new MapInfo("playmap_42", "MowCow-Burger Restaurant", 15, 60, "Map.mp3", null, null),
        ["playmap_43"] = new MapInfo("playmap_43", "The Temple of the Wheeto", 15, 30, "Map.mp3", null, null),
        ["playmap_44"] = new MapInfo("playmap_44", "Town Hall", 15, 30, "Map.mp3", null, null),
        ["playmap_45"] = new MapInfo("playmap_45", "Town Hall", 15, 30, "Map.mp3", null, null),
        ["playmap_46"] = new MapInfo("playmap_46", "Arena of Agrawos", 15, 30, "xDeviruchi - Prepare for Battle! .mp3", null, null),
        ["playmap_47"] = new MapInfo("playmap_47", "House", 15, 30, "Map.mp3", null, null),
        ["playmap_48"] = new MapInfo("playmap_48", "House", 15, 30, "Map.mp3", null, null),
        ["playmap_49"] = new MapInfo("playmap_49", "House", 15, 30, "Map.mp3", null, null),
        ["playmap_50"] = new MapInfo("playmap_50", "Pokete-Care", 15, 30, "Map.mp3", null, null),
        ["pokete_center_1"] = new MapInfo("pokete_center_1", "Posharp Center", 10, 15, "Map.mp3", null, null),
    };
}