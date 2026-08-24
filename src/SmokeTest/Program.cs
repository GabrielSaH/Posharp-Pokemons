using Pokete.Core;
using Pokete.Data.Generated;
using Pokete.Models;

Console.WriteLine("=== Smoke test: merged Posharp battle system ===");
Console.WriteLine($"Posharp species loaded: {GeneratedPosharpEspecies.All.Count}");
Console.WriteLine($"Moves loaded: {GeneratedMoves.All.Count}");
Console.WriteLine($"Items loaded: {GeneratedItems.All.Count}");
Console.WriteLine($"Maps loaded: {GeneratedMaps.All.Count}");
Console.WriteLine($"Trainers loaded: {GeneratedTrainers.All.Count}");

var pisharpSpecies = GeneratedPosharpEspecies.All["Pisharp"];
var starter = new PosharpInstance(pisharpSpecies, level: 5, xp: PosharpInstance.XpForLevel(5));
Console.WriteLine($"\nStarter: {starter.Species.Name} Lv.{starter.Level} HP {starter.CurrentHealthPoints}/{starter.MaxHealthPoints} " +
                   $"Atk {starter.Attack} Def {starter.Defense} SpA {starter.SpecialAttack} SpD {starter.SpecialDefense} Init {starter.Initiative}");
Console.WriteLine($"Moves: {string.Join(", ", starter.Moves.Select(m => $"{m.BaseMove.Name}(PP {m.CurrentPP}/{m.MaxPP})"))}");

// ---- Level curve check ----
Console.WriteLine("\n=== Level curve checks ===");
Console.WriteLine($"XpForLevel(5) = {PosharpInstance.XpForLevel(5)} -> LevelForXp should be 5, got: {PosharpInstance.LevelForXp(PosharpInstance.XpForLevel(5))}");
Console.WriteLine($"LevelForXp(0) = {PosharpInstance.LevelForXp(0)} (should clamp to 1)");

// ---- GainXp + incremental move learning + evolution check ----
Console.WriteLine("\n=== GainXp / move-learning / evolution checks ===");
var sproutling = new PosharpInstance(GeneratedPosharpEspecies.All["Sproutling"], level: 1, xp: 0);
Console.WriteLine($"Fresh Lv.1 Sproutling moves: {string.Join(", ", sproutling.Moves.Select(m => m.BaseMove.Name))} (should be < 4, no pending)");

// Level it up a lot in one jump - once it already knows 4 moves, any further newly
// eligible move should come back as "pending" instead of silently overwriting one.
var outcome = sproutling.GainXp(PosharpInstance.XpForLevel(20) - sproutling.Xp);
Console.WriteLine($"After jumping to ~level 20: leveled={outcome.LeveledUp}, now Lv.{sproutling.Level}, " +
                   $"knows {sproutling.Moves.Count} moves, pending-to-learn: {outcome.PendingNewMoveIds.Count} " +
                   $"[{string.Join(", ", outcome.PendingNewMoveIds)}]");

if (outcome.PendingNewMoveIds.Count > 0)
{
    string forgetName = sproutling.Moves[0].BaseMove.Id;
    sproutling.LearnMove(outcome.PendingNewMoveIds[0], 0);
    Console.WriteLine($"LearnMove replaced slot 0 ({forgetName}) with {sproutling.Moves[0].BaseMove.Name} - now: {string.Join(", ", sproutling.Moves.Select(m => m.BaseMove.Name))}");
}

int oldMax = sproutling.MaxHealthPoints;
sproutling.GainXp(PosharpInstance.XpForLevel(sproutling.Species.EvolveLvl) - sproutling.Xp);
if (sproutling.Level >= sproutling.Species.EvolveLvl && GeneratedPosharpEspecies.All.TryGetValue(sproutling.Species.IdPosharpEvolvesInto!, out var evolved))
{
    sproutling.SetSpecies(evolved);
    sproutling.RemakeStats();
    Console.WriteLine($"Evolved into: {sproutling.Species.Name}, HP {oldMax} -> {sproutling.MaxHealthPoints}");
}

// ---- Catch chance check ----
Console.WriteLine("\n=== Catch chance checks ===");
var wildTarget = new PosharpInstance(GeneratedPosharpEspecies.All["Splashfin"], level: 10);
double fullHpChance = Pokete.Battle.CatchSystem.GetCatchChance(wildTarget, Pokete.Battle.CatchSystem.BallMultiplier["poketeball"]);
wildTarget.TakeDamage(wildTarget.MaxHealthPoints - 1);
double lowHpChance = Pokete.Battle.CatchSystem.GetCatchChance(wildTarget, Pokete.Battle.CatchSystem.BallMultiplier["poketeball"]);
Console.WriteLine($"Poketeball vs full-HP wild: {fullHpChance:P1}");
Console.WriteLine($"Poketeball vs near-dead wild: {lowHpChance:P1} (should be higher)");

// ---- Trainer roster checks ----
Console.WriteLine("\n=== Trainer roster checks ===");
int badTeamRefs = 0, badPlacements = 0;
foreach (var (id, def) in GeneratedTrainers.All)
{
    if (!GeneratedMaps.All.ContainsKey(def.MapId))
    {
        Console.WriteLine($"  !! {id} placed on unknown map '{def.MapId}'");
        badPlacements++;
    }
    else
    {
        var map = Pokete.Data.SampleMapBuilder.Build(def.MapId);
        var tile = map.At(def.X, def.Y);
        bool ok = tile is { TrainerId: not null };
        Console.WriteLine($"  {id} on {def.MapId} at ({def.X},{def.Y}): tile tagged correctly = {ok}");
        if (!ok) badPlacements++;
    }

    foreach (var (speciesId, level) in def.Team)
    {
        if (!GeneratedPosharpEspecies.All.ContainsKey(speciesId))
        {
            Console.WriteLine($"  !! {id} team references unknown species '{speciesId}'");
            badTeamRefs++;
        }
    }

    if (def.PreFightDialogue.Length == 0) Console.WriteLine($"  !! {id} has no pre-fight dialogue");

    // Build it end-to-end the same way NpcTrainer.FromDefinition does
    var npc = NpcTrainer.FromDefinition(def);
    Console.WriteLine($"  -> built '{npc.Name}': {npc.Deck.Count} Posharp, ${npc.Money} reward, team: " +
                       $"{string.Join(", ", npc.Deck.Select(p => $"{p.Species.Name} Lv.{p.Level}"))}");
}
Console.WriteLine($"Bad team references: {badTeamRefs}, bad map placements: {badPlacements}");

// ---- Save/load roundtrip (including Money + DefeatedTrainerIds) ----
Console.WriteLine("\n=== Save/load roundtrip ===");
var player = new Player { Name = "TestTrainer", Money = 150 };
player.Deck.Add(starter);
player.DefeatedTrainerIds.Add("youngster_bruno");
player.Inventory.Add("poketeball", 3);
SaveManager.SavePlayer(player);
var loaded = SaveManager.LoadPlayer();
Console.WriteLine($"Save/load roundtrip: player={loaded?.Name}, money={loaded?.Money}, " +
                   $"defeatedTrainers=[{string.Join(",", loaded?.DefeatedTrainerIds ?? [])}], " +
                   $"deck[0]={loaded?.Deck[0].Species.Id} Lv.{loaded?.Deck[0].Level}, path={SaveManager.GetSaveFilePath()}");

// ---- Map + wild encounter pool checks ----
Console.WriteLine("\n=== Map layout + wild encounter pool checks ===");
var map1 = Pokete.Data.SampleMapBuilder.Build("playmap_1");
Console.WriteLine($"playmap_1 '{map1.DisplayName}' size {map1.Width}x{map1.Height}, objects placed: {map1.Objects.Count}");

var door = map1.GetDoor(90, 12);
Console.WriteLine($"Door at (90,12) -> {(door is not null ? $"{door.TargetMap} ({door.TargetX},{door.TargetY})" : "NONE")}");
Console.WriteLine($"Ball at (54,4): {map1.HasBall(54, 4)}");

int grassCells = 0;
for (int y = 0; y < map1.Height; y++)
for (int x = 0; x < map1.Width; x++)
    if (map1.At(x, y) is { IsTallGrass: true }) grassCells++;
Console.WriteLine($"Tall grass cells found: {grassCells}");

int mapsWithPool = 0, badRefs = 0;
var seenAcrossAllMaps = new HashSet<string>();
foreach (var (mapId, info) in GeneratedMaps.All)
{
    if (info.PokeArgs is null) continue;
    mapsWithPool++;
    foreach (var speciesId in info.PokeArgs.Pokes)
    {
        seenAcrossAllMaps.Add(speciesId);
        if (!GeneratedPosharpEspecies.All.ContainsKey(speciesId))
        {
            Console.WriteLine($"  !! {mapId} references unknown species '{speciesId}'");
            badRefs++;
        }
    }
}
Console.WriteLine($"Maps with a wild spawn pool: {mapsWithPool}, bad species references: {badRefs}");

var evolvedInto = GeneratedPosharpEspecies.All.Values
    .Where(s => s.CanEvolve)
    .Select(s => s.IdPosharpEvolvesInto!)
    .ToHashSet();
var baseFormSpecies = GeneratedPosharpEspecies.All.Keys.Where(id => !evolvedInto.Contains(id)).ToList();
var missingFromMaps = baseFormSpecies.Where(id => !seenAcrossAllMaps.Contains(id)).ToList();
Console.WriteLine($"Base-form species: {baseFormSpecies.Count}, missing from every map's pool: {missingFromMaps.Count}");
if (missingFromMaps.Count > 0) Console.WriteLine($"  Missing: {string.Join(", ", missingFromMaps)}");

Console.WriteLine("\n=== Smoke test complete ===");
