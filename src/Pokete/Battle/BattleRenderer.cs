using Pokete.Core;
using Pokete.Models;
using Pokete.Moves;

namespace Pokete.Battle;

/// <summary>
/// Describes a transient visual effect applied to one combatant's icon for a single
/// frame: a horizontal shift (physical lunge), a foreground color override (special
/// flash) and/or a decoration character sprinkled around the icon (status sparkle).
/// </summary>
public readonly record struct IconAnimation(int ColumnOffset = 0, ConsoleColor? Color = null, char? Decoration = null)
{
    public static readonly IconAnimation None = new();
}

/// <summary>
/// Draws the battle HUD to the console. Every call repaints the entire fixed-size frame
/// in place through <see cref="ConsoleScreen"/> - never <see cref="Console.Clear"/> -
/// which is what keeps the redraw flicker-free. This class only knows how to render a
/// given state; it has no awareness of turns, input or battle rules.
/// </summary>
public static class BattleRenderer
{
    public const int OuterWidth = ConsoleScreen.Width;
    private const int InnerWidth = OuterWidth - 2;

    private const int BoxOuterWidth = 22;
    private const int BoxInnerWidth = BoxOuterWidth - 2;
    private const int BoxHeight = 5;
    private const int IconRegionWidth = InnerWidth - BoxOuterWidth;
    private const int HealthBarSegments = 10;
    private const int GapRows = 3;

    private const int TopBorderRow = 0;
    private const int EnemyBoxTopRow = TopBorderRow + 1;
    private const int GapTopRow = EnemyBoxTopRow + BoxHeight;
    private const int PlayerBoxTopRow = GapTopRow + GapRows;
    private const int SeparatorRow = PlayerBoxTopRow + BoxHeight;
    private const int MessageRow = SeparatorRow + 1;
    private const int PromptRow = MessageRow + 1;
    private const int BlankRow = PromptRow + 1;
    private const int MenuRow = BlankRow + 1;
    private const int BottomBorderRow = MenuRow + 1;
    public const int TotalRows = BottomBorderRow + 1;

    private const int PopupTopRow = 4;
    private const int PopupInnerWidth = 34;

    private static readonly string[] MenuLabels = ["Attack", "Run!", "Inv.", "Deck"];

    public static void RenderHud(
        PosharpInstance player,
        PosharpInstance enemy,
        string message,
        bool showMenu = false,
        int menuCursorIndex = 0,
        IconAnimation playerAnimation = default,
        IconAnimation enemyAnimation = default)
    {
        WriteBorderRow(TopBorderRow);
        DrawCombatantSection(EnemyBoxTopRow, enemy, boxOnLeft: true, enemyAnimation);
        for (int i = 0; i < GapRows; i++) WriteRow(GapTopRow + i, "");
        DrawCombatantSection(PlayerBoxTopRow, player, boxOnLeft: false, playerAnimation);
        WriteRow(SeparatorRow, new string('-', InnerWidth));

        WriteRow(MessageRow, message);
        WriteRow(PromptRow, showMenu ? "What do you want to do?" : "");
        WriteRow(BlankRow, "");
        WriteRow(MenuRow, showMenu ? BuildMenuLine(menuCursorIndex) : "");

        WriteBorderRow(BottomBorderRow);
        Console.SetCursorPosition(0, TotalRows);
    }

    public static void RenderMovePopup(PosharpInstance player, PosharpInstance enemy, IReadOnlyList<MoveInstance> moves, int cursorIndex, bool showDescription = false)
    {
        RenderHud(player, enemy, message: "");
        DrawMovePopup(moves, cursorIndex, showDescription);
        Console.SetCursorPosition(0, TotalRows);
    }

    /// <summary>
    /// A titled, bordered list popup in the same spot and style as the move popup -
    /// used for the Inventory menu, switching the active Posharp, and choosing which
    /// move to forget on level-up, so every in-battle choice looks and feels the same.
    /// </summary>
    public static void RenderSelectionPopup(
        PosharpInstance player,
        PosharpInstance enemy,
        string title,
        IReadOnlyList<string> optionLines,
        int cursorIndex,
        IReadOnlyList<string>? hintLines = null)
    {
        RenderHud(player, enemy, message: "");
        DrawSelectionPopup(title, optionLines, cursorIndex, hintLines ?? []);
        Console.SetCursorPosition(0, TotalRows);
    }

    // ---- Combatant sections (status box + icon) ----------------------------------

    private static void DrawCombatantSection(int topRow, PosharpInstance combatant, bool boxOnLeft, IconAnimation animation)
    {
        string[] iconLines = BuildIconLines(combatant.Species.Icon, IconRegionWidth, animation.ColumnOffset, animation.Decoration);
        HealthDisplay health = BuildHealthDisplay(combatant);

        for (int row = 0; row < BoxHeight; row++)
        {
            string boxText = row switch
            {
                0 or 4 => "+" + new string('-', BoxInnerWidth) + "+",
                1 => FormatBoxLine(combatant.Name),
                2 => FormatBoxLine($"Lvl:{combatant.Level}"),
                3 => FormatBoxLine(health.Text),
                _ => new string(' ', BoxOuterWidth)
            };

            string iconText = ConsoleScreen.ClipOrPad(iconLines[row], IconRegionWidth);
            string content = boxOnLeft ? boxText + iconText : iconText + boxText;

            RowSpan? colorSpan = null;
            if (row == 3 && health.BarLength > 0)
            {
                int boxColumnStart = boxOnLeft ? 0 : IconRegionWidth;
                int barStart = 1 + boxColumnStart + 1 + health.PrefixLength; // outer pipe + box column + box's own pipe + prefix text
                colorSpan = new RowSpan(barStart, health.BarLength, health.Color);
            }
            else if (animation.Color is { } iconColor)
            {
                int iconColumnStart = boxOnLeft ? BoxOuterWidth : 0;
                colorSpan = new RowSpan(1 + iconColumnStart, IconRegionWidth, iconColor);
            }

            WriteRow(topRow + row, content, colorSpan);
        }
    }

    private static string[] BuildIconLines(string icon, int regionWidth, int columnOffset, char? decoration)
    {
        string[] rawLines = icon.Replace("\r", "").Split('\n');
        int iconWidth = rawLines.Length == 0 ? 0 : rawLines.Max(line => line.Length);
        int leftPad = Math.Max(0, (regionWidth - iconWidth) / 2 + columnOffset);
        int topPad = Math.Max(0, (BoxHeight - rawLines.Length) / 2);

        var lines = new string[BoxHeight];
        for (int row = 0; row < BoxHeight; row++)
        {
            int sourceIndex = row - topPad;
            string source = sourceIndex >= 0 && sourceIndex < rawLines.Length ? rawLines[sourceIndex] : "";
            if (source.Length == 0)
            {
                lines[row] = "";
                continue;
            }

            // The decoration wraps the glyph itself (before the centering padding is added),
            // so it always sits right next to the icon regardless of how far it's centered.
            string decorated = decoration is { } symbol ? $"{symbol}{source}{symbol}" : source;
            lines[row] = new string(' ', leftPad) + decorated;
        }

        return lines;
    }

    private readonly record struct HealthDisplay(string Text, int PrefixLength, int BarLength, ConsoleColor Color);

    private static HealthDisplay BuildHealthDisplay(PosharpInstance combatant)
    {
        double fraction = combatant.MaxHealthPoints == 0
            ? 0
            : (double)combatant.CurrentHealthPoints / combatant.MaxHealthPoints;

        int filled = Math.Clamp((int)Math.Round(fraction * HealthBarSegments), 0, HealthBarSegments);
        ConsoleColor color = fraction switch
        {
            > 0.5 => ConsoleColor.Green,
            > 0.2 => ConsoleColor.Yellow,
            _ => ConsoleColor.Red
        };

        string prefix = $"HP:{combatant.CurrentHealthPoints} <";
        string bar = new string('#', filled);
        string suffix = new string(' ', HealthBarSegments - filled) + ">";

        return new HealthDisplay(prefix + bar + suffix, prefix.Length, filled, color);
    }

    private static string FormatBoxLine(string text) => "|" + ConsoleScreen.ClipOrPad(text, BoxInnerWidth) + "|";

    // ---- Move selection popup ------------------------------------------------------

    private const int DescriptionPanelInnerWidth = 28;
    private const int DescriptionPanelHeight = 8;
    private const int PopupGap = 1;

    private static void DrawMovePopup(IReadOnlyList<MoveInstance> moves, int cursorIndex, bool showDescription)
    {
        int movePopupOuterWidth = PopupInnerWidth + 2;
        int descPopupOuterWidth = DescriptionPanelInnerWidth + 2;
        int totalWidth = showDescription ? movePopupOuterWidth + PopupGap + descPopupOuterWidth : movePopupOuterWidth;
        int startCol = Math.Max(0, (OuterWidth - totalWidth) / 2);

        DrawMoveListBox(startCol, moves, cursorIndex);

        if (showDescription)
        {
            DrawDescriptionBox(startCol + movePopupOuterWidth + PopupGap, moves[cursorIndex].BaseMove);
        }
    }

    private static void DrawMoveListBox(int startCol, IReadOnlyList<MoveInstance> moves, int cursorIndex)
    {
        ConsoleScreen.WriteAt(PopupTopRow, startCol, "+" + new string('-', PopupInnerWidth) + "+");
        ConsoleScreen.WriteAt(PopupTopRow + 1, startCol, FormatPopupLine("Choose a move:"));
        ConsoleScreen.WriteAt(PopupTopRow + 2, startCol, FormatPopupLine("(Esc: back, Q: info)"));

        for (int i = 0; i < moves.Count; i++)
        {
            MoveInstance move = moves[i];
            string cursor = i == cursorIndex ? ">" : " ";
            string line = $"{cursor}{move.BaseMove.Name} (PP {move.CurrentPP}/{move.MaxPP})";
            ConsoleScreen.WriteAt(PopupTopRow + 3 + i, startCol, FormatPopupLine(line));
        }

        ConsoleScreen.WriteAt(PopupTopRow + 3 + moves.Count, startCol, "+" + new string('-', PopupInnerWidth) + "+");
    }

    private static void DrawDescriptionBox(int startCol, Move move)
    {
        const int contentRows = DescriptionPanelHeight - 4; // 2 borders + title + blank spacer
        List<string> wrapped = ConsoleScreen.WrapText(move.Description, DescriptionPanelInnerWidth);

        ConsoleScreen.WriteAt(PopupTopRow, startCol, "+" + new string('-', DescriptionPanelInnerWidth) + "+");
        ConsoleScreen.WriteAt(PopupTopRow + 1, startCol, FormatDescriptionLine(move.Name));
        ConsoleScreen.WriteAt(PopupTopRow + 2, startCol, FormatDescriptionLine(""));

        for (int i = 0; i < contentRows; i++)
        {
            string line = i < wrapped.Count ? wrapped[i] : "";
            ConsoleScreen.WriteAt(PopupTopRow + 3 + i, startCol, FormatDescriptionLine(line));
        }

        ConsoleScreen.WriteAt(PopupTopRow + DescriptionPanelHeight - 1, startCol, "+" + new string('-', DescriptionPanelInnerWidth) + "+");
    }

    private static string FormatDescriptionLine(string text) => "|" + ConsoleScreen.ClipOrPad(text, DescriptionPanelInnerWidth) + "|";

    // ---- Generic selection popup (items, deck switching, move learning) ------------

    private static void DrawSelectionPopup(string title, IReadOnlyList<string> optionLines, int cursorIndex, IReadOnlyList<string> hintLines)
    {
        int startCol = Math.Max(0, (OuterWidth - (PopupInnerWidth + 2)) / 2);

        ConsoleScreen.WriteAt(PopupTopRow, startCol, "+" + new string('-', PopupInnerWidth) + "+");
        ConsoleScreen.WriteAt(PopupTopRow + 1, startCol, FormatPopupLine(title));

        int row = PopupTopRow + 2;
        foreach (string hint in hintLines)
        {
            ConsoleScreen.WriteAt(row, startCol, FormatPopupLine(hint));
            row++;
        }

        for (int i = 0; i < optionLines.Count; i++)
        {
            string cursor = i == cursorIndex ? ">" : " ";
            ConsoleScreen.WriteAt(row, startCol, FormatPopupLine($"{cursor}{optionLines[i]}"));
            row++;
        }

        ConsoleScreen.WriteAt(row, startCol, "+" + new string('-', PopupInnerWidth) + "+");
    }

    private static string FormatPopupLine(string text) => "|" + ConsoleScreen.ClipOrPad(text, PopupInnerWidth) + "|";

    // ---- Menu line -------------------------------------------------------------

    private static string BuildMenuLine(int cursorIndex)
    {
        var parts = new List<string>();
        for (int i = 0; i < MenuLabels.Length; i++)
        {
            string cursor = i == cursorIndex ? ">" : " ";
            parts.Add($"{cursor}{i + 1}: {MenuLabels[i]}");
        }
        return string.Join("   ", parts);
    }

    // ---- Low-level row writing ---------------------------------------------------

    private readonly record struct RowSpan(int Start, int Length, ConsoleColor Color);

    private static void WriteRow(int row, string content, RowSpan? colorSpan = null)
    {
        string full = "|" + ConsoleScreen.ClipOrPad(content, InnerWidth) + "|";

        Console.SetCursorPosition(0, row);
        if (colorSpan is not { } span || span.Length <= 0)
        {
            Console.Write(full);
            return;
        }

        int start = Math.Clamp(span.Start, 0, full.Length);
        int end = Math.Clamp(span.Start + span.Length, start, full.Length);

        Console.Write(full[..start]);
        Console.ForegroundColor = span.Color;
        Console.Write(full[start..end]);
        Console.ResetColor();
        Console.Write(full[end..]);
    }

    private static void WriteBorderRow(int row) => ConsoleScreen.WriteAt(row, 0, "+" + new string('-', InnerWidth) + "+");
}
