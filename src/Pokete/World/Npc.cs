namespace Pokete.World;

/// <summary>
/// Um NPC não-treinador que pode manter um diálogo ramificado com várias
/// respostas, seguindo a funcionalidade "NPCs are way smarter now and support
/// multi-answer chats" do Changelog v0.7.0.
/// </summary>
public class DialogueNode
{
    public required string Text { get; init; }
    public List<(string Answer, DialogueNode Next)> Choices { get; init; } = new();
}

public class Npc
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required MapObject MapObject { get; init; }
    public DialogueNode? Dialogue { get; init; }
}
