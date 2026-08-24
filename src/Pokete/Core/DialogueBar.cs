namespace Pokete.Core;

/// <summary>
/// A little dialogue bar near the bottom of the screen - not a full screen change.
/// Any NPC or trainer can show a line (or a few, advanced one at a time with Enter)
/// with a single call: <c>DialogueBar.Show(lines)</c>. Each logical line is word-wrapped
/// to fit, so a dialogue author never has to hand-count characters to avoid getting
/// the end of a sentence clipped off.
/// </summary>
public static class DialogueBar
{
    private const int FirstRow = 23;
    private const int MaxTextRows = 3;
    private const int HintRow = FirstRow + MaxTextRows;
    private const int TextWidth = ConsoleScreen.Width - 4; // leaves room for the "» " / "  " prefix

    public static void Show(string line) => Show([line]);

    public static void Show(IReadOnlyList<string> lines)
    {
        if (lines.Count == 0) return;

        foreach (string line in lines)
        {
            ConsoleScreen.ClearRows(FirstRow, HintRow);

            List<string> wrapped = ConsoleScreen.WrapText(line, TextWidth);
            for (int i = 0; i < wrapped.Count && i < MaxTextRows; i++)
            {
                string prefix = i == 0 ? "\u00bb " : "  ";
                ConsoleScreen.WriteRow(FirstRow + i, prefix + wrapped[i]);
            }

            ConsoleScreen.WriteRow(HintRow, "  (Enter to continue)");
            while (Console.ReadKey(intercept: true).Key != ConsoleKey.Enter) { }
        }

        ConsoleScreen.ClearRows(FirstRow, HintRow);
    }
}
