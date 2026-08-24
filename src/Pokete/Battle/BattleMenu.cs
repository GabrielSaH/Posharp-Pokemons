using Pokete.Models;
using Pokete.Moves;

namespace Pokete.Battle;

/// <summary>
/// Owns the interactive menu loops: reads keys, moves a ">" cursor with A/D or W/S,
/// and asks <see cref="BattleRenderer"/> to redraw after every cursor move. Number keys
/// remain a direct shortcut alongside cursor navigation in both menus.
/// </summary>
public static class BattleMenu
{
    private static readonly BattleAction[] MainMenuActions =
    [
        BattleAction.Attack,
        BattleAction.Run,
        BattleAction.Inventory,
        BattleAction.Deck
    ];

    public static BattleAction PromptMainMenu(PosharpInstance player, PosharpInstance enemy, string message)
    {
        int cursorIndex = 0;
        BattleRenderer.RenderHud(player, enemy, message, showMenu: true, menuCursorIndex: cursorIndex);

        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey(intercept: true);

            int numericChoice = key.KeyChar - '1';
            if (numericChoice >= 0 && numericChoice < MainMenuActions.Length) return MainMenuActions[numericChoice];

            if (key.Key == ConsoleKey.Enter) return MainMenuActions[cursorIndex];

            int nextCursorIndex = char.ToLowerInvariant(key.KeyChar) switch
            {
                'a' => (cursorIndex - 1 + MainMenuActions.Length) % MainMenuActions.Length,
                'd' => (cursorIndex + 1) % MainMenuActions.Length,
                _ => cursorIndex
            };

            if (nextCursorIndex == cursorIndex) continue;

            cursorIndex = nextCursorIndex;
            BattleRenderer.RenderHud(player, enemy, message, showMenu: true, menuCursorIndex: cursorIndex);
        }
    }

    /// <summary>Returns the chosen move's index, or null if the player backed out with Escape.</summary>
    public static int? PromptMoveSelection(PosharpInstance player, PosharpInstance enemy, IReadOnlyList<MoveInstance> moves)
    {
        int cursorIndex = 0;
        bool showDescription = false;
        BattleRenderer.RenderMovePopup(player, enemy, moves, cursorIndex, showDescription);

        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey(intercept: true);
            if (key.Key == ConsoleKey.Escape) return null;
            if (key.Key == ConsoleKey.Enter) return cursorIndex;

            int numericChoice = key.KeyChar - '1';
            if (numericChoice >= 0 && numericChoice < moves.Count) return numericChoice;

            char pressed = char.ToLowerInvariant(key.KeyChar);
            if (pressed == 'q')
            {
                showDescription = !showDescription;
                BattleRenderer.RenderMovePopup(player, enemy, moves, cursorIndex, showDescription);
                continue;
            }

            int nextCursorIndex = pressed switch
            {
                'w' => (cursorIndex - 1 + moves.Count) % moves.Count,
                's' => (cursorIndex + 1) % moves.Count,
                _ => cursorIndex
            };

            if (nextCursorIndex == cursorIndex) continue;

            cursorIndex = nextCursorIndex;
            BattleRenderer.RenderMovePopup(player, enemy, moves, cursorIndex, showDescription);
        }
    }

    /// <summary>
    /// A titled popup list for anything else in battle that needs a choice - items,
    /// switching the active Posharp, picking which move to forget. W/S or arrows move
    /// the cursor, Enter confirms, number keys jump straight to an option. Returns null
    /// if the player backs out with Escape (only when <paramref name="allowCancel"/>).
    /// </summary>
    public static int? PromptSelection(
        PosharpInstance player,
        PosharpInstance enemy,
        string title,
        IReadOnlyList<string> options,
        IReadOnlyList<string>? hintLines = null,
        bool allowCancel = true)
    {
        int cursorIndex = 0;
        BattleRenderer.RenderSelectionPopup(player, enemy, title, options, cursorIndex, hintLines);

        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey(intercept: true);
            if (allowCancel && key.Key == ConsoleKey.Escape) return null;
            if (key.Key == ConsoleKey.Enter) return cursorIndex;

            int numericChoice = key.KeyChar - '1';
            if (numericChoice >= 0 && numericChoice < options.Count) return numericChoice;

            int nextCursorIndex = char.ToLowerInvariant(key.KeyChar) switch
            {
                'w' => (cursorIndex - 1 + options.Count) % options.Count,
                's' => (cursorIndex + 1) % options.Count,
                _ => cursorIndex
            };

            if (nextCursorIndex == cursorIndex) continue;

            cursorIndex = nextCursorIndex;
            BattleRenderer.RenderSelectionPopup(player, enemy, title, options, cursorIndex, hintLines);
        }
    }
}
