namespace Pokete.Models;

/// <summary>
/// Controla a quantidade de cada item (por identificador, correspondendo ao
/// ItemInfo.Id de Data/Generated/GeneratedItems.cs) que um treinador possui.
/// </summary>
public class Inventory
{
    private readonly Dictionary<string, int> _counts = new();

    public int Count(string itemId) => _counts.GetValueOrDefault(itemId);

    public void Add(string itemId, int amount = 1) =>
        _counts[itemId] = Count(itemId) + amount;

    public bool TryUse(string itemId, int amount = 1)
    {
        if (Count(itemId) < amount) return false;
        _counts[itemId] -= amount;
        return true;
    }

    public IReadOnlyDictionary<string, int> Snapshot => _counts;
}
