using Pokete.Moves;
using Pokete.Moves.Effects;
using Pokete.Moves.Effects.Implementations;

namespace Pokete.Data.Generated;

public class GeneratedMoves
{
    public static readonly Dictionary<string, Move> All = new()
    {
        // ==============================================================
        // HEAL MOVES
        // ==============================================================

        // Level 4
        ["AutoSave"] = new Move(
            id: "AutoSave",
            name: "Auto Save",
            category: MoveCategory.Status,
            basePower: 0,
            accuracy: 100,
            basePp: 15,
            minimumLevel: 4,
            description: "The Posharp checkpoints its current state before things get messy"
            ) {Effect = new HealSelfEffect(20)},

        // Level 10
        ["GarbageCollector"] = new Move(
            id: "GarbageCollector",
            name: "Garbage Collector",
            category: MoveCategory.Status,
            basePower: 0,
            accuracy: 100,
            basePp: 10,
            minimumLevel: 10,
            description: "The Posharp sweeps away unused damage, freeing up a chunk of its health"
            ) {Effect = new HealSelfEffect(30)},

        ["Recover"] = new Move(
            id: "Recover",
            name: "Recover",
            category: MoveCategory.Status,
            basePower: 0,
            accuracy: 100,
            basePp: 10,
            minimumLevel: 10,
            description: "A short breather patches the user right back up"
            ) {Effect = new HealSelfEffect(25)},

        // Level 20
        ["HotReload"] = new Move(
            id: "HotReload",
            name: "Hot Reload",
            category: MoveCategory.Status,
            basePower: 0,
            accuracy: 100,
            basePp: 8,
            minimumLevel: 20,
            description: "The Posharp reloads its own code without ever powering down"
            ) {Effect = new HealSelfEffect(40)},

        ["Regenerate"] = new Move(
            id: "Regenerate",
            name: "Regenerate",
            category: MoveCategory.Status,
            basePower: 0,
            accuracy: 100,
            basePp: 8,
            minimumLevel: 20,
            description: "Torn tissue knits back together at an unnatural pace"
            ) {Effect = new HealSelfEffect(35)},

        // Level 32
        ["CheckpointRestore"] = new Move(
            id: "CheckpointRestore",
            name: "Checkpoint Restore",
            category: MoveCategory.Status,
            basePower: 0,
            accuracy: 100,
            basePp: 5,
            minimumLevel: 32,
            description: "The Posharp rolls itself back to the last known good state"
            ) {Effect = new HealSelfEffect(50)},

        // Level 35
        ["SecondWind"] = new Move(
            id: "SecondWind",
            name: "Second Wind",
            category: MoveCategory.Status,
            basePower: 0,
            accuracy: 100,
            basePp: 6,
            minimumLevel: 35,
            description: "A sudden burst of stamina from seemingly nowhere"
            ) {Effect = new HealSelfEffect(45)},

        // Level 45
        ["LastStand"] = new Move(
            id: "LastStand",
            name: "Last Stand",
            category: MoveCategory.Status,
            basePower: 0,
            accuracy: 100,
            basePp: 4,
            minimumLevel: 45,
            description: "Everything left is poured into one final act of recovery"
            ) {Effect = new HealSelfEffect(55)},


        // ==============================================================
        // DAMAGE MOVES
        // ==============================================================

        // Level 0
        ["Scratch"] = new Move(
            id: "Scratch",
            name: "Scratch",
            category: MoveCategory.Physical,
            basePower: 40,
            accuracy: 100,
            basePp: 35,
            minimumLevel: 0,
            description: "Hard, pointed, and sharp claws rake the foe to inflict damage"
            ) {Effect = new NormalDamageEffect(100, 40)},

        ["Tackle"] = new Move(
            id: "Tackle",
            name: "Tackle",
            category: MoveCategory.Physical,
            basePower: 35,
            accuracy: 100,
            basePp: 35,
            minimumLevel: 0,
            description: "A full-body slam; simple, reliable, and always in style"
            ) {Effect = new NormalDamageEffect(100, 35)},

        ["Peck"] = new Move(
            id: "Peck",
            name: "Peck",
            category: MoveCategory.Physical,
            basePower: 30,
            accuracy: 100,
            basePp: 35,
            minimumLevel: 0,
            description: "A quick jab with a sharp beak or point"
            ) {Effect = new NormalDamageEffect(100, 30)},

        ["Bite"] = new Move(
            id: "Bite",
            name: "Bite",
            category: MoveCategory.Physical,
            basePower: 40,
            accuracy: 95,
            basePp: 30,
            minimumLevel: 0,
            description: "Sharp teeth clamp down hard on the foe"
            ) {Effect = new NormalDamageEffect(95, 40)},

        ["WaterGun"] = new Move(
            id: "WaterGun",
            name: "Water Gun",
            category: MoveCategory.Special,
            basePower: 35,
            accuracy: 100,
            basePp: 30,
            minimumLevel: 0,
            description: "A blast of water sprays the foe"
            ) {Effect = new SpecialDamageEffect(100, 35)},

        ["Ember"] = new Move(
            id: "Ember",
            name: "Ember",
            category: MoveCategory.Special,
            basePower: 35,
            accuracy: 100,
            basePp: 30,
            minimumLevel: 0,
            description: "A small flame licks at the foe"
            ) {Effect = new SpecialDamageEffect(100, 35)},

        // Level 3
        ["PointerJab"] = new Move(
            id: "PointerJab",
            name: "Pointer Jab",
            category: MoveCategory.Physical,
            basePower: 45,
            accuracy: 100,
            basePp: 30,
            minimumLevel: 3,
            description: "The Posharp jabs straight at a raw memory address, no bounds-checking included"
            ) {Effect = new NormalDamageEffect(100, 45)},

        // Level 5
        ["VineWhip"] = new Move(
            id: "VineWhip",
            name: "Vine Whip",
            category: MoveCategory.Physical,
            basePower: 45,
            accuracy: 100,
            basePp: 25,
            minimumLevel: 5,
            description: "Thin vines lash out and snap against the foe"
            ) {Effect = new NormalDamageEffect(100, 45)},

        ["RockThrow"] = new Move(
            id: "RockThrow",
            name: "Rock Throw",
            category: MoveCategory.Physical,
            basePower: 45,
            accuracy: 90,
            basePp: 25,
            minimumLevel: 5,
            description: "A chunk of rock is hurled at the foe"
            ) {Effect = new NormalDamageEffect(90, 45)},

        ["ThunderShock"] = new Move(
            id: "ThunderShock",
            name: "Thunder Shock",
            category: MoveCategory.Special,
            basePower: 40,
            accuracy: 100,
            basePp: 30,
            minimumLevel: 5,
            description: "A jolt of electricity zaps the foe"
            ) {Effect = new SpecialDamageEffect(100, 40)},

        // Level 6
        ["RefSlash"] = new Move(
            id: "RefSlash",
            name: "Ref Slash",
            category: MoveCategory.Physical,
            basePower: 55,
            accuracy: 95,
            basePp: 25,
            minimumLevel: 6,
            description: "The Posharp passes its blade-arm by reference and cuts the foe twice as sharp"
            ) {Effect = new NormalDamageEffect(95, 55)},

        // Level 8
        ["ByteBite"] = new Move(
            id: "ByteBite",
            name: "Byte Bite",
            category: MoveCategory.Physical,
            basePower: 50,
            accuracy: 100,
            basePp: 30,
            minimumLevel: 8,
            description: "The Posharp chomps down and consumes a chunk of the foe, one byte at a time"
            ) {Effect = new NormalDamageEffect(100, 50)},

        // Level 10
        ["WingAttack"] = new Move(
            id: "WingAttack",
            name: "Wing Attack",
            category: MoveCategory.Physical,
            basePower: 50,
            accuracy: 100,
            basePp: 25,
            minimumLevel: 10,
            description: "Wings, fins, or flaps slap the foe hard"
            ) {Effect = new NormalDamageEffect(100, 50)},

        ["PoisonSting"] = new Move(
            id: "PoisonSting",
            name: "Poison Sting",
            category: MoveCategory.Physical,
            basePower: 45,
            accuracy: 100,
            basePp: 30,
            minimumLevel: 10,
            description: "A sharp point jabs the foe with something unpleasant"
            ) {Effect = new NormalDamageEffect(100, 45)},

        ["BubbleBeam"] = new Move(
            id: "BubbleBeam",
            name: "Bubble Beam",
            category: MoveCategory.Special,
            basePower: 50,
            accuracy: 95,
            basePp: 20,
            minimumLevel: 10,
            description: "A relentless spray of bubbles pelts the foe"
            ) {Effect = new SpecialDamageEffect(95, 50)},

        // Level 12
        ["MemoryLeak"] = new Move(
            id: "MemoryLeak",
            name: "Memory Leak",
            category: MoveCategory.Special,
            basePower: 55,
            accuracy: 100,
            basePp: 25,
            minimumLevel: 12,
            description: "The Posharp slowly drains resources the foe is never getting back"
            ) {Effect = new SpecialDamageEffect(100, 55)},

        // Level 14
        ["StackOverflow"] = new Move(
            id: "StackOverflow",
            name: "Stack Overflow",
            category: MoveCategory.Physical,
            basePower: 75,
            accuracy: 90,
            basePp: 15,
            minimumLevel: 14,
            description: "The Posharp calls itself over and over, crashing into the foe with recursive force"
            ) {Effect = new NormalDamageEffect(90, 75)},

        // Level 15
        ["QuickStrike"] = new Move(
            id: "QuickStrike",
            name: "Quick Strike",
            category: MoveCategory.Physical,
            basePower: 40,
            accuracy: 100,
            basePp: 30,
            minimumLevel: 15,
            description: "A blur of motion, over before the foe can react"
            ) {Effect = new NormalDamageEffect(100, 40)},

        ["RazorLeaf"] = new Move(
            id: "RazorLeaf",
            name: "Razor Leaf",
            category: MoveCategory.Physical,
            basePower: 55,
            accuracy: 95,
            basePp: 20,
            minimumLevel: 15,
            description: "Leaves as sharp as blades slice through the air"
            ) {Effect = new NormalDamageEffect(95, 55)},

        ["FrostBite"] = new Move(
            id: "FrostBite",
            name: "Frost Bite",
            category: MoveCategory.Special,
            basePower: 55,
            accuracy: 90,
            basePp: 20,
            minimumLevel: 15,
            description: "A chill bites deep enough to sting long after contact"
            ) {Effect = new SpecialDamageEffect(90, 55)},

        // Level 16
        ["RaceCondition"] = new Move(
            id: "RaceCondition",
            name: "Race Condition",
            category: MoveCategory.Special,
            basePower: 60,
            accuracy: 90,
            basePp: 20,
            minimumLevel: 16,
            description: "Two threads, one outcome - The Posharp wins the race and the foe pays for it"
            ) {Effect = new SpecialDamageEffect(90, 60)},

        // Level 18
        ["KernelPanic"] = new Move(
            id: "KernelPanic",
            name: "Kernel Panic",
            category: MoveCategory.Special,
            basePower: 65,
            accuracy: 95,
            basePp: 20,
            minimumLevel: 18,
            description: "The Posharp forces the foe's core process to halt entirely"
            ) {Effect = new SpecialDamageEffect(95, 65)},

        // Level 20
        ["HeadCharge"] = new Move(
            id: "HeadCharge",
            name: "Head Charge",
            category: MoveCategory.Physical,
            basePower: 65,
            accuracy: 90,
            basePp: 20,
            minimumLevel: 20,
            description: "A reckless charge, head-first, full force"
            ) {Effect = new NormalDamageEffect(90, 65)},

        ["StoneEdge"] = new Move(
            id: "StoneEdge",
            name: "Stone Edge",
            category: MoveCategory.Physical,
            basePower: 70,
            accuracy: 85,
            basePp: 15,
            minimumLevel: 20,
            description: "Jagged stones erupt around the foe at deadly angles"
            ) {Effect = new NormalDamageEffect(85, 70)},

        ["Thunderbolt"] = new Move(
            id: "Thunderbolt",
            name: "Thunderbolt",
            category: MoveCategory.Special,
            basePower: 70,
            accuracy: 90,
            basePp: 15,
            minimumLevel: 20,
            description: "A heavy bolt of electricity crashes down"
            ) {Effect = new SpecialDamageEffect(90, 70)},

        // Level 22
        ["SegFault"] = new Move(
            id: "SegFault",
            name: "Seg Fault",
            category: MoveCategory.Special,
            basePower: 70,
            accuracy: 100,
            basePp: 15,
            minimumLevel: 22,
            description: "The Posharp jabs at a memory address it was never meant to touch, corrupting the foe from within"
            ) {Effect = new SpecialDamageEffect(100, 70)},

        // Level 24
        ["HardReset"] = new Move(
            id: "HardReset",
            name: "Hard Reset",
            category: MoveCategory.Physical,
            basePower: 85,
            accuracy: 85,
            basePp: 10,
            minimumLevel: 24,
            description: "The Posharp yanks the power cord clean out, resetting the fight with zero mercy"
            ) {Effect = new NormalDamageEffect(85, 85)},

        // Level 25
        ["IceShard"] = new Move(
            id: "IceShard",
            name: "Ice Shard",
            category: MoveCategory.Special,
            basePower: 65,
            accuracy: 100,
            basePp: 15,
            minimumLevel: 25,
            description: "A shard of ice flies at the foe, fast and cold"
            ) {Effect = new SpecialDamageEffect(100, 65)},

        ["EarthSlam"] = new Move(
            id: "EarthSlam",
            name: "Earth Slam",
            category: MoveCategory.Physical,
            basePower: 75,
            accuracy: 85,
            basePp: 15,
            minimumLevel: 25,
            description: "The ground itself rises up to slam into the foe"
            ) {Effect = new NormalDamageEffect(85, 75)},

        // Level 30
        ["BufferOverflow"] = new Move(
            id: "BufferOverflow",
            name: "Buffer Overflow",
            category: MoveCategory.Special,
            basePower: 95,
            accuracy: 85,
            basePp: 10,
            minimumLevel: 30,
            description: "The Posharp floods the foe with more data than they can hold, spilling raw damage everywhere"
            ) {Effect = new SpecialDamageEffect(85, 95)},

        ["HydroPump"] = new Move(
            id: "HydroPump",
            name: "Hydro Pump",
            category: MoveCategory.Special,
            basePower: 95,
            accuracy: 75,
            basePp: 10,
            minimumLevel: 30,
            description: "An overwhelming torrent of water blasts everything in its path"
            ) {Effect = new SpecialDamageEffect(75, 95)},

        ["Flamethrower"] = new Move(
            id: "Flamethrower",
            name: "Flamethrower",
            category: MoveCategory.Special,
            basePower: 85,
            accuracy: 90,
            basePp: 15,
            minimumLevel: 30,
            description: "A steady stream of fire scorches the foe"
            ) {Effect = new SpecialDamageEffect(90, 85)},

        // Level 35
        ["Landslide"] = new Move(
            id: "Landslide",
            name: "Landslide",
            category: MoveCategory.Physical,
            basePower: 90,
            accuracy: 80,
            basePp: 10,
            minimumLevel: 35,
            description: "Tons of rock and debris come crashing down all at once"
            ) {Effect = new NormalDamageEffect(80, 90)},

        // Level 40
        ["Guillotine"] = new Move(
            id: "Guillotine",
            name: "Guillotine",
            category: MoveCategory.Physical,
            basePower: 100,
            accuracy: 75,
            basePp: 5,
            minimumLevel: 40,
            description: "A single, decisive strike aimed to end things quickly"
            ) {Effect = new NormalDamageEffect(75, 100)},

        ["Cyclone"] = new Move(
            id: "Cyclone",
            name: "Cyclone",
            category: MoveCategory.Special,
            basePower: 95,
            accuracy: 80,
            basePp: 8,
            minimumLevel: 40,
            description: "A howling vortex tears at everything nearby"
            ) {Effect = new SpecialDamageEffect(80, 95)},

        // Level 42
        ["Deconstructor"] = new Move(
            id: "Deconstructor",
            name: "Deconstructor",
            category: MoveCategory.Physical,
            basePower: 110,
            accuracy: 80,
            basePp: 5,
            minimumLevel: 42,
            description: "The Posharp tears the foe down piece by piece, the way a deconstructor unpacks an object"
            ) {Effect = new NormalDamageEffect(80, 110)},

        // Level 48
        ["BlueScreen"] = new Move(
            id: "BlueScreen",
            name: "Blue Screen",
            category: MoveCategory.Special,
            basePower: 120,
            accuracy: 75,
            basePp: 5,
            minimumLevel: 48, // ----------------------
            description: "The Posharp crashes the whole system at once; the foe sees nothing but blue"
            ) {Effect = new SpecialDamageEffect(75, 120)},

        // Level 50
        ["Meteor"] = new Move(
            id: "Meteor",
            name: "Meteor",
            category: MoveCategory.Special,
            basePower: 115,
            accuracy: 75,
            basePp: 5,
            minimumLevel: 50,
            description: "A blazing mass falls from high above with devastating impact"
            ) {Effect = new SpecialDamageEffect(75, 115)},

        ["Cataclysm"] = new Move(
            id: "Cataclysm",
            name: "Cataclysm",
            category: MoveCategory.Physical,
            basePower: 120,
            accuracy: 70,
            basePp: 5,
            minimumLevel: 50,
            description: "Raw, overwhelming force with nothing held back"
            ) {Effect = new NormalDamageEffect(70, 120)},


        // ==============================================================
        // STATUS MOVES
        // ==============================================================

        // Level 5
        ["Deprecate"] = new Move(
            id: "Deprecate",
            name: "Deprecate",
            category: MoveCategory.Status,
            basePower: 0,
            accuracy: 100,
            basePp: 20,
            minimumLevel: 5,
            description: "The Posharp marks the foe's power as deprecated; it still works, just weaker"
            ) {Effect = new StatDebuffEffect(100, StatType.Attack, 20)},

        ["Growl"] = new Move(
            id: "Growl",
            name: "Growl",
            category: MoveCategory.Status,
            basePower: 0,
            accuracy: 100,
            basePp: 30,
            minimumLevel: 5,
            description: "A low, threatening growl saps the foe's fighting spirit"
            ) {Effect = new StatDebuffEffect(100, StatType.Attack, 15)},

        // Level 7
        ["Throttle"] = new Move(
            id: "Throttle",
            name: "Throttle",
            category: MoveCategory.Status,
            basePower: 0,
            accuracy: 100,
            basePp: 20,
            minimumLevel: 7,
            description: "The Posharp rate-limits the foe's every move"
            ) {Effect = new StatDebuffEffect(100, StatType.Initiative, 25)},

        // Level 10
        ["Leer"] = new Move(
            id: "Leer",
            name: "Leer",
            category: MoveCategory.Status,
            basePower: 0,
            accuracy: 100,
            basePp: 30,
            minimumLevel: 10,
            description: "A sharp glare cracks the foe's guard"
            ) {Effect = new StatDebuffEffect(100, StatType.Defense, 15)},

        // Level 11
        ["Sandbox"] = new Move(
            id: "Sandbox",
            name: "Sandbox",
            category: MoveCategory.Status,
            basePower: 0,
            accuracy: 95,
            basePp: 15,
            minimumLevel: 11,
            description: "The Posharp traps the foe in a restricted sandbox, stripping its protections"
            ) {Effect = new StatDebuffEffect(95, StatType.Defense, 20)},

        // Level 13
        ["AccessDenied"] = new Move(
            id: "AccessDenied",
            name: "Access Denied",
            category: MoveCategory.Status,
            basePower: 0,
            accuracy: 95,
            basePp: 15,
            minimumLevel: 13,
            description: "The Posharp revokes the foe's permissions to cast anything special"
            ) {Effect = new StatDebuffEffect(95, StatType.SpecialAttack, 20)},

        // Level 15
        ["SandAttack"] = new Move(
            id: "SandAttack",
            name: "Sand Attack",
            category: MoveCategory.Status,
            basePower: 0,
            accuracy: 100,
            basePp: 25,
            minimumLevel: 15,
            description: "A spray of sand kicks up right into the foe's eyes"
            ) {Effect = new StatDebuffEffect(100, StatType.Accuracy, 20)},

        // Level 17
        ["PatchNotes"] = new Move(
            id: "PatchNotes",
            name: "Patch Notes",
            category: MoveCategory.Status,
            basePower: 0,
            accuracy: 100,
            basePp: 20,
            minimumLevel: 17,
            description: "The Posharp reads out the foe's patch notes out loud: nerfed defenses, mostly"
            ) {Effect = new StatDebuffEffect(100, StatType.SpecialDefense, 20)},

        // Level 19
        ["Obfuscate"] = new Move(
            id: "Obfuscate",
            name: "Obfuscate",
            category: MoveCategory.Status,
            basePower: 0,
            accuracy: 90,
            basePp: 15,
            minimumLevel: 19,
            description: "The Posharp scrambles its own code, making it a blur for the foe to target"
            ) {Effect = new StatDebuffEffect(90, StatType.Accuracy, 25)},

        // Level 20
        ["Screech"] = new Move(
            id: "Screech",
            name: "Screech",
            category: MoveCategory.Status,
            basePower: 0,
            accuracy: 85,
            basePp: 15,
            minimumLevel: 20,
            description: "An ear-splitting shriek rattles the foe down to its core"
            ) {Effect = new StatDebuffEffect(85, StatType.SpecialDefense, 20)},

        // Level 27
        ["RateLimit"] = new Move(
            id: "RateLimit",
            name: "Rate Limit",
            category: MoveCategory.Status,
            basePower: 0,
            accuracy: 90,
            basePp: 10,
            minimumLevel: 27,
            description: "The Posharp caps the foe's requests-per-second down to almost nothing"
            ) {Effect = new StatDebuffEffect(90, StatType.Initiative, 30)},

        // Level 30
        ["Intimidate"] = new Move(
            id: "Intimidate",
            name: "Intimidate",
            category: MoveCategory.Status,
            basePower: 0,
            accuracy: 95,
            basePp: 15,
            minimumLevel: 30,
            description: "A menacing presence alone is enough to shake the foe's resolve"
            ) {Effect = new StatDebuffEffect(95, StatType.Attack, 25)},

        // Level 40
        ["CrackingRoar"] = new Move(
            id: "CrackingRoar",
            name: "Cracking Roar",
            category: MoveCategory.Status,
            basePower: 0,
            accuracy: 85,
            basePp: 10,
            minimumLevel: 40,
            description: "A roar so heavy it seems to slow down time itself"
            ) {Effect = new StatDebuffEffect(85, StatType.Initiative, 30)},


        // ==============================================================
        // STATUS & DAMAGE MOVES (HYBRID)
        // ==============================================================

        // Level 9
        ["BruteForce"] = new Move(
            id: "BruteForce",
            name: "Brute Force",
            category: MoveCategory.Physical,
            basePower: 50,
            accuracy: 90,
            basePp: 20,
            minimumLevel: 9,
            description: "The Posharp hammers away at every possible combination until something breaks"
            ) {Effect = new DamageWithDebuffEffect(90, 50, false, StatType.Defense, 15, 40)},

        // Level 15
        ["CompilerWarning"] = new Move(
            id: "CompilerWarning",
            name: "Compiler Warning",
            category: MoveCategory.Physical,
            basePower: 45,
            accuracy: 100,
            basePp: 20,
            minimumLevel: 15,
            description: "A harsh warning rattles the foe's confidence, even though nothing technically failed"
            ) {Effect = new DamageWithDebuffEffect(100, 45, false, StatType.Attack, 10, 50)},

        // Level 19
        ["CacheMiss"] = new Move(
            id: "CacheMiss",
            name: "Cache Miss",
            category: MoveCategory.Physical,
            basePower: 55,
            accuracy: 95,
            basePp: 20,
            minimumLevel: 19,
            description: "The foe reaches for its next move and comes up empty, stumbling for a beat"
            ) {Effect = new DamageWithDebuffEffect(95, 55, false, StatType.Initiative, 15, 35)},

        // Level 21
        ["SqlInjection"] = new Move(
            id: "SqlInjection",
            name: "SQL Injection",
            category: MoveCategory.Special,
            basePower: 65,
            accuracy: 90,
            basePp: 15,
            minimumLevel: 21,
            description: "The Posharp slips a malicious query straight through the foe's defenses"
            ) {Effect = new DamageWithDebuffEffect(90, 65, true, StatType.SpecialDefense, 15, 40)},

        // Level 23
        ["PhishingAttempt"] = new Move(
            id: "PhishingAttempt",
            name: "Phishing Attempt",
            category: MoveCategory.Special,
            basePower: 50,
            accuracy: 95,
            basePp: 20,
            minimumLevel: 23,
            description: "The Posharp baits the foe into clicking somewhere it really shouldn't have"
            ) {Effect = new DamageWithDebuffEffect(95, 50, true, StatType.Accuracy, 20, 40)},

        // Level 25
        ["ManInTheMiddle"] = new Move(
            id: "ManInTheMiddle",
            name: "Man In The Middle",
            category: MoveCategory.Special,
            basePower: 60,
            accuracy: 95,
            basePp: 15,
            minimumLevel: 25,
            description: "The Posharp intercepts the foe's attack mid-cast and quietly weakens it"
            ) {Effect = new DamageWithDebuffEffect(95, 60, true, StatType.SpecialAttack, 15, 40)},

        ["CrushClaw"] = new Move(
            id: "CrushClaw",
            name: "Crush Claw",
            category: MoveCategory.Physical,
            basePower: 55,
            accuracy: 90,
            basePp: 20,
            minimumLevel: 25,
            description: "A crushing claw strike that can crack the foe's guard wide open"
            ) {Effect = new DamageWithDebuffEffect(90, 55, false, StatType.Defense, 15, 40)},

        // Level 29
        ["MergeConflict"] = new Move(
            id: "MergeConflict",
            name: "Merge Conflict",
            category: MoveCategory.Physical,
            basePower: 70,
            accuracy: 85,
            basePp: 15,
            minimumLevel: 29,
            description: "Two versions collide and only one of them survives - painfully"
            ) {Effect = new DamageWithDebuffEffect(85, 70, false, StatType.Attack, 15, 45)},

        // Level 30
        ["ShellBash"] = new Move(
            id: "ShellBash",
            name: "Shell Bash",
            category: MoveCategory.Physical,
            basePower: 60,
            accuracy: 90,
            basePp: 15,
            minimumLevel: 30,
            description: "A hard-shelled bash that rattles the foe's focus along with its ribs"
            ) {Effect = new DamageWithDebuffEffect(90, 60, false, StatType.SpecialAttack, 15, 35)},

        // Level 33
        ["FirewallBreach"] = new Move(
            id: "FirewallBreach",
            name: "Firewall Breach",
            category: MoveCategory.Physical,
            basePower: 80,
            accuracy: 85,
            basePp: 10,
            minimumLevel: 33,
            description: "The Posharp punches clean through every layer of protection"
            ) {Effect = new DamageWithDebuffEffect(85, 80, false, StatType.Defense, 20, 50)},

        // Level 35
        ["Overdrive"] = new Move(
            id: "Overdrive",
            name: "Overdrive",
            category: MoveCategory.Special,
            basePower: 75,
            accuracy: 85,
            basePp: 10,
            minimumLevel: 35,
            description: "Every ounce of energy is redirected into a single overwhelming blast"
            ) {Effect = new DamageWithDebuffEffect(85, 75, true, StatType.SpecialDefense, 20, 45)},

        // Level 36
        ["Ddos"] = new Move(
            id: "Ddos",
            name: "DDoS",
            category: MoveCategory.Special,
            basePower: 85,
            accuracy: 80,
            basePp: 10,
            minimumLevel: 36,
            description: "The Posharp floods the foe with far more requests than it could ever handle"
            ) {Effect = new DamageWithDebuffEffect(80, 85, true, StatType.SpecialDefense, 20, 50)},

        // Level 38
        ["ForcePush"] = new Move(
            id: "ForcePush",
            name: "Force Push",
            category: MoveCategory.Physical,
            basePower: 90,
            accuracy: 80,
            basePp: 10,
            minimumLevel: 38,
            description: "The Posharp overwrites the foe's history whether it likes it or not"
            ) {Effect = new DamageWithDebuffEffect(80, 90, false, StatType.Defense, 20, 40)},

        // Level 45
        ["Avalanche"] = new Move(
            id: "Avalanche",
            name: "Avalanche",
            category: MoveCategory.Physical,
            basePower: 95,
            accuracy: 80,
            basePp: 8,
            minimumLevel: 45,
            description: "An unstoppable wall of force buries the foe's defenses along with the foe"
            ) {Effect = new DamageWithDebuffEffect(80, 95, false, StatType.Defense, 20, 45)},


        // ==============================================================
        // NEW HEAL MOVES
        // ==============================================================

        // Level 8
        ["InnerFocus"] = new Move(
            id: "InnerFocus",
            name: "Inner Focus",
            category: MoveCategory.Status,
            basePower: 0,
            accuracy: 100,
            basePp: 15,
            minimumLevel: 8,
            description: "A slow breath in, a slow breath out, and the pain fades enough to keep fighting"
            ) {Effect = new HealSelfEffect(25)},

        // Level 12
        ["Photosynthesis"] = new Move(
            id: "Photosynthesis",
            name: "Photosynthesis",
            category: MoveCategory.Status,
            basePower: 0,
            accuracy: 100,
            basePp: 15,
            minimumLevel: 12,
            description: "The Posharp turns a moment of sunlight straight into repaired cells"
            ) {Effect = new HealSelfEffect(30)},

        // Level 18
        ["Molt"] = new Move(
            id: "Molt",
            name: "Molt",
            category: MoveCategory.Status,
            basePower: 0,
            accuracy: 100,
            basePp: 10,
            minimumLevel: 18,
            description: "An old, damaged shell splits and falls away, leaving something tougher underneath"
            ) {Effect = new HealSelfEffect(35)},


        // ==============================================================
        // NEW DAMAGE MOVES
        // ==============================================================

        // Level 0
        ["Ram"] = new Move(
            id: "Ram",
            name: "Ram",
            category: MoveCategory.Physical,
            basePower: 35,
            accuracy: 100,
            basePp: 35,
            minimumLevel: 0,
            description: "A blunt, full-speed charge favored by anything with a hard head"
            ) {Effect = new NormalDamageEffect(100, 35)},

        ["LeafCutter"] = new Move(
            id: "LeafCutter",
            name: "Leaf Cutter",
            category: MoveCategory.Physical,
            basePower: 40,
            accuracy: 100,
            basePp: 30,
            minimumLevel: 0,
            description: "A single leaf, honed to a surprisingly clean edge"
            ) {Effect = new NormalDamageEffect(100, 40)},

        ["RisingKnee"] = new Move(
            id: "RisingKnee",
            name: "Rising Knee",
            category: MoveCategory.Physical,
            basePower: 45,
            accuracy: 100,
            basePp: 25,
            minimumLevel: 0,
            description: "A sharp knee strike aimed straight up into the foe's guard"
            ) {Effect = new NormalDamageEffect(100, 45)},

        // Level 6
        ["SonicBuzz"] = new Move(
            id: "SonicBuzz",
            name: "Sonic Buzz",
            category: MoveCategory.Special,
            basePower: 45,
            accuracy: 100,
            basePp: 25,
            minimumLevel: 6,
            description: "A disorienting high-pitched buzz batters the foe's senses"
            ) {Effect = new SpecialDamageEffect(100, 45)},

        // Level 10
        ["Uppercut"] = new Move(
            id: "Uppercut",
            name: "Uppercut",
            category: MoveCategory.Physical,
            basePower: 55,
            accuracy: 100,
            basePp: 20,
            minimumLevel: 10,
            description: "A rising punch that connects clean under the chin"
            ) {Effect = new NormalDamageEffect(100, 55)},

        ["MindSpike"] = new Move(
            id: "MindSpike",
            name: "Mind Spike",
            category: MoveCategory.Special,
            basePower: 50,
            accuracy: 100,
            basePp: 25,
            minimumLevel: 10,
            description: "A focused thought jabs directly into the foe's mind"
            ) {Effect = new SpecialDamageEffect(100, 50)},

        // Level 12
        ["SeedBomb"] = new Move(
            id: "SeedBomb",
            name: "Seed Bomb",
            category: MoveCategory.Physical,
            basePower: 55,
            accuracy: 100,
            basePp: 20,
            minimumLevel: 12,
            description: "A hardened seed pod is lobbed at the foe and detonates on impact"
            ) {Effect = new NormalDamageEffect(100, 55)},

        ["StingLash"] = new Move(
            id: "StingLash",
            name: "Sting Lash",
            category: MoveCategory.Physical,
            basePower: 50,
            accuracy: 95,
            basePp: 20,
            minimumLevel: 12,
            description: "A barbed limb lashes out, fast and precise"
            ) {Effect = new NormalDamageEffect(95, 50)},

        // Level 14
        ["ShadowClaw"] = new Move(
            id: "ShadowClaw",
            name: "Shadow Claw",
            category: MoveCategory.Physical,
            basePower: 55,
            accuracy: 95,
            basePp: 20,
            minimumLevel: 14,
            description: "Claws wreathed in creeping darkness tear at the foe"
            ) {Effect = new NormalDamageEffect(95, 55)},

        // Level 18
        ["ThornLash"] = new Move(
            id: "ThornLash",
            name: "Thorn Lash",
            category: MoveCategory.Physical,
            basePower: 60,
            accuracy: 95,
            basePp: 15,
            minimumLevel: 18,
            description: "A whip of woody thorns cracks across the foe"
            ) {Effect = new NormalDamageEffect(95, 60)},

        // Level 20
        ["NullPointer"] = new Move(
            id: "NullPointer",
            name: "Null Pointer",
            category: MoveCategory.Special,
            basePower: 65,
            accuracy: 95,
            basePp: 15,
            minimumLevel: 20,
            description: "The Posharp reaches for something that was never there, and the foe pays the price anyway"
            ) {Effect = new SpecialDamageEffect(95, 65)},

        // Level 25
        ["DragonClaw"] = new Move(
            id: "DragonClaw",
            name: "Dragon Claw",
            category: MoveCategory.Physical,
            basePower: 70,
            accuracy: 100,
            basePp: 15,
            minimumLevel: 25,
            description: "Claws imbued with ancient, overwhelming force rake across the foe"
            ) {Effect = new NormalDamageEffect(100, 70)},

        // Level 28
        ["PsychicWave"] = new Move(
            id: "PsychicWave",
            name: "Psychic Wave",
            category: MoveCategory.Special,
            basePower: 75,
            accuracy: 90,
            basePp: 15,
            minimumLevel: 28,
            description: "A ripple of pure thought crashes over the battlefield"
            ) {Effect = new SpecialDamageEffect(90, 75)},

        // Level 30
        ["DraconicRoar"] = new Move(
            id: "DraconicRoar",
            name: "Draconic Roar",
            category: MoveCategory.Special,
            basePower: 80,
            accuracy: 90,
            basePp: 10,
            minimumLevel: 30,
            description: "An ancient roar that carries far more force than sound alone"
            ) {Effect = new SpecialDamageEffect(90, 80)},

        // Level 34
        ["SolarBurst"] = new Move(
            id: "SolarBurst",
            name: "Solar Burst",
            category: MoveCategory.Special,
            basePower: 90,
            accuracy: 85,
            basePp: 10,
            minimumLevel: 34,
            description: "Concentrated sunlight is focused into a single searing beam"
            ) {Effect = new SpecialDamageEffect(85, 90)},

        // Level 36
        ["FocusBlow"] = new Move(
            id: "FocusBlow",
            name: "Focus Blow",
            category: MoveCategory.Physical,
            basePower: 95,
            accuracy: 80,
            basePp: 10,
            minimumLevel: 36,
            description: "Every muscle fires at once for one bone-rattling hit"
            ) {Effect = new NormalDamageEffect(80, 95)},


        // ==============================================================
        // NEW STATUS MOVES
        // ==============================================================

        // Level 8
        ["TailSwipe"] = new Move(
            id: "TailSwipe",
            name: "Tail Swipe",
            category: MoveCategory.Status,
            basePower: 0,
            accuracy: 100,
            basePp: 30,
            minimumLevel: 8,
            description: "A heavy tail sweeps low and knocks the foe's footing loose"
            ) {Effect = new StatDebuffEffect(100, StatType.Defense, 15)},

        // Level 9
        ["LowBattery"] = new Move(
            id: "LowBattery",
            name: "Low Battery",
            category: MoveCategory.Status,
            basePower: 0,
            accuracy: 95,
            basePp: 20,
            minimumLevel: 9,
            description: "The foe's internal charge starts running dangerously low"
            ) {Effect = new StatDebuffEffect(95, StatType.SpecialAttack, 20)},

        // Level 13
        ["RootBind"] = new Move(
            id: "RootBind",
            name: "Root Bind",
            category: MoveCategory.Status,
            basePower: 0,
            accuracy: 90,
            basePp: 15,
            minimumLevel: 13,
            description: "Creeping roots wrap around the foe's legs and refuse to let go"
            ) {Effect = new StatDebuffEffect(90, StatType.Initiative, 25)},

        // Level 23
        ["StaticCling"] = new Move(
            id: "StaticCling",
            name: "Static Cling",
            category: MoveCategory.Status,
            basePower: 0,
            accuracy: 90,
            basePp: 15,
            minimumLevel: 23,
            description: "Charged fur and fluff cling stubbornly to the foe's sensors"
            ) {Effect = new StatDebuffEffect(90, StatType.Accuracy, 20)},


        // ==============================================================
        // NEW STATUS & DAMAGE MOVES (HYBRID)
        // ==============================================================

        // Level 14
        ["RootSnare"] = new Move(
            id: "RootSnare",
            name: "Root Snare",
            category: MoveCategory.Physical,
            basePower: 50,
            accuracy: 95,
            basePp: 20,
            minimumLevel: 14,
            description: "Roots burst from the ground mid-strike and coil tight around the foe"
            ) {Effect = new DamageWithDebuffEffect(95, 50, false, StatType.Initiative, 20, 50)},

        // Level 16
        ["MindCrush"] = new Move(
            id: "MindCrush",
            name: "Mind Crush",
            category: MoveCategory.Special,
            basePower: 55,
            accuracy: 95,
            basePp: 15,
            minimumLevel: 16,
            description: "A crushing wave of pure thought batters both body and resolve"
            ) {Effect = new DamageWithDebuffEffect(95, 55, true, StatType.SpecialDefense, 15, 45)},

        // Level 20
        ["TakeDown"] = new Move(
            id: "TakeDown",
            name: "Take Down",
            category: MoveCategory.Physical,
            basePower: 65,
            accuracy: 90,
            basePp: 15,
            minimumLevel: 20,
            description: "A reckless full-body tackle that leaves the foe's guard wide open"
            ) {Effect = new DamageWithDebuffEffect(90, 65, false, StatType.Defense, 15, 45)},

        // Level 27
        ["HeavyBlow"] = new Move(
            id: "HeavyBlow",
            name: "Heavy Blow",
            category: MoveCategory.Physical,
            basePower: 75,
            accuracy: 85,
            basePp: 10,
            minimumLevel: 27,
            description: "A crushing strike aimed to break the foe's fighting spirit along with its ribs"
            ) {Effect = new DamageWithDebuffEffect(85, 75, false, StatType.Attack, 15, 40)}
    };
}