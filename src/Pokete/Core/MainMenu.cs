using System.Threading;

namespace Pokete.Core;

/// <summary>
/// The title-screen menu: a bigger Pisharp above a centered, bordered option box,
/// used only for the very first choice the player makes (Continue/New game/Quit).
/// Everywhere else that needs a simple list picker still uses the plain
/// <see cref="Pokete.Menu.MenuSystem"/> - this one exists specifically for the extra
/// presentation (the mascot, the framed box) a title screen deserves and an in-game
/// menu doesn't.
/// <para>
/// The mascot idles between three fixed-size frames (same width/height, so swapping
/// frames never shifts anything else on screen) on a simple time-based cycle: it
/// blinks briefly and waves its arms every so often. Nothing here uses a background
/// thread - the wait loop just polls <see cref="Console.KeyAvailable"/> and advances
/// the animation between checks, the same single-threaded style as the rest of the
/// game.
/// </para>
/// </summary>
public static class MainMenu
{
    private const int MascotTopRow = 1;
    private const int MascotWidth = 14;
    private const int BoxTopRow = MascotTopRow + 5;

    private const double CycleSeconds = 8.0;
    private const double BlinkAt = 0.0;
    private const double BlinkDuration = 0.25;
    private const double WaveAt = 3.5;
    private const double WaveDuration = 0.6;

    private const int PollDelayMs = 60;

    
    public static int Choose(string title, IReadOnlyList<string> options)
    {
        ConsoleScreen.EnsureSize();
        ConsoleScreen.ClearScreen();

        int selected = 0;
        var clock = System.Diagnostics.Stopwatch.StartNew();
        DrawBox(title, options, selected);

        while (true)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(intercept: true);
                // Same held-key fix as InputHandler: only ever act on the most recent buffered key.
                while (Console.KeyAvailable) key = Console.ReadKey(intercept: true);

                if (key.Key == ConsoleKey.Enter) return selected;

                int next = key.Key switch
                {
                    ConsoleKey.UpArrow or ConsoleKey.W => (selected - 1 + options.Count) % options.Count,
                    ConsoleKey.DownArrow or ConsoleKey.S => (selected + 1) % options.Count,
                    _ => selected
                };

                if (next != selected)
                {
                    selected = next;
                    DrawBox(title, options, selected);
                }

                continue;
            }
            
            Thread.Sleep(PollDelayMs);
        }
    }
    
    
    private static void DrawBox(string title, IReadOnlyList<string> options, int selected)
    {
        int innerWidth = Math.Max(title.Length, options.Max(o => o.Length + 2)) + 2;
        int startCol = Math.Max(0, (ConsoleScreen.Width - (innerWidth + 2)) / 2);

        int row = BoxTopRow;
        ConsoleScreen.WriteAt(row++, startCol, "+" + new string('-', innerWidth) + "+");
        ConsoleScreen.WriteAt(row++, startCol, "|" + Center(title, innerWidth) + "|");
        ConsoleScreen.WriteAt(row++, startCol, "+" + new string('-', innerWidth) + "+");

        for (int i = 0; i < options.Count; i++)
        {
            string line = i == selected ? $"> {options[i]}" : $"  {options[i]}";
            ConsoleScreen.WriteAt(row++, startCol, "|" + line.PadRight(innerWidth) + "|");
        }

        ConsoleScreen.WriteAt(row, startCol, "+" + new string('-', innerWidth) + "+");
    }

    private static string Center(string text, int width)
    {
        int totalPad = Math.Max(0, width - text.Length);
        int left = totalPad / 2;
        int right = totalPad - left;
        return new string(' ', left) + text + new string(' ', right);
    }
}
