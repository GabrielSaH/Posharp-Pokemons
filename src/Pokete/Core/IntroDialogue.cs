namespace Pokete.Core;

/// <summary>Your dad's quick welcome speech, shown once when a brand new player is created.</summary>
public static class IntroDialogue
{
    public static void Show(string playerName)
    {
        DialogueBar.Show(
        [
            $"Happy birthday, {playerName}! Eighteen years old today.",
            "That means you're finally old enough to become a Posharp trainer, same as I was at your age.",
            "Head out down the road, catch and train some Posharp of your own, and beat every gym you find along the way.",
            "The big city at the end of the road has the toughest gym in the region - that's your real goal.",
            "Here, take these before you go: five Poketeballs, and five potions in case you get banged up out there.",
            "Take care of yourself. I'll be here when you make it back."
        ]);
    }
}
