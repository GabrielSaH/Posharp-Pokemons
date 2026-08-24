using Pokete.World;

namespace Pokete.Core;

/// <summary>
/// Desenha mapas e a interface no terminal. Substitui a dependência do
/// projeto original na biblioteca própria do autor, `scrap_engine`.
///
/// A renderização usa um diff de "células sujas" contra o quadro anterior
/// (a mesma técnica que o próprio scrap_engine usa) em vez de chamar
/// Console.Clear() a cada quadro: só as células cujo caractere ou cor
/// realmente mudou são reescritas no console. Console.Clear() apaga todo o
/// buffer de tela antes de redesenhar, o que causa o piscar visível ao se
/// mover - o diff evita isso quase por completo, já que um único passo só
/// muda um par de células (a posição antiga e a nova do jogador).
/// </summary>
public class Renderer
{
    /// <summary>
    /// Glifo usado para representar o jogador no mapa. '☺' (U+263A) aparece como
    /// uma carinha/pessoa simples no Console do Windows (e em qualquer terminal com
    /// fonte que cubra símbolos Unicode básicos), diferente do estilo roguelike '@'.
    /// Troque essa constante única pra testar outra aparência, ex: '☻', '♀'/'♂', '☃'.
    /// </summary>
    private const char PlayerSymbol = '☺';

    private readonly int _viewportWidth;
    private readonly int _viewportHeight;

    // Buffer de tela: o que está desenhado atualmente em cada (x, y) da viewport.
    // null significa "desconhecido / precisa (re)desenhar".
    private readonly (char Ch, ConsoleColor Color)?[,] _lastFrame;
    private GameMap? _lastMap;
    private bool _consoleCleared;

    private string _lastMapNameLine = string.Empty;
    private string _lastMessageLine = string.Empty;

    public Renderer(int viewportWidth = 60, int viewportHeight = 20)
    {
        _viewportWidth = viewportWidth;
        _viewportHeight = viewportHeight;
        _lastFrame = new (char, ConsoleColor)?[viewportWidth, viewportHeight];
        Console.CursorVisible = false;
    }

    public void DrawMap(GameMap map, int playerX, int playerY)
    {
        EnsureConsoleReady(map);

        int camX = Math.Clamp(playerX - _viewportWidth / 2, 0, Math.Max(0, map.Width - _viewportWidth));
        int camY = Math.Clamp(playerY - _viewportHeight / 2, 0, Math.Max(0, map.Height - _viewportHeight));

        for (int y = 0; y < _viewportHeight; y++)
        {
            for (int x = 0; x < _viewportWidth; x++)
            {
                int mapX = camX + x;
                int mapY = camY + y;

                char ch;
                ConsoleColor color;

                if (mapX == playerX && mapY == playerY)
                {
                    ch = PlayerSymbol;
                    color = ConsoleColor.Yellow;
                }
                else if (map.HasBall(mapX, mapY))
                {
                    // A loose Poketeball on the ground - a small red 'o'. This is purely a
                    // render-time overlay (nothing in the tile grid itself is touched), so
                    // collecting it just removes it from the map's ball set and the next
                    // frame naturally falls back to drawing whatever tile is really there.
                    ch = 'o';
                    color = ConsoleColor.Red;
                }
                else
                {
                    var obj = map.At(mapX, mapY);
                    ch = obj?.Symbol ?? ' ';
                    color = obj?.Color ?? ConsoleColor.Gray;
                }

                var cell = (ch, color);
                if (_lastFrame[x, y] != cell)
                {
                    WriteAt(x, y, ch, color);
                    _lastFrame[x, y] = cell;
                }
            }
        }

        WriteLineIfChanged(_viewportHeight + 1, map.DisplayName, ConsoleColor.White, ref _lastMapNameLine);
    }

    public void ShowMessage(string text) =>
        WriteLineIfChanged(_viewportHeight + 2, text, ConsoleColor.Cyan, ref _lastMessageLine);

    /// <summary>Força um redesenho completo no próximo quadro (ex: ao sair de uma tela de batalha que usou o console).</summary>
    public void Invalidate()
    {
        for (int y = 0; y < _viewportHeight; y++)
            for (int x = 0; x < _viewportWidth; x++)
                _lastFrame[x, y] = null;
        _lastMapNameLine = string.Empty;
        _lastMessageLine = string.Empty;
        _consoleCleared = false;
    }

    private void EnsureConsoleReady(GameMap map)
    {
        // Um clear real acontece só aqui - na entrada do jogo e em toda troca de mapa
        // (a única vez que a viewport inteira pode mudar de conteúdo de uma vez) -
        // nunca a cada quadro, que é o que causaria piscar.
        if (!_consoleCleared || !ReferenceEquals(_lastMap, map))
        {
            ConsoleScreen.ClearScreen();
            _consoleCleared = true;
            _lastMap = map;
            for (int y = 0; y < _viewportHeight; y++)
                for (int x = 0; x < _viewportWidth; x++)
                    _lastFrame[x, y] = null;
            _lastMapNameLine = string.Empty;
            _lastMessageLine = string.Empty;
        }
    }

    private void WriteLineIfChanged(int row, string text, ConsoleColor color, ref string lastValue)
    {
        string padded = text.PadRight(_viewportWidth);
        if (padded == lastValue) return;

        Console.SetCursorPosition(0, row);
        Console.ForegroundColor = color;
        Console.Write(padded);
        Console.ResetColor();
        lastValue = padded;
    }

    private void WriteAt(int x, int y, char c, ConsoleColor color)
    {
        Console.SetCursorPosition(x, y);
        Console.ForegroundColor = color;
        Console.Write(c);
        Console.ResetColor();
    }
}
