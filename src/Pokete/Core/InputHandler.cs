namespace Pokete.Core;

public enum GameAction
{
    MoveUp,
    MoveDown,
    MoveLeft,
    MoveRight,
    OpenDeck,
    OpenSettings,
    Confirm,
    Cancel,
    QuickAttack1,
    QuickAttack2,
    QuickAttack3,
    QuickAttack4,
    None
}

/// <summary>
/// Traduz teclas pressionadas em ações do jogo usando uma tabela de vínculos
/// remapeável, seguindo a funcionalidade "Added remappable controls" da v0.8.0
/// e o esquema WASD + ataque rápido (yzcv) descrito no README.
/// </summary>
public class InputHandler
{
    private readonly Dictionary<ConsoleKey, GameAction> _bindings = new()
    {
        [ConsoleKey.W] = GameAction.MoveUp,
        [ConsoleKey.S] = GameAction.MoveDown,
        [ConsoleKey.A] = GameAction.MoveLeft,
        [ConsoleKey.D] = GameAction.MoveRight,
        [ConsoleKey.D1] = GameAction.OpenDeck,
        [ConsoleKey.E] = GameAction.OpenSettings,
        [ConsoleKey.Enter] = GameAction.Confirm,
        [ConsoleKey.Escape] = GameAction.Cancel,
        [ConsoleKey.Y] = GameAction.QuickAttack1,
        [ConsoleKey.Z] = GameAction.QuickAttack2,
        [ConsoleKey.C] = GameAction.QuickAttack3,
        [ConsoleKey.V] = GameAction.QuickAttack4,
    };

    public void Rebind(ConsoleKey key, GameAction action) => _bindings[key] = action;

    public GameAction ReadAction()
    {
        var keyInfo = Console.ReadKey(intercept: true);

        // A held key sends repeat keypresses into the input buffer faster than we
        // render frames. Without this, all of them get processed back-to-back before
        // the next redraw, so the character visually "teleports" several tiles at
        // once instead of stepping smoothly. Draining to only the most recent key
        // means a held key still moves - just one tile per render, like a tap.
        while (Console.KeyAvailable) keyInfo = Console.ReadKey(intercept: true);

        return _bindings.GetValueOrDefault(keyInfo.Key, GameAction.None);
    }
}
