using Pokete.Models;

namespace Pokete.Data.Generated;

public class GeneratedPosharpEspecies
{
    public static readonly Dictionary<string, PosharpSpecies> All = new()
    {
        ["Pisharp"] = new PosharpSpecies(
            id: "Pisharp",
            name: "Pisharp",
            healthPoints: 36,
            attack: 52,
            defense: 40,
            specialAttack: 30,
            specialDefense: 27,
            possibleNaturalMovesIds:
            [
                "Scratch",           // Level 0
                "PointerJab",        // Level 3
                "AutoSave",          // Level 4
                "RefSlash",          // Level 6
                "ByteBite",          // Level 8
                "GarbageCollector",  // Level 10
                "StackOverflow",     // Level 14
                "PatchNotes",        // Level 17
                "SqlInjection",      // Level 21
                "HardReset",         // Level 24
                "RateLimit",         // Level 27
                "BufferOverflow",    // Level 30
                "FirewallBreach",    // Level 33
                "CheckpointRestore", // Level 32
                "Ddos",              // Level 36
                "ForcePush",         // Level 38
                "Deconstructor",     // Level 42
                "BlueScreen",        // Level 48
            ],
            xpGainWhenDefeated: 45,
            types: ["Steel", "Normal"],
            initiative: 28,
            description: "A bladed Pokete forged from scrap metal. Its arms were compiled sharp and lean, and it cuts straight to the point.",
            icon:
            """
             _____ 
            \|'ᵕ'|/
             ‾‾‾‾‾
            """
        ),


        // ==============================================================
        // Evolution line: Ember Wolf
        // ==============================================================

        ["EmberFang"] = new PosharpSpecies(
            id: "EmberFang",
            name: "EmberFang",
            healthPoints: 17,
            attack: 30,
            defense: 15,
            specialAttack: 22,
            specialDefense: 13,
            possibleNaturalMovesIds:
            [
                "Tackle",      // Level 0
                "Bite",        // Level 0
                "Growl",       // Level 5
                "Leer",        // Level 10
                "Recover",     // Level 10
                "QuickStrike", // Level 15
                "HeadCharge",  // Level 20
                "Regenerate",  // Level 20
                "Screech",     // Level 20
                "CrushClaw",   // Level 25
                "Flamethrower",// Level 30
                "Intimidate",  // Level 30
                "SecondWind",  // Level 35
                "Overdrive",   // Level 35
                "Guillotine",  // Level 40
                "CrackingRoar",// Level 40
                "LastStand",   // Level 45
                "Cataclysm",   // Level 50
            ],
            xpGainWhenDefeated: 38,
            types: ["Fire", "Normal"],
            initiative: 25,
            description: "A young wolf-like Pokete with fur that smolders faintly at the tips. It nips first and asks questions never.",
            idPosharpEvolvesInto: "CinderFang",
            evolveLvl: 22,
            icon:
            """
               ^---^
               (   )
               >(.)<
            """
        ),

        ["CinderFang"] = new PosharpSpecies(
            id: "CinderFang",
            name: "CinderFang",
            healthPoints: 28,
            attack: 48,
            defense: 25,
            specialAttack: 38,
            specialDefense: 23,
            possibleNaturalMovesIds:
            [
                "Tackle",      // Level 0
                "Bite",        // Level 0
                "Growl",       // Level 5
                "Leer",        // Level 10
                "Recover",     // Level 10
                "QuickStrike", // Level 15
                "HeadCharge",  // Level 20
                "Regenerate",  // Level 20
                "Screech",     // Level 20
                "CrushClaw",   // Level 25
                "Flamethrower",// Level 30
                "Intimidate",  // Level 30
                "SecondWind",  // Level 35
                "Overdrive",   // Level 35
                "Guillotine",  // Level 40
                "CrackingRoar",// Level 40
                "LastStand",   // Level 45
                "Cataclysm",   // Level 50
            ],
            xpGainWhenDefeated: 55,
            types: ["Fire", "Normal"],
            initiative: 32,
            description: "Its coat has hardened into something between fur and embers. When it howls, sparks scatter across the ground.",
            icon:
            """
               \^-^/
               {   }
               >{.}<
            """
        ),


        // ==============================================================
        // Evolution line: Permafrost
        // ==============================================================

        ["Permafrost"] = new PosharpSpecies(
            id: "Permafrost",
            name: "Permafrost",
            healthPoints: 16,
            attack: 18,
            defense: 20,
            specialAttack: 22,
            specialDefense: 20,
            possibleNaturalMovesIds:
            [
                "Tackle",     // Level 0
                "Bite",       // Level 0
                "RockThrow",  // Level 5
                "Growl",      // Level 5
                "Leer",       // Level 10
                "Recover",    // Level 10
                "FrostBite",  // Level 15
                "QuickStrike",// Level 15
                "Regenerate", // Level 20
                "Screech",    // Level 20
                "IceShard",   // Level 25
                "CrushClaw",  // Level 25
                "Intimidate", // Level 30
                "ShellBash",  // Level 30
                "SecondWind", // Level 35
                "Overdrive",  // Level 35
                "CrackingRoar",// Level 40
                "Avalanche",  // Level 45
                "LastStand",  // Level 45
            ],
            xpGainWhenDefeated: 40,
            types: ["Ice", "Normal"],
            initiative: 10,
            description: "A block of ice roughly shaped like an animal. Nobody has ever seen what, if anything, lives inside.",
            idPosharpEvolvesInto: "Glacikeep",
            evolveLvl: 25,
            icon:
            """
               -----
               |   |
               -----
            """
        ),

        ["Glacikeep"] = new PosharpSpecies(
            id: "Glacikeep",
            name: "Glacikeep",
            healthPoints: 24,
            attack: 28,
            defense: 32,
            specialAttack: 30,
            specialDefense: 30,
            possibleNaturalMovesIds:
            [
                "Tackle",     // Level 0
                "Bite",       // Level 0
                "RockThrow",  // Level 5
                "Growl",      // Level 5
                "Leer",       // Level 10
                "Recover",    // Level 10
                "FrostBite",  // Level 15
                "QuickStrike",// Level 15
                "Regenerate", // Level 20
                "Screech",    // Level 20
                "IceShard",   // Level 25
                "CrushClaw",  // Level 25
                "Intimidate", // Level 30
                "ShellBash",  // Level 30
                "SecondWind", // Level 35
                "Overdrive",  // Level 35
                "CrackingRoar",// Level 40
                "Avalanche",  // Level 45
                "LastStand",  // Level 45
            ],
            xpGainWhenDefeated: 58,
            types: ["Ice", "Normal"],
            initiative: 14,
            description: "The ice has grown thick enough to survive a summer. Legends say a heart still beats somewhere near its core.",
            idPosharpEvolvesInto: "Cryocube",
            evolveLvl: 45,
            icon:
            """
              -------
              |     |
              -------
            """
        ),

        ["Cryocube"] = new PosharpSpecies(
            id: "Cryocube",
            name: "Cryocube",
            healthPoints: 32,
            attack: 44,
            defense: 48,
            specialAttack: 46,
            specialDefense: 44,
            possibleNaturalMovesIds:
            [
                "Tackle",     // Level 0
                "Bite",       // Level 0
                "RockThrow",  // Level 5
                "Growl",      // Level 5
                "Leer",       // Level 10
                "Recover",    // Level 10
                "FrostBite",  // Level 15
                "QuickStrike",// Level 15
                "Regenerate", // Level 20
                "Screech",    // Level 20
                "IceShard",   // Level 25
                "CrushClaw",  // Level 25
                "Intimidate", // Level 30
                "ShellBash",  // Level 30
                "SecondWind", // Level 35
                "Overdrive",  // Level 35
                "CrackingRoar",// Level 40
                "Avalanche",  // Level 45
                "LastStand",  // Level 45
            ],
            xpGainWhenDefeated: 90,
            types: ["Ice", "Normal"],
            initiative: 20,
            description: "Glacikeep's ice grew so dense it stopped reflecting light the way ice should, and one day it simply wasn't shaped like two dimensions anymore.",
            icon:
            """
               +-----+
              /     /|
             +-----+ |
             |     | +
             |     |/
             +-----+
            """
        ),


        // ==============================================================
        // Evolution line: Vipertongue
        // ==============================================================

        ["Vipertongue"] = new PosharpSpecies(
            id: "Vipertongue",
            name: "Vipertongue",
            healthPoints: 15,
            attack: 30,
            defense: 13,
            specialAttack: 24,
            specialDefense: 13,
            possibleNaturalMovesIds:
            [
                "Tackle",     // Level 0
                "Bite",       // Level 0
                "Growl",      // Level 5
                "PoisonSting",// Level 10
                "Leer",       // Level 10
                "Recover",    // Level 10
                "QuickStrike",// Level 15
                "SandAttack", // Level 15
                "Screech",    // Level 20
                "Regenerate", // Level 20
                "CrushClaw",  // Level 25
                "Intimidate", // Level 30
                "SecondWind", // Level 35
                "Overdrive",  // Level 35
                "CrackingRoar",// Level 40
                "LastStand",  // Level 45
                "Cataclysm",  // Level 50
            ],
            xpGainWhenDefeated: 38,
            types: ["Poison", "Normal"],
            initiative: 30,
            description: "A slender, venomous Pokete that prefers striking from tall grass to any sort of fair fight.",
            idPosharpEvolvesInto: "Coilfang",
            evolveLvl: 28,
            icon:
            """
              >'({{{
              }}}}}}}
             {{{{{{{{{
            """
        ),

        ["Coilfang"] = new PosharpSpecies(
            id: "Coilfang",
            name: "Coilfang",
            healthPoints: 26,
            attack: 50,
            defense: 22,
            specialAttack: 36,
            specialDefense: 22,
            possibleNaturalMovesIds:
            [
                "Tackle",     // Level 0
                "Bite",       // Level 0
                "Growl",      // Level 5
                "PoisonSting",// Level 10
                "Leer",       // Level 10
                "Recover",    // Level 10
                "QuickStrike",// Level 15
                "SandAttack", // Level 15
                "Screech",    // Level 20
                "Regenerate", // Level 20
                "CrushClaw",  // Level 25
                "Intimidate", // Level 30
                "SecondWind", // Level 35
                "Overdrive",  // Level 35
                "CrackingRoar",// Level 40
                "LastStand",  // Level 45
                "Cataclysm",  // Level 50
            ],
            xpGainWhenDefeated: 58,
            types: ["Poison", "Normal"],
            initiative: 40,
            description: "Fully grown, its coils can crush stone. It rarely needs the venom anymore.",
            icon:
            """
             _______
            /____ * \\
             (   \\   \\
            \\______   \\
            """
        ),


        // ==============================================================
        // Evolution line: Rootling
        // ==============================================================

        ["Rootling"] = new PosharpSpecies(
            id: "Rootling",
            name: "Rootling",
            healthPoints: 18,
            attack: 18,
            defense: 28,
            specialAttack: 20,
            specialDefense: 24,
            possibleNaturalMovesIds:
            [
                "Tackle",    // Level 0
                "Bite",      // Level 0
                "VineWhip",  // Level 5
                "Growl",     // Level 5
                "Leer",      // Level 10
                "Recover",   // Level 10
                "RazorLeaf", // Level 15
                "SandAttack",// Level 15
                "Regenerate",// Level 20
                "StoneEdge", // Level 20
                "EarthSlam", // Level 25
                "Intimidate",// Level 30
                "Landslide", // Level 35
                "SecondWind",// Level 35
                "Guillotine",// Level 40
                "Avalanche", // Level 45
                "LastStand", // Level 45
                "Cataclysm", // Level 50
            ],
            xpGainWhenDefeated: 35,
            types: ["Plant", "Ground"],
            initiative: 12,
            description: "A small plant Pokete that spends most of its life with only its leaves poking above the soil.",
            idPosharpEvolvesInto: "Deeproot",
            evolveLvl: 24,
            icon:
            """
             .__ / __.
              \\_\\|/_/
               /o o\\
               \\ - /
            """
        ),

        ["Deeproot"] = new PosharpSpecies(
            id: "Deeproot",
            name: "Deeproot",
            healthPoints: 30,
            attack: 30,
            defense: 45,
            specialAttack: 32,
            specialDefense: 40,
            possibleNaturalMovesIds:
            [
                "Tackle",    // Level 0
                "Bite",      // Level 0
                "VineWhip",  // Level 5
                "Growl",     // Level 5
                "Leer",      // Level 10
                "Recover",   // Level 10
                "RazorLeaf", // Level 15
                "SandAttack",// Level 15
                "Regenerate",// Level 20
                "StoneEdge", // Level 20
                "EarthSlam", // Level 25
                "Intimidate",// Level 30
                "Landslide", // Level 35
                "SecondWind",// Level 35
                "Guillotine",// Level 40
                "Avalanche", // Level 45
                "LastStand", // Level 45
                "Cataclysm", // Level 50
            ],
            xpGainWhenDefeated: 58,
            types: ["Plant", "Ground"],
            initiative: 16,
            description: "Its root system stretches for meters underground; uprooting it is said to be nearly impossible.",
            icon:
            """
             .__   __.
              \\_\\_/_/
               /o o\\
               \\ - /
            """
        ),


        // ==============================================================
        // Standalone species
        // ==============================================================

        ["Splashfin"] = new PosharpSpecies(
            id: "Splashfin",
            name: "Splashfin",
            healthPoints: 16,
            attack: 12,
            defense: 12,
            specialAttack: 16,
            specialDefense: 14,
            possibleNaturalMovesIds:
            [
                "Tackle",    // Level 0
                "WaterGun",  // Level 0
                "Bite",      // Level 0
                "BubbleBeam",// Level 10
                "Recover",   // Level 10
                "Leer",      // Level 10
                "QuickStrike",// Level 15
                "SandAttack",// Level 15
                "Regenerate",// Level 20
                "HydroPump", // Level 30
                "Intimidate",// Level 30
                "ShellBash", // Level 30
                "SecondWind",// Level 35
                "Cyclone",   // Level 40
                "LastStand", // Level 45
            ],
            xpGainWhenDefeated: 20,
            types: ["Water", "Normal"],
            initiative: 14,
            description: "A common river-dwelling Pokete, and a notoriously weak battler - most would rather flee than fight, and it's hard to blame them.",
            idPosharpEvolvesInto: "Ragefin",
            evolveLvl: 30,
            icon:
            """
              <°))))><
            """
        ),

        ["Ragefin"] = new PosharpSpecies(
            id: "Ragefin",
            name: "Ragefin",
            healthPoints: 34,
            attack: 60,
            defense: 30,
            specialAttack: 48,
            specialDefense: 28,
            possibleNaturalMovesIds:
            [
                "Tackle",    // Level 0
                "WaterGun",  // Level 0
                "Bite",      // Level 0
                "BubbleBeam",// Level 10
                "Recover",   // Level 10
                "Leer",      // Level 10
                "QuickStrike",// Level 15
                "SandAttack",// Level 15
                "Regenerate",// Level 20
                "HydroPump", // Level 30
                "Intimidate",// Level 30
                "ShellBash", // Level 30
                "SecondWind",// Level 35
                "Cyclone",   // Level 40
                "LastStand", // Level 45
            ],
            xpGainWhenDefeated: 68,
            types: ["Water", "Normal"],
            initiative: 42,
            description: "Specialists are reluctant to even call this an evolution. Most Splashfin just keep losing, but every so often, one grows sick of it, and comes back overwhelmingly, unrecognizably strong.",
            icon:
            """
               v
              <'))))><
            """
        ),

        ["Duskowl"] = new PosharpSpecies(
            id: "Duskowl",
            name: "Duskowl",
            healthPoints: 24,
            attack: 34,
            defense: 18,
            specialAttack: 28,
            specialDefense: 20,
            possibleNaturalMovesIds:
            [
                "Peck",      // Level 0
                "Tackle",    // Level 0
                "WingAttack",// Level 10
                "Leer",      // Level 10
                "Recover",   // Level 10
                "QuickStrike",// Level 15
                "HeadCharge",// Level 20
                "Screech",   // Level 20
                "Regenerate",// Level 20
                "CrushClaw", // Level 25
                "Intimidate",// Level 30
                "SecondWind",// Level 35
                "Cyclone",   // Level 40
                "LastStand", // Level 45
                "Meteor",    // Level 50
            ],
            xpGainWhenDefeated: 40,
            types: ["Flying", "Normal", "Bird"],
            initiative: 40,
            description: "A nocturnal hunter with near-silent wings. Most people only ever hear it, never see it.",
            icon:
            """
               ,___,
               {o,o}
               /)_)
                ""
            """
        ),

        ["Voltcell"] = new PosharpSpecies(
            id: "Voltcell",
            name: "Voltcell",
            healthPoints: 20,
            attack: 20,
            defense: 16,
            specialAttack: 46,
            specialDefense: 24,
            possibleNaturalMovesIds:
            [
                "Tackle",     // Level 0
                "Bite",       // Level 0
                "ThunderShock",// Level 5
                "RockThrow",  // Level 5
                "Recover",    // Level 10
                "Leer",       // Level 10
                "QuickStrike",// Level 15
                "Thunderbolt",// Level 20
                "Regenerate", // Level 20
                "StoneEdge",  // Level 20
                "Intimidate", // Level 30
                "Overdrive",  // Level 35
                "SecondWind", // Level 35
                "CrackingRoar",// Level 40
                "LastStand",  // Level 45
                "Cataclysm",  // Level 50
            ],
            xpGainWhenDefeated: 38,
            types: ["Electro"],
            initiative: 34,
            description: "A small orb of contained static electricity. Touching it is a mistake made exactly once.",
            icon:
            """
                ( )
                 +
            """
        ),

        ["Wisp"] = new PosharpSpecies(
            id: "Wisp",
            name: "Wisp",
            healthPoints: 22,
            attack: 14,
            defense: 18,
            specialAttack: 42,
            specialDefense: 34,
            possibleNaturalMovesIds:
            [
                "Tackle",    // Level 0
                "Bite",      // Level 0
                "Growl",     // Level 5
                "Leer",      // Level 10
                "Recover",   // Level 10
                "SandAttack",// Level 15
                "FrostBite", // Level 15
                "Screech",   // Level 20
                "Regenerate",// Level 20
                "Intimidate",// Level 30
                "Overdrive", // Level 35
                "SecondWind",// Level 35
                "CrackingRoar",// Level 40
                "LastStand", // Level 45
                "Meteor",    // Level 50
            ],
            xpGainWhenDefeated: 42,
            types: ["Undead"],
            initiative: 24,
            description: "A pale light that drifts through old buildings and forgotten places. It's not sure it means any harm.",
            icon:
            """
                 _
                (_)
                { }
            """
        ),

        ["Cragmaw"] = new PosharpSpecies(
            id: "Cragmaw",
            name: "Cragmaw",
            healthPoints: 38,
            attack: 42,
            defense: 55,
            specialAttack: 15,
            specialDefense: 38,
            possibleNaturalMovesIds:
            [
                "Tackle",    // Level 0
                "RockThrow", // Level 5
                "Growl",     // Level 5
                "Leer",      // Level 10
                "Recover",   // Level 10
                "SandAttack",// Level 15
                "HeadCharge",// Level 20
                "StoneEdge", // Level 20
                "Regenerate",// Level 20
                "EarthSlam", // Level 25
                "CrushClaw", // Level 25
                "Intimidate",// Level 30
                "ShellBash", // Level 30
                "Landslide", // Level 35
                "SecondWind",// Level 35
                "Guillotine",// Level 40
                "CrackingRoar",// Level 40
                "Avalanche", // Level 45
                "LastStand", // Level 45
                "Cataclysm", // Level 50
            ],
            xpGainWhenDefeated: 50,
            types: ["Stone", "Normal"],
            initiative: 8,
            description: "A slow-moving Pokete made of living stone. It has never once lost a staring contest.",
            icon:
            """
            +---------+
            |  o   o  |
            |   ---   |
            +---------+
            """
        ),


        // ==============================================================
        // STARTER Evolution line: Verdant (Plant)
        // ==============================================================

        ["Sproutling"] = new PosharpSpecies(
            id: "Sproutling",
            name: "Sproutling",
            healthPoints: 15,
            attack: 18,
            defense: 16,
            specialAttack: 20,
            specialDefense: 16,
            possibleNaturalMovesIds:
            [
                "Tackle",         // Level 0
                "LeafCutter",     // Level 0
                "Growl",          // Level 5
                "VineWhip",       // Level 5
                "InnerFocus",     // Level 8
                "Leer",           // Level 10
                "Recover",        // Level 10
                "SeedBomb",       // Level 12
                "RootBind",       // Level 13
                "RootSnare",      // Level 14
                "RazorLeaf",      // Level 15
                "SandAttack",     // Level 15
                "ThornLash",      // Level 18
                "Screech",        // Level 20
                "Regenerate",     // Level 20
                "CrushClaw",      // Level 25
                "Intimidate",     // Level 30
                "SolarBurst",     // Level 34
                "SecondWind",     // Level 35
                "LastStand",      // Level 45
                "Cataclysm",      // Level 50
            ],
            xpGainWhenDefeated: 28,
            types: ["Plant"],
            initiative: 18,
            description: "A starter Pokete given to new trainers. A single leaf-bud sits on its head, unfurling a little more every time it wins a battle.",
            idPosharpEvolvesInto: "Bramblewood",
            evolveLvl: 16,
            icon:
            """
               .
              /_\
             ( o )
              \_/
            """
        ),

        ["Bramblewood"] = new PosharpSpecies(
            id: "Bramblewood",
            name: "Bramblewood",
            healthPoints: 24,
            attack: 30,
            defense: 27,
            specialAttack: 32,
            specialDefense: 28,
            possibleNaturalMovesIds:
            [
                "Tackle",         // Level 0
                "LeafCutter",     // Level 0
                "Growl",          // Level 5
                "VineWhip",       // Level 5
                "InnerFocus",     // Level 8
                "Leer",           // Level 10
                "Recover",        // Level 10
                "SeedBomb",       // Level 12
                "RootBind",       // Level 13
                "RootSnare",      // Level 14
                "RazorLeaf",      // Level 15
                "SandAttack",     // Level 15
                "ThornLash",      // Level 18
                "Screech",        // Level 20
                "Regenerate",     // Level 20
                "CrushClaw",      // Level 25
                "Intimidate",     // Level 30
                "SolarBurst",     // Level 34
                "SecondWind",     // Level 35
                "LastStand",      // Level 45
                "Cataclysm",      // Level 50
            ],
            xpGainWhenDefeated: 46,
            types: ["Plant"],
            initiative: 24,
            description: "Branches have sprouted from its shoulders and back, thick enough now to block a direct hit.",
            idPosharpEvolvesInto: "Verdantitan",
            evolveLvl: 32,
            icon:
            """
               \|/
              --*--
             (  o  )
              \___/
            """
        ),

        ["Verdantitan"] = new PosharpSpecies(
            id: "Verdantitan",
            name: "Verdantitan",
            healthPoints: 32,
            attack: 42,
            defense: 38,
            specialAttack: 44,
            specialDefense: 40,
            possibleNaturalMovesIds:
            [
                "Tackle",         // Level 0
                "LeafCutter",     // Level 0
                "Growl",          // Level 5
                "VineWhip",       // Level 5
                "InnerFocus",     // Level 8
                "Leer",           // Level 10
                "Recover",        // Level 10
                "SeedBomb",       // Level 12
                "RootBind",       // Level 13
                "RootSnare",      // Level 14
                "RazorLeaf",      // Level 15
                "SandAttack",     // Level 15
                "ThornLash",      // Level 18
                "Screech",        // Level 20
                "Regenerate",     // Level 20
                "CrushClaw",      // Level 25
                "Intimidate",     // Level 30
                "SolarBurst",     // Level 34
                "SecondWind",     // Level 35
                "LastStand",      // Level 45
                "Cataclysm",      // Level 50
            ],
            xpGainWhenDefeated: 72,
            types: ["Plant"],
            initiative: 26,
            description: "A small forest could grow across its back. Old trainers say a fully grown Verdantitan hasn't moved from the same spot in years, and doesn't need to.",
            icon:
            """
              \\|//
             --\o/--
              /( )\
               d b
            """
        ),


        // ==============================================================
        // STARTER Evolution line: Maelstriker (Water / Fighting)
        // ==============================================================

        ["Tadpaw"] = new PosharpSpecies(
            id: "Tadpaw",
            name: "Tadpaw",
            healthPoints: 16,
            attack: 22,
            defense: 14,
            specialAttack: 16,
            specialDefense: 14,
            possibleNaturalMovesIds:
            [
                "Tackle",         // Level 0
                "RisingKnee",     // Level 0
                "WaterGun",       // Level 0
                "Growl",          // Level 5
                "TailSwipe",      // Level 8
                "BubbleBeam",     // Level 10
                "Leer",           // Level 10
                "Recover",        // Level 10
                "Uppercut",       // Level 10
                "QuickStrike",    // Level 15
                "SandAttack",     // Level 15
                "TakeDown",       // Level 20
                "Regenerate",     // Level 20
                "CrushClaw",      // Level 25
                "Intimidate",     // Level 30
                "HydroPump",      // Level 30
                "Overdrive",      // Level 35
                "SecondWind",     // Level 35
                "Cyclone",        // Level 40
                "LastStand",      // Level 45
                "Cataclysm",      // Level 50
            ],
            xpGainWhenDefeated: 28,
            types: ["Water", "Fighting"],
            initiative: 20,
            description: "A starter Pokete found near riverbanks. Its stubby limbs already throw a surprisingly solid punch for something this small.",
            idPosharpEvolvesInto: "Ripplefist",
            evolveLvl: 16,
            icon:
            """
              (o o)
             ('=')
              d b
            """
        ),

        ["Ripplefist"] = new PosharpSpecies(
            id: "Ripplefist",
            name: "Ripplefist",
            healthPoints: 24,
            attack: 34,
            defense: 22,
            specialAttack: 24,
            specialDefense: 22,
            possibleNaturalMovesIds:
            [
                "Tackle",         // Level 0
                "RisingKnee",     // Level 0
                "WaterGun",       // Level 0
                "Growl",          // Level 5
                "TailSwipe",      // Level 8
                "BubbleBeam",     // Level 10
                "Leer",           // Level 10
                "Recover",        // Level 10
                "Uppercut",       // Level 10
                "QuickStrike",    // Level 15
                "SandAttack",     // Level 15
                "TakeDown",       // Level 20
                "Regenerate",     // Level 20
                "CrushClaw",      // Level 25
                "Intimidate",     // Level 30
                "HydroPump",      // Level 30
                "Overdrive",      // Level 35
                "SecondWind",     // Level 35
                "Cyclone",        // Level 40
                "LastStand",      // Level 45
                "Cataclysm",      // Level 50
            ],
            xpGainWhenDefeated: 46,
            types: ["Water", "Fighting"],
            initiative: 28,
            description: "Its hind legs have grown powerful enough to launch it clean out of the water mid-punch.",
            idPosharpEvolvesInto: "Maelstriker",
            evolveLvl: 32,
            icon:
            """
              (o o)
             <('=')>
              d  b
            """
        ),

        ["Maelstriker"] = new PosharpSpecies(
            id: "Maelstriker",
            name: "Maelstriker",
            healthPoints: 34,
            attack: 50,
            defense: 32,
            specialAttack: 34,
            specialDefense: 32,
            possibleNaturalMovesIds:
            [
                "Tackle",         // Level 0
                "RisingKnee",     // Level 0
                "WaterGun",       // Level 0
                "Growl",          // Level 5
                "TailSwipe",      // Level 8
                "BubbleBeam",     // Level 10
                "Leer",           // Level 10
                "Recover",        // Level 10
                "Uppercut",       // Level 10
                "QuickStrike",    // Level 15
                "SandAttack",     // Level 15
                "TakeDown",       // Level 20
                "Regenerate",     // Level 20
                "CrushClaw",      // Level 25
                "Intimidate",     // Level 30
                "HydroPump",      // Level 30
                "Overdrive",      // Level 35
                "SecondWind",     // Level 35
                "Cyclone",        // Level 40
                "LastStand",      // Level 45
                "Cataclysm",      // Level 50
            ],
            xpGainWhenDefeated: 74,
            types: ["Water", "Fighting"],
            initiative: 36,
            description: "It can punch a hole clean through a boulder underwater without breaking stride. Riverside villages consider one a lucky sign.",
            icon:
            """
              (O O)
             <{'=='}>
              D    B
            """
        ),


        // ==============================================================
        // Evolution line: Wyrmlet
        // ==============================================================

        ["Wyrmlet"] = new PosharpSpecies(
            id: "Wyrmlet",
            name: "Wyrmlet",
            healthPoints: 20,
            attack: 28,
            defense: 20,
            specialAttack: 24,
            specialDefense: 20,
            possibleNaturalMovesIds:
            [
                "Tackle",         // Level 0
                "Bite",           // Level 0
                "Growl",          // Level 5
                "TailSwipe",      // Level 8
                "Leer",           // Level 10
                "Recover",        // Level 10
                "QuickStrike",    // Level 15
                "SandAttack",     // Level 15
                "Screech",        // Level 20
                "Regenerate",     // Level 20
                "DragonClaw",     // Level 25
                "CrushClaw",      // Level 25
                "Intimidate",     // Level 30
                "DraconicRoar",   // Level 30
                "Overdrive",      // Level 35
                "SecondWind",     // Level 35
                "CrackingRoar",   // Level 40
                "LastStand",      // Level 45
                "Cataclysm",      // Level 50
            ],
            xpGainWhenDefeated: 40,
            types: ["Dragon"],
            initiative: 22,
            description: "A small, serpentine Pokete said to be a distant echo of something far larger. It already hoards shiny objects out of pure instinct.",
            idPosharpEvolvesInto: "Drakoros",
            evolveLvl: 26,
            icon:
            """
              /\_/\
             ( o.o )
              > ^ <~~~
            """
        ),

        ["Drakoros"] = new PosharpSpecies(
            id: "Drakoros",
            name: "Drakoros",
            healthPoints: 28,
            attack: 42,
            defense: 32,
            specialAttack: 38,
            specialDefense: 32,
            possibleNaturalMovesIds:
            [
                "Tackle",         // Level 0
                "Bite",           // Level 0
                "Growl",          // Level 5
                "TailSwipe",      // Level 8
                "Leer",           // Level 10
                "Recover",        // Level 10
                "QuickStrike",    // Level 15
                "SandAttack",     // Level 15
                "Screech",        // Level 20
                "Regenerate",     // Level 20
                "DragonClaw",     // Level 25
                "CrushClaw",      // Level 25
                "Intimidate",     // Level 30
                "DraconicRoar",   // Level 30
                "Overdrive",      // Level 35
                "SecondWind",     // Level 35
                "CrackingRoar",   // Level 40
                "LastStand",      // Level 45
                "Cataclysm",      // Level 50
            ],
            xpGainWhenDefeated: 62,
            types: ["Dragon"],
            initiative: 28,
            description: "Its wings are still too small to properly fly, so it glides between rooftops instead, wings catching sparks of static as it goes.",
            icon:
            """
               /\___/\
              ( o   o )
              (   ^   )~~~~
               \  -  /
            """
        ),


        // ==============================================================
        // Standalone species
        // ==============================================================

        ["Mirage"] = new PosharpSpecies(
            id: "Mirage",
            name: "Mirage",
            healthPoints: 18,
            attack: 12,
            defense: 15,
            specialAttack: 48,
            specialDefense: 32,
            possibleNaturalMovesIds:
            [
                "Tackle",         // Level 0
                "MindSpike",      // Level 10
                "Leer",           // Level 10
                "Recover",        // Level 10
                "SandAttack",     // Level 15
                "MindCrush",      // Level 16
                "Obfuscate",      // Level 19
                "Screech",        // Level 20
                "Regenerate",     // Level 20
                "PsychicWave",    // Level 28
                "Intimidate",     // Level 30
                "Overdrive",      // Level 35
                "SecondWind",     // Level 35
                "LastStand",      // Level 45
                "Meteor",         // Level 50
            ],
            xpGainWhenDefeated: 40,
            types: ["Psychic"],
            initiative: 33,
            description: "It's never quite where it looks like it is. Most trainers only ever catch one out of the corner of their eye.",
            icon:
            """
              .-""-.
             /  ..  \\
             \\  ~~  /
              '-..-'
            """
        ),

        ["Chitterwing"] = new PosharpSpecies(
            id: "Chitterwing",
            name: "Chitterwing",
            healthPoints: 16,
            attack: 32,
            defense: 14,
            specialAttack: 26,
            specialDefense: 14,
            possibleNaturalMovesIds:
            [
                "Peck",           // Level 0
                "Tackle",         // Level 0
                "SonicBuzz",      // Level 6
                "Leer",           // Level 10
                "Recover",        // Level 10
                "StingLash",      // Level 12
                "QuickStrike",    // Level 15
                "SandAttack",     // Level 15
                "Screech",        // Level 20
                "Regenerate",     // Level 20
                "CrushClaw",      // Level 25
                "Intimidate",     // Level 30
                "Overdrive",      // Level 35
                "SecondWind",     // Level 35
                "Cyclone",        // Level 40
                "LastStand",      // Level 45
            ],
            xpGainWhenDefeated: 36,
            types: ["Bug", "Sound"],
            initiative: 42,
            description: "It rubs its wings together constantly, producing a buzz that's mostly background noise, right up until it isn't.",
            icon:
            """
               /\  /\
              ((*)（*))
               \\/  \\/
            """
        ),

        ["Grimlatch"] = new PosharpSpecies(
            id: "Grimlatch",
            name: "Grimlatch",
            healthPoints: 26,
            attack: 40,
            defense: 24,
            specialAttack: 38,
            specialDefense: 26,
            possibleNaturalMovesIds:
            [
                "Tackle",         // Level 0
                "Bite",           // Level 0
                "Growl",          // Level 5
                "Leer",           // Level 10
                "Recover",        // Level 10
                "ShadowClaw",     // Level 14
                "QuickStrike",    // Level 15
                "NullPointer",    // Level 20
                "Screech",        // Level 20
                "Regenerate",     // Level 20
                "CrushClaw",      // Level 25
                "Intimidate",     // Level 30
                "Overdrive",      // Level 35
                "SecondWind",     // Level 35
                "CrackingRoar",   // Level 40
                "LastStand",      // Level 45
                "BlueScreen",     // Level 48
            ],
            xpGainWhenDefeated: 44,
            types: ["Dark", "Undead"],
            initiative: 28,
            description: "It was compiled from corrupted save data nobody ever bothered to delete. It doesn't remember what it used to be, and doesn't seem to mind.",
            icon:
            """
              .-'''-.
             /  x  x \\
             |   __   |
              \\______/
            """
        )
    };
}
