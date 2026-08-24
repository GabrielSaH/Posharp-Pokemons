using System.Text.Json;
using System.Text.Json.Serialization;
using Pokete.Data.Generated;
using Pokete.Models;

namespace Pokete.Core;

/// <summary>
/// Dados de save simples e legíveis, seguindo o espírito das entradas do
/// changelog do projeto original "Made the savefile json" (v0.6.0) e
/// "Cleaned up save file to be more readable for humans" (v0.4.1).
/// </summary>
public class SaveData
{
    public string PlayerName { get; set; } = string.Empty;
    public string CurrentMapId { get; set; } = "playmap_1";
    public int X { get; set; }
    public int Y { get; set; }
    public int Money { get; set; }
    public double PlaytimeSeconds { get; set; }
    public DateTime StartupTime { get; set; }

    public List<SavedPosharp> Deck { get; set; } = new();
    public List<SavedPosharp> Storage { get; set; } = new();
    public Dictionary<string, int> Inventory { get; set; } = new();
    public List<string> UnlockedAchievements { get; set; } = new();
    public List<string> SeenSpecies { get; set; } = new();
    public List<string> CaughtSpecies { get; set; } = new();
    public List<string> DefeatedTrainers { get; set; } = new();
}

public class SavedPosharp
{
    public string SpeciesId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Level { get; set; }
    public int Xp { get; set; }
    public int CurrentHp { get; set; }

    // Individual values (0-31), rolled once when the Posharp is first created and
    // preserved forever after - restoring them on load keeps its exact stats stable
    // across save/load instead of re-rolling a slightly different Posharp.
    public int AttackIv { get; set; }
    public int DefenseIv { get; set; }
    public int SpecialAttackIv { get; set; }
    public int SpecialDefenseIv { get; set; }
    public int InitiativeIv { get; set; }
    public int HealthIv { get; set; }

    public List<SavedMove> Moves { get; set; } = new();
}

public class SavedMove
{
    public string MoveId { get; set; } = string.Empty;
    public int CurrentPp { get; set; }
}

/// <summary>
/// Reads/writes the save file (XDG-style save directory, following the original
/// project's "save location is based on XDG dirs" behavior) and owns the full
/// conversion between a live <see cref="Player"/> and the on-disk <see cref="SaveData"/>
/// shape, so nothing outside this class needs to know the save format.
/// </summary>
public static class SaveManager
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public static string GetSaveDirectory()
    {
        string baseDir = Environment.GetEnvironmentVariable("XDG_DATA_HOME")
            ?? Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        return Path.Combine(baseDir, "pokete");
    }

    public static string GetSaveFilePath() => Path.Combine(GetSaveDirectory(), "pokete.json");

    public static bool SaveExists() => File.Exists(GetSaveFilePath());

    public static void SavePlayer(Player player)
    {
        Directory.CreateDirectory(GetSaveDirectory());
        var data = new SaveData
        {
            PlayerName = player.Name,
            CurrentMapId = player.CurrentMapId,
            X = player.X,
            Y = player.Y,
            Money = player.Money,
            PlaytimeSeconds = player.Playtime.TotalSeconds,
            StartupTime = player.StartupTime,
            Deck = player.Deck.Select(ToSavedPosharp).ToList(),
            Storage = player.Storage.Select(ToSavedPosharp).ToList(),
            Inventory = player.Inventory.Snapshot.ToDictionary(kv => kv.Key, kv => kv.Value),
            UnlockedAchievements = player.UnlockedAchievements.ToList(),
            SeenSpecies = player.SeenSpecies.ToList(),
            CaughtSpecies = player.CaughtSpecies.ToList(),
            DefeatedTrainers = player.DefeatedTrainerIds.ToList(),
        };

        File.WriteAllText(GetSaveFilePath(), JsonSerializer.Serialize(data, Options));
    }

    /// <summary>Loads and rebuilds a full <see cref="Player"/>, or null if there's no save / it has no usable Posharp.</summary>
    public static Player? LoadPlayer()
    {
        string path = GetSaveFilePath();
        if (!File.Exists(path)) return null;

        var data = JsonSerializer.Deserialize<SaveData>(File.ReadAllText(path), Options);
        if (data is null) return null;

        var player = new Player
        {
            Name = data.PlayerName,
            CurrentMapId = string.IsNullOrEmpty(data.CurrentMapId) ? "playmap_1" : data.CurrentMapId,
            X = data.X,
            Y = data.Y,
            Money = data.Money,
            StartupTime = data.StartupTime,
            Playtime = TimeSpan.FromSeconds(data.PlaytimeSeconds),
        };

        foreach (var id in data.UnlockedAchievements) player.UnlockedAchievements.Add(id);
        foreach (var id in data.SeenSpecies) player.SeenSpecies.Add(id);
        foreach (var id in data.CaughtSpecies) player.CaughtSpecies.Add(id);
        foreach (var id in data.DefeatedTrainers) player.DefeatedTrainerIds.Add(id);
        foreach (var (id, count) in data.Inventory) player.Inventory.Add(id, count);

        foreach (var saved in data.Deck)
            if (RestorePosharp(saved) is { } instance) player.Deck.Add(instance);
        foreach (var saved in data.Storage)
            if (RestorePosharp(saved) is { } instance) player.Storage.Add(instance);

        return player.Deck.Count == 0 ? null : player;
    }

    private static PosharpInstance? RestorePosharp(SavedPosharp saved)
    {
        if (!GeneratedPosharpEspecies.All.TryGetValue(saved.SpeciesId, out var species)) return null;

        var instance = new PosharpInstance(
            species,
            name: string.IsNullOrWhiteSpace(saved.Name) ? species.Name : saved.Name,
            level: saved.Level,
            xp: saved.Xp,
            attackIv: saved.AttackIv,
            defenseIv: saved.DefenseIv,
            specialAttackIv: saved.SpecialAttackIv,
            specialDefenseIv: saved.SpecialDefenseIv,
            initiativeIv: saved.InitiativeIv,
            healthIv: saved.HealthIv);

        instance.CurrentHealthPoints = Math.Clamp(saved.CurrentHp, 0, instance.MaxHealthPoints);

        // SetMovesByLevel (run inside the constructor above) already gave the fresh move
        // set full PP; restore each move's exact saved PP where the move id still matches.
        foreach (var savedMove in saved.Moves)
        {
            var existing = instance.Moves.FirstOrDefault(m => m.BaseMove.Id == savedMove.MoveId);
            if (existing is not null) existing.CurrentPP = Math.Clamp(savedMove.CurrentPp, 0, existing.MaxPP);
        }

        return instance;
    }

    private static SavedPosharp ToSavedPosharp(PosharpInstance p) => new()
    {
        SpeciesId = p.Species.Id,
        Name = p.Name,
        Level = p.Level,
        Xp = p.Xp,
        CurrentHp = p.CurrentHealthPoints,
        AttackIv = p.AttackIndividualValue,
        DefenseIv = p.DefenseIndividualValue,
        SpecialAttackIv = p.SpecialAttackIndividualValue,
        SpecialDefenseIv = p.SpecialDefenseIndividualValue,
        InitiativeIv = p.InitiativeIndividualValue,
        HealthIv = p.HealthIndividualValue,
        Moves = p.Moves.Select(m => new SavedMove { MoveId = m.BaseMove.Id, CurrentPp = m.CurrentPP }).ToList()
    };
}
