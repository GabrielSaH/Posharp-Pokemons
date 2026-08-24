using Pokete.Models;

namespace Pokete.Data.Generated;

/// <summary>
/// Every NPC trainer in the game. Adding a new one is just adding an entry here -
/// same pattern as <see cref="GeneratedMoves"/> and <see cref="GeneratedPosharpEspecies"/>:
/// pick an id, a map/spot to stand on, a team (species id + level), and some
/// dialogue. <see cref="NpcTrainer.FromDefinition"/> does the rest.
/// </summary>
public static class GeneratedTrainers
{
    public static readonly Dictionary<string, TrainerDefinition> All = new()
    {
        ["youngster_bruno"] = new TrainerDefinition(
            id: "youngster_bruno",
            name: "Youngster Bruno",
            mapId: "playmap_1",
            x: 17, y: 2,
            symbol: 'a',
            money: 20,
            team: [("Pisharp", 4), ("Splashfin", 4)],
            preFightDialogue:
            [
                "Hey! You look like a rookie trainer.",
                "I've been training right here in Nice Town - let's see what you've got!"
            ],
            postFightDialogue: ["Aw man, I lost... You're stronger than you look!"]
        ),

        ["camper_elin"] = new TrainerDefinition(
            id: "camper_elin",
            name: "Camper Elin",
            mapId: "playmap_51",
            x: 6, y: 2,
            symbol: 'a',
            money: 25,
            team: [("Sproutling", 5), ("Tadpaw", 5)],
            preFightDialogue:
            [
                "Whoa, watch where you're walking!",
                "Since you're here, want to battle? My Posharp could use the practice."
            ],
            postFightDialogue: ["Good battle! I've got some studying to do."]
        ),

        ["hiker_otto"] = new TrainerDefinition(
            id: "hiker_otto",
            name: "Hiker Otto",
            mapId: "playmap_2",
            x: 4, y: 2,
            symbol: 'a',
            money: 40,
            team: [("Cragmaw", 8), ("EmberFang", 9), ("Voltcell", 9)],
            preFightDialogue:
            [
                "This route separates the trainers from the tourists.",
                "Let's find out which one you are!"
            ],
            postFightDialogue: ["Solid team. Mind the route ahead - it only gets tougher."]
        ),

        ["ace_trainer_vesna"] = new TrainerDefinition(
            id: "ace_trainer_vesna",
            name: "Ace Trainer Vesna",
            mapId: "playmap_28",
            x: 4, y: 2,
            symbol: 'a',
            money: 300,
            team: [("Permafrost", 45), ("Grimlatch", 46), ("Wyrmlet", 47), ("Mirage", 48)],
            preFightDialogue:
            [
                "An Ace Trainer never turns down a challenger.",
                "Show me you belong on this route."
            ],
            postFightDialogue: ["...Impressive. Truly impressive."]
        ),

        ["rival_devon"] = new TrainerDefinition(
            id: "rival_devon",
            name: "Rival Devon",
            mapId: "playmap_1",
            x: 29, y: 6,
            symbol: 'a',
            money: 0,
            team: [("Permafrost", 5)],
            preFightDialogue:
            [
                "Hey! Dad gave me my first Posharp today too - a Permafrost!",
                "So we're both trainers now, huh? Let's see how strong you are - battle me!"
            ],
            postFightDialogue: ["Ha, you got me! I'll catch up, just you watch."]
        ),

        ["cave_guardian_reyes"] = new TrainerDefinition(
            id: "cave_guardian_reyes",
            name: "Cave Guardian Reyes",
            mapId: "playmap_51",
            x: 21, y: 1,
            symbol: 'a',
            money: 35,
            team: [("Wisp", 6), ("Cragmaw", 7), ("Vipertongue", 10)],
            preFightDialogue:
            [
                "Hold it right there!",
                "Nobody sets foot in Nice Town Cave without proving they can handle what's inside."
            ],
            postFightDialogue: ["...Alright, you've earned your way through. Watch your step down there."]
        ),
    };
}
