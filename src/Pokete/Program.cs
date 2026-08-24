using Pokete.Core;
using Pokete.Data;
using Pokete.Menu;
using Pokete.Models;

namespace Pokete;

public static class Program
{
    public static void Main(string[] args)
    {
        try { Console.OutputEncoding = System.Text.Encoding.UTF8; } catch { /* not supported when output is redirected */ }

        if (args.Contains("--help") || args.Contains("-h"))
        {
            Console.WriteLine("Pokete -- Grey Edition (C# port)");
            Console.WriteLine();
            Console.WriteLine("Usage: pokete [options]");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("  --no_audio      Suppress all audio (placeholder, no audio engine yet)");
            Console.WriteLine("  --help, -h      Show this help message");
            Console.WriteLine();
            Console.WriteLine("Controls: W A S D to move, 1 to open deck, E for settings, Esc to quit.");
            return;
        }

        ConsoleScreen.EnsureSize();

        var options = new List<string> { "New game" };
        if (SaveManager.SaveExists()) options.Insert(0, "Continue");
        options.Add("Quit");

        string selected = options[MenuSystem.Choose("Pokete -- Grey Edition (C# port)", options)];
        Player? player = selected switch
        {
            "Continue" => SaveManager.LoadPlayer() ?? PlayerFactory.CreateNew(),
            "New game" => PlayerFactory.CreateNew(),
            _ => null
        };
        if (player is null) return;

        var map = SampleMapBuilder.BuildForPlayer(player);
        new GameEngine(player, map).Run();

        SaveManager.SavePlayer(player);
        ConsoleScreen.ClearScreen();
        ConsoleScreen.WriteRow(0, "Game saved. Thanks for playing!");
    }
}
