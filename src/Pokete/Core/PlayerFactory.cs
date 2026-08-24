using Pokete.Data;
using Pokete.Data.Generated;
using Pokete.Models;

namespace Pokete.Core;

/// <summary>Builds a brand new <see cref="Player"/>: asks for a name, hands them the starter Posharp, and places them at the starting map's spawn point.</summary>
public static class PlayerFactory
{
    private const string StartMapId = "playmap_1";
    private const string StarterSpeciesId = "Pisharp";
    private const int StarterLevel = 5;

    public static Player CreateNew()
    {
        ConsoleScreen.ClearScreen();
        ConsoleScreen.WriteRow(0, "Enter your trainer name:");
        Console.SetCursorPosition(0, 1);
        string name = Console.ReadLine() ?? "Trainer";

        var player = new Player { Name = string.IsNullOrWhiteSpace(name) ? "Trainer" : name };

        var starter = new PosharpInstance(
            GeneratedPosharpEspecies.All[StarterSpeciesId],
            level: StarterLevel,
            xp: PosharpInstance.XpForLevel(StarterLevel));

        player.Deck.Add(starter);
        player.Storage.Add(starter);
        player.CaughtSpecies.Add(starter.Species.Id);
        player.Inventory.Add("poketeball", 5);
        player.Inventory.Add("healing_potion", 5);

        player.CurrentMapId = StartMapId;
        (player.X, player.Y) = SampleMapBuilder.FindSpawnPoint(StartMapId);

        IntroDialogue.Show(player.Name);

        return player;
    }
}
