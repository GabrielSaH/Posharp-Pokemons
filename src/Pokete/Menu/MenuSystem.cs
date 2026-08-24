using Pokete.Core;

namespace Pokete.Menu;

/// <summary>
/// Minimal menu navigable by arrows/WASD/Enter, used for the main menu and the
/// overworld deck view. Every render overwrites the full menu area (title, options,
/// and any leftover rows below from a previous, longer menu) through
/// <see cref="ConsoleScreen"/> - never <see cref="Console.Clear"/> - so switching
/// between menus, or from the world map into a menu, never flickers.
/// </summary>
public static class MenuSystem
{
    private const int TitleRow = 0;
    private const int SeparatorRow = 1;
    private const int FirstOptionRow = 2;

    public static int Choose(string title, IReadOnlyList<string> options)
    {
        ConsoleScreen.EnsureSize();
        ConsoleScreen.ClearScreen(); // opening a menu is a real screen change
        int selected = 0;
        Render(title, options, selected);

        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey(intercept: true);
            if (key.Key == ConsoleKey.Enter) return selected;

            int next = key.Key switch
            {
                ConsoleKey.UpArrow or ConsoleKey.W => (selected - 1 + options.Count) % options.Count,
                ConsoleKey.DownArrow or ConsoleKey.S => (selected + 1) % options.Count,
                _ => selected
            };

            if (next == selected) continue;
            selected = next;
            Render(title, options, selected);
        }
    }

    private static void Render(string title, IReadOnlyList<string> options, int selected)
    {
        ConsoleScreen.WriteRow(TitleRow, title);
        ConsoleScreen.WriteRow(SeparatorRow, new string('-', Math.Min(title.Length, ConsoleScreen.Width)));

        for (int i = 0; i < options.Count; i++)
        {
            string line = i == selected ? $" > {options[i]}" : $"   {options[i]}";
            ConsoleScreen.WriteRow(FirstOptionRow + i, line);
        }
    }
}
