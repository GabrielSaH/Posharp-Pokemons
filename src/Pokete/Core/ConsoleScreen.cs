namespace Pokete.Core;

/// <summary>
/// Every screen in the game (menus, the world map, the battle HUD, the dialogue bar)
/// writes through here. Per-frame and per-keypress updates never call
/// <see cref="Console.Clear"/> - overwriting the relevant rows with blank, padded text
/// via <see cref="Console.SetCursorPosition"/> is what keeps those flicker-free. The one
/// exception is <see cref="ClearScreen"/>, used only at real screen transitions (see its
/// own doc comment).
/// </summary>
public static class ConsoleScreen
{
    /// <summary>Fixed column budget every screen renders within.</summary>
    public const int Width = 70;

    /// <summary>Fixed row budget reserved for the whole game (world viewport + status + dialogue bar).</summary>
    public const int Height = 30;

    private static bool _sized;

    /// <summary>
    /// Grows the console buffer/window once, up front, so nothing needs to resize
    /// mid-game. Requests a couple of columns beyond <see cref="Width"/>: writing a
    /// full <see cref="Width"/>-character row starting at column 0 reaches the
    /// console's literal last column, and some terminals auto-wrap the cursor the
    /// moment that happens, which can visibly eat the last character or two of the
    /// line. Leaving a real margin column that's never written to avoids that.
    /// </summary>
    public static void EnsureSize()
    {
        if (_sized) return;
        _sized = true;

        int targetWidth = Width + 2;

        try
        {
            if (Console.BufferWidth < targetWidth || Console.BufferHeight < Height)
                Console.SetBufferSize(Math.Max(Console.BufferWidth, targetWidth), Math.Max(Console.BufferHeight, Height));

            if (Console.WindowWidth < targetWidth || Console.WindowHeight < Height)
                Console.SetWindowSize(Math.Max(Console.WindowWidth, targetWidth), Math.Max(Console.WindowHeight, Height));

            Console.CursorVisible = false;
        }
        catch (Exception)
        {
            // Some terminals (non-Windows, redirected output, etc.) don't support resizing or
            // hiding the cursor - just proceed with whatever the console already gives us.
        }
    }

    public static void WriteRow(int row, string text, ConsoleColor color = ConsoleColor.Gray)
    {
        Console.SetCursorPosition(0, row);
        Console.ForegroundColor = color;
        Console.Write(ClipOrPad(text, Width));
        Console.ResetColor();
    }

    public static void WriteAt(int row, int col, string text)
    {
        Console.SetCursorPosition(col, row);
        Console.Write(text);
    }

    /// <summary>
    /// A real screen change: swap from the world to a battle, from a battle back to
    /// the world, load a new map, open the top-level menu. One <see cref="Console.Clear"/>
    /// here is fine and guarantees no artifact survives the transition - what causes
    /// flicker is calling it on every frame or every keypress, which nothing in this
    /// game does anymore.
    /// </summary>
    public static void ClearScreen()
    {
        Console.Clear();
        Console.CursorVisible = false;
    }

    /// <summary>"Clears" a row range by overwriting it with blank padded rows - for partial, in-place refreshes, not full screen changes.</summary>
    public static void ClearRows(int startRow, int endRowInclusive)
    {
        for (int row = startRow; row <= endRowInclusive; row++) WriteRow(row, "");
    }

    public static string ClipOrPad(string text, int width) =>
        text.Length > width ? text[..width] : text.PadRight(width);

    /// <summary>Greedy word-wrap shared by anything that shows free-form text in a fixed-width space (dialogue, move descriptions).</summary>
    public static List<string> WrapText(string text, int width)
    {
        var lines = new List<string>();
        string current = "";

        foreach (string word in text.Split(' ', StringSplitOptions.RemoveEmptyEntries))
        {
            string candidate = current.Length == 0 ? word : $"{current} {word}";
            if (candidate.Length > width && current.Length > 0)
            {
                lines.Add(current);
                current = word;
            }
            else
            {
                current = candidate;
            }
        }
        if (current.Length > 0) lines.Add(current);

        return lines;
    }
}
