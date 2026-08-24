namespace Pokete.World;

/// <summary>
/// Qualquer coisa que possa ser desenhada no mapa: jogador, NPCs, tiles,
/// grama alta, Poketeballs no chão, etc. É o análogo em C# das classes
/// Object/Entity do scrap_engine, sobre as quais o jogo original constrói tudo.
/// </summary>
public class MapObject
{
    public char Symbol { get; set; }
    public ConsoleColor Color { get; set; } = ConsoleColor.Gray;
    public int X { get; set; }
    public int Y { get; set; }
    public bool IsSolid { get; set; }
    /// <summary>True para tiles de grama alta ';' que podem disparar encontros selvagens.</summary>
    public bool IsTallGrass { get; set; }
    /// <summary>
    /// True para tiles de água rasa. Andável como a grama (não sólido), reservado
    /// como gancho para encontros com poketes aquáticos depois - ainda não ligado ao GameEngine.
    /// </summary>
    public bool IsWater { get; set; }
    /// <summary>
    /// True para tiles de trecho de barro/terra. Andável (não sólido), reservado
    /// como gancho para encontros com poketes do tipo Terra depois - ainda não ligado ao GameEngine.
    /// </summary>
    public bool IsMud { get; set; }
    /// <summary>Id opcional de um NpcTrainer que este objeto representa.</summary>
    public string? TrainerId { get; set; }
    /// <summary>Identificador de item opcional que este objeto representa (ex: uma Poketeball no chão).</summary>
    public string? ItemId { get; set; }
    /// <summary>True para a enfermeira do Posharp Center: esbarrar nela cura o time inteiro (HP e PP), sem batalha.</summary>
    public bool IsHealer { get; set; }
}
