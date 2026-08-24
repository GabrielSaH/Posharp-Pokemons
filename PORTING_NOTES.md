# Update: merged with the Pokete-Pokemon (Posharp) battle engine

The overworld/map/save code below is unchanged, but **all Pokete/Attack/battle
logic described in this document has been removed and replaced** with the
"Posharp" battle system from the separate Pokete-Pokemon C# project:

- `Models/Pokete.cs`, `Models/Attack.cs`, `Battle/BattleSystem.cs`,
  `Data/TypeChart.cs`, and the `GeneratedPokes`/`GeneratedAttacks`/
  `GeneratedNatures`/`GeneratedTypes` data files are gone.
- In their place: `Models/PosharpSpecies.cs`, `Models/PosharpInstance.cs`,
  `Moves/*` (moves + their damage/heal/debuff effect system), and
  `Battle/*` (a full console battle HUD with animations, a move-info popup,
  team switching, item use and catching) — ported from Pokete-Pokemon and
  extended with XP/leveling (`PosharpInstance.GainXp`, a cubic XP curve),
  evolution, and a new `Battle/CatchSystem.cs` catch-chance formula (neither
  source project had one that fit the Posharp stat system).
- `GeneratedMoves.cs` (101 moves) and `GeneratedPosharpEspecies.cs` (25
  Posharp, including two 3-stage starter lines) replace the old move/species
  data wholesale.
- Every map's `PokeSpawnInfo` in `GeneratedMaps.cs` was rewritten to spawn
  Posharp species instead of the old roster, with level ranges rescaled to
  the new level-1-100 stat formula (the old data used the original Python
  game's very different `xp = lvl²-1` scale, up to "level" 1700, which
  doesn't mean anything under the new leveling formula). All 16 base-form
  Posharp species (every species that isn't itself an evolution) appear in
  at least one of the 23 maps that have wild encounters; several appear in
  many.
- `GameEngine.StartWildEncounter` now just builds a wild `PosharpInstance`
  and hands off to `new BattleInstance(player, wild).Start()`; the old
  inline attack-resolution loop, catch-roll and evolve-check in
  `GameEngine.cs` are gone (that logic now lives inside `BattleInstance`
  itself, alongside the ported HUD/animation code it already owned).
- `SaveManager`/`Program.cs` were updated to (de)serialize the new
  `PosharpInstance` shape (species id, level, xp, six individual values,
  per-move current PP) instead of the old Pokete/Attack/Nature shape.
- `SmokeTest` was rewritten to validate the merged system: species/move/
  item/map counts, stat/level-curve/evolution math, catch-chance behavior,
  a full save/load roundtrip, and — importantly — that every map's spawn
  pool resolves to real species and that all 16 base-form species are
  reachable somewhere in the world.

Not changed: map layouts/doors/balls, the renderer, input handling, NPC/menu
scaffolding, items/achievements/weather data. Trainer battles were not
wired in either version of the project (no trainer NPCs are placed on any
map in this codebase yet), so only wild encounters were connected.

# Update 2: popups, trainers, dialogue, no more Console.Clear()

A second round of changes on top of the merge above:

1. **Battle Inventory/Deck are popups now.** `BattleRenderer.RenderSelectionPopup`
   generalizes the move-selection popup into a reusable titled list box, used by
   `BattleMenu.PromptSelection`. The Inventory and Deck (switch active Posharp)
   battle actions now render through it instead of the old full-screen
   `MenuSystem.Choose`, so they look and feel exactly like picking a move.

2. **`Console.Clear()` is gone from the entire codebase** (verified: zero calls
   anywhere in `src/`). A new `Core/ConsoleScreen.cs` is the one place that writes
   to the console at a low level - every "clear" is an overwrite with blank padded
   rows via `SetCursorPosition`, not a buffer reset, which is what removes the
   flicker. `MenuSystem` and `Renderer.EnsureConsoleReady` were rewritten around
   it; `BattleRenderer`'s own duplicate row-writing helpers were removed in favor
   of it too.

3. **Level-up move learning is interactive.** `PosharpInstance.GainXp` now
   auto-learns a newly-eligible move into a free slot, or - once all 4 slots are
   full - reports it back as "pending" instead of silently overwriting anything.
   `BattleInstance.PromptLearnMove` shows a "forget X / don't learn" popup (via
   the same selection-popup system above) for each pending move.

4. **`Program.cs` is a flat ~35-line script again.** All the logic that used to
   live in its private static helpers moved to where it actually belongs:
   `Core/PlayerFactory.cs` (new player creation), `SaveManager.LoadPlayer`/
   `SavePlayer` (all save<->domain conversion, not just raw JSON I/O), and
   `SampleMapBuilder.FindSpawnPoint`/`BuildForPlayer` (map-related spawn logic).

5. **Trainers.** `Models/TrainerDefinition.cs` is a static blueprint (id, map +
   position, symbol, money, team as `(speciesId, level)` pairs, dialogue) - the
   trainer equivalent of `PosharpSpecies`. `NpcTrainer.FromDefinition` turns one
   into a battle-ready trainer with a real Posharp team. `BattleInstance` was
   generalized to take a `TrainerBase` opponent instead of a single
   `PosharpInstance`, and a new `WildEncounter` type wraps a single wild Posharp
   as a one-member trainer - so wild encounters and trainer battles are the exact
   same "keep sending out your next usable Posharp until someone runs out" loop,
   XP is awarded per knockout (doubled for trainers), and trainer battles disallow
   both fleeing and catching. `GameEngine` recognizes a trainer's map tile
   (`MapObject.TrainerId`, already scaffolded) and starts a conversation+battle on
   bump; `Player.DefeatedTrainerIds` (persisted in the save) stops a beaten
   trainer from fighting again.

6. **Adding a trainer is one entry.** `Data/Generated/GeneratedTrainers.cs`
   follows the exact same dictionary pattern as `GeneratedMoves.cs`/
   `GeneratedPosharpEspecies.cs`. Four sample trainers are included, placed on
   verified-walkable tiles on `playmap_1`, `playmap_51`, `playmap_2` and
   `playmap_28`.

7. **A small dialogue bar.** `Core/DialogueBar.cs` shows one or more lines near
   the bottom of the screen (`DialogueBar.Show(lines)`) - two rows, no full
   screen change, advances on Enter. Used for trainers' pre/post-fight lines.

`SmokeTest` was extended to validate all of the above: trainer team/placement
resolution, the level-up-with-full-moveset pending/learn flow, and
Money/DefeatedTrainerIds surviving a save/load roundtrip.

# Update 3: bug fixes, intro, Posharp Center, rival, cave gate, new evolutions, rebalance

1. **Held-key "teleport" bug fixed.** `InputHandler.ReadAction` now drains to the
   most recent buffered key before acting. A held movement key sends repeat
   keypresses faster than the game renders frames; without draining, they'd all
   get processed before the next redraw and the character would visually jump
   several tiles at once. Now a held key steps one tile per render instead.

2. **`Console.Clear()` is back, used sparingly.** A single call is genuinely
   useful (and doesn't cause flicker) at real screen transitions, so
   `ConsoleScreen.ClearScreen()` wraps it and is called exactly at: entering/
   leaving a battle, loading a new map (`Renderer.EnsureConsoleReady`), opening
   a menu (`MenuSystem.Choose`), the name-entry prompt, and the closing "Game
   saved" message. Every per-frame and per-keypress update still avoids it
   entirely (unchanged from before) - that's what actually caused the original
   flicker complaint.

3. **Intro dialogue.** `Core/IntroDialogue.cs` shows a short dialogue bar from
   the player's dad on character creation (turned 18, become a trainer, beat
   the gym at the end of the road in the big city) and mentions the starting
   items. `PlayerFactory.CreateNew` now also gives 5 `healing_potion` alongside
   the 5 Poketeballs.

4. **A trainer blocks the way into the cave.** Route 0 (`playmap_51`)'s only
   door into Nice Town Cave sits in a gap in an otherwise solid wall row, with
   exactly one walkable approach tile directly below it - `cave_guardian_reyes`
   stands on that tile, so the door is unreachable until they're beaten.

5. **The Posharp Center is enterable.** A door was added right in front of the
   Center building on `playmap_1` (not part of the original map_data.py - see
   the custom-features note in `SampleMapBuilder.cs`) leading to a small
   hand-built interior (`pokete_center_1` in `GeneratedMapLayouts.cs`) with a
   nurse. `MapObject` gained an `IsHealer` flag; bumping into her fully heals
   the whole team's HP and move PP, no battle, no menu - handled directly in
   `GameEngine.TryMove`.

6. **A rival, standing by the Center.** `rival_devon` is a `TrainerDefinition`
   like any other, team `[("Permafrost", 5)]`, no money reward, with dialogue
   about you both becoming trainers today.

7. **Permafrost's third evolution.** `Glacikeep` now evolves (level 45) into
   `Cryocube` - cube art, a description about the ice growing so dense it
   stopped being shaped like three dimensions.

8. **Every Posharp's base stats were rebalanced.** All 6 stats (HP, Attack,
   Defense, SpecialAttack, SpecialDefense, Initiative) summed, target roughly
   100-200 total for most species, with the final stage of 3-stage lines and a
   couple of deliberately-special evolutions allowed above that (Verdantitan
   222, Maelstriker 218, Cryocube 234, Ragefin 242). Pisharp - strong on
   purpose despite never evolving - sits at 213. Every other species' target
   total and the reasoning for its role (base/mid/evolved/standalone) is
   recorded in the rebalancing script's comments if you want to extend the
   pattern for a new species later.

9. **Splashfin's own evolution.** Deliberately weak as Splashfin (84 total,
   below every other species) - "most would rather flee than fight." Evolves
   (level 30) into `Ragefin`: same silhouette plus an angry ಠ_ಠ face, and a
   description that plays up specialists barely calling it an evolution.

`SmokeTest` still validates species/move/item/map/trainer counts, trainer
team/placement resolution, and base-species map coverage, all currently green
against the 27-species, 6-trainer, 54-map roster.

# Update 4: dialogue wrapping, Center bump-entry, visible loose balls, docs

1. **Dialogue text no longer gets clipped.** Two compounding causes fixed:
   `ConsoleScreen.EnsureSize` now requests a couple of columns beyond the
   logical `Width` (writing exactly `Width` characters to column 0 reaches the
   terminal's literal last column, and some terminals auto-wrap the cursor
   right then, eating a character or two); and `DialogueBar` now word-wraps
   each line via a new shared `ConsoleScreen.WrapText` (also adopted by
   `BattleRenderer`'s move-description panel, removing its own duplicate copy)
   instead of relying on every line of dialogue fitting on one row.

2. **Entering the Posharp Center is a deliberate bump, not a walk-through.**
   The door used to sit on the open ground tile right in front of the
   building, so simply walking up to it triggered the transition. It now sits
   on the building's own solid tile; `GameEngine.TryMove` was generalized to
   support doors on solid tiles ("bump" - the player attempts to walk into it,
   isn't allowed to actually stand there, and is teleported anyway) alongside
   the existing walkable-tile doors (every outdoor route/cave transition,
   unchanged - walk onto it and you're through).

3. **Loose Poketeballs are now visible, as a small red 'o', and their counts
   were refined.** The collection mechanism already existed
   (`GameMap`'s ball set + `GameEngine` picking it up on step-on) but had no
   visual representation. `Renderer.DrawMap` now draws a red `o` overlay
   wherever `GameMap.HasBall` is true - purely at render time, so collecting a
   ball just removes it from the set and the tile underneath renders normally
   again next frame, no tile-restoration bookkeeping needed. Counts trimmed to
   1 on `playmap_1` and 2 on `playmap_51`, keeping their original (real,
   ported) coordinates.

4. **`ARCHITECTURE.md` added** - design patterns, abstractions, and the
   reasoning behind the module structure, separate from this changelog-style
   porting-notes file.

# Update 5: Center entrance, corrected

The previous "bump to enter" fix was on the wrong tile - it required walking
to a spot slightly off from where a player would naturally stand. The
building's own art has a real door: the "___" gap between the two '#' pillars
on its face (row y=3, x=24-26), directly above the one walkable notch in its
base wall (x=25, y=4) - which is also, not coincidentally, the map's real
ported player spawn point. The door now sits there instead, so standing at
spawn (or anywhere else along that notch) and pressing "up" once enters the
Center, matching the building's actual drawn doorway instead of an arbitrary
point in front of it.

---

# Pokete → C# — Notas de Port (v2, com dados reais)

## O que mudou desde a v1

Na primeira passada eu só tinha acesso ao README/Changelog, então os dados de
Poketes/ataques eram placeholders inventados. Agora usei o **código-fonte real**
(`pokete-master.zip`, ~255 arquivos Python, ~25.700 linhas) e:

1. Extraí os dicionários literais reais de `src/pokete/data/*.py` com um parser
   AST em Python (script `codegen.py`/`extract_data.py` — não incluídos no zip
   final, mas o resultado está em `src/Pokete/Data/Generated/`).
2. Portei as **fórmulas de jogo reais**, lidas direto de:
   - `src/pokete/classes/fight/attack_process.py` → dano, chance de acerto, efetividade
   - `src/pokete/classes/fight/items/balls.py` → chance de captura
   - `src/pokete/classes/fight/fight.py` → XP, ordem de turno, fuga
   - `src/pokete/classes/poke/poke.py` → nível (`floor(sqrt(xp+1))`), stats por nível/natureza/shiny

## Dados 100% reais portados

| Fonte Python                          | Destino C#                              | Qtde |
|----------------------------------------|-------------------------------------------|------|
| `data/poketes.py`                      | `Data/Generated/GeneratedPokes.cs`         | 59 Poketes |
| `data/attacks.py`                      | `Data/Generated/GeneratedAttacks.cs`       | 69 ataques |
| `data/types.py`                        | `Data/Generated/GeneratedTypes.cs`         | 11 tipos |
| `data/natures.py`                      | `Data/Generated/GeneratedNatures.cs`       | 4 naturezas |
| `data/achievements.py`                 | `Data/Generated/GeneratedAchievements.cs`  | 4 conquistas |
| `data/weather.py`                      | `Data/Generated/GeneratedWeather.cs`       | 4 climas |
| `data/items.py`                        | `Data/Generated/GeneratedItems.cs`         | 8 itens |
| `data/maps.py` (só metadados)           | `Data/Generated/GeneratedMaps.cs`          | 53 mapas |

Cada Pokete tem nome, hp, atc, defesa, lista de ataques, pool de ataques
aleatórios, miss_chance, descrição, lose_xp, raridade, tipos, evolução
(espécie+nível), iniciativa, se é noturno/diurno, e até a arte ASCII (`ico`).

## Fórmulas de jogo portadas fielmente

- **Nível**: `lvl = floor(sqrt(xp + 1))` — sem tabela de XP, é direto da fórmula.
- **Stats**: `atc/defesa/iniciativa = round((lvl + base + (2 se shiny)) * multiplicador_natureza)`.
  **HP não escala com nível** — só o valor base da espécie (+5 se shiny). Isso é
  uma peculiaridade real do jogo, não um bug do port.
- **Dano**: fator aleatório `{0(erro), 0.75, 1, 1.26}` ponderado por
  `attack.miss_chance + attacker.miss_chance`; efetividade `1.3`/`0.5`/`1` por tipo;
  `dano = max(3, round(atc * fator_ataque / max(defesa,1) * fator_aleatorio * efetividade))`.
- **Captura**: `chance = (hp_max/hp_atual * chance_da_bola) / (isso + hp_max)`.
  Poketeball=1, Superball=6, Hyperball=1000 (chances relativas).
- **XP de vitória**: soma de `lose_xp + max(0, nivel_perdedor - nivel_vencedor)` de
  cada Pokete do time perdedor, dobrado em duelo de treinador.
- **Fuga**: falha se `random(0,100) < clamp(50 - (iniciativa_prÃ³pria - iniciativa_inimiga), 5, 95)`.
- Aprendizado de novo ataque a cada 5 níveis (`lvl % 5 == 0`), evolução por
  nível mínimo (`evolve_poke`/`evolve_lvl`).

Tudo isso está em `Battle/BattleSystem.cs` e `Models/Pokete.cs`, com comentários
apontando o arquivo/função Python de origem de cada fórmula.

## Compilação — testada de verdade

Diferente da v1, desta vez consegui instalar o .NET 8 SDK (via `apt-get install
dotnet-sdk-8.0`, que estava disponível no repositório Ubuntu liberado na política
de rede) e **compilar e rodar o projeto de fato**, incluindo um `SmokeTest`
que valida:
- Carregamento dos 59 Poketes / 69 ataques / 8 itens / 53 mapas reais
- Fórmula de nível (`xp=24` → nível 5, confirmado)
- Stats calculados batendo com a fórmula (Steini Lv.1: Atc 3, Def 5, Init 6)
- Filtro de ataques por `min_lvl` no momento da criação (Steini nível 1 não
  aprende "Brick Throw", que exige nível 15 — igual ao Python)
- Chance de captura variando corretamente com HP
- Round-trip de save/load em JSON

```shell
cd PoketeCSharp
dotnet build src/Pokete/Pokete.csproj      # o jogo
dotnet run --project src/Pokete            # jogar
dotnet run --project src/SmokeTest         # rodar as validações acima
```

Um `NuGet.Config` com `<clear/>` foi incluído porque o projeto não tem
dependências externas (usa só a BCL do .NET 8) — isso evita que o `dotnet
build` tente acessar a internet à toa.

## O que ainda é simplificado/placeholder

- **Mapas**: só os metadados são reais (nome, dimensões, poketes que aparecem
  e faixa de nível, clima, música). O layout de tiles é gerado
  proceduralmente (paredes na borda + um retângulo de grama alta), porque o
  layout ASCII real está em `src/pokete/data/map_data.py` (~4200 linhas) e não
  foi portado. Dá pra portar depois com o mesmo approach de codegen.
- **Efeitos de status** (queimadura, paralisia, confusão, etc. — em
  `src/pokete/classes/effects.py`): os identificadores de efeito (`attack.Effect`)
  já estão nos dados, mas a aplicação/resolução do efeito não está implementada.
- **Ações especiais de ataque** (`attack.Action`, ex: "cry", "chocer" —
  `src/pokete/classes/attack_actions.py`): também só o identificador é
  portado, sem a lógica.
- **UI real** (curses/scrap_engine com mouse, animações, caixas de diálogo
  bonitas): recriei um menu simples em `System.Console`.
- **Multiplayer** (servidor Go + protobuf RPC em `pkg/server/`, `bs_rpc/`):
  não portado — é essencialmente um projeto backend à parte.
- **NPCs com diálogo**, loja, Pokete-Care, mods: estrutura pronta em
  `World/Npc.cs` mas sem os dados reais de `data/npcs.py`/`data/trainers.py`
  (822 e 469 linhas — dá pra portar com o mesmo codegen se quiser).

## Próximos passos sugeridos (se quiser continuar)

1. Rodar `codegen.py` de novo incluindo `npcs.py`, `trainers.py`,
   `mapstations.py` para ter treinadores e NPCs reais.
2. Portar `effects.py` e `attack_actions.py` para status effects funcionarem.
3. Se precisar do layout real dos mapas, portar `map_data.py` com um parser
   similar (é ASCII posicional, dá pra extrair com regex/AST também).

## Atualização: mapas reais portados (map_data.py)

Os **53 mapas do jogo original** agora são reais, não mais procedurais:

- Extraídos de `src/pokete/data/map_data.py` (~4200 linhas) via parser AST,
  resolvendo as constantes `CENTER`/`SHOP`/`HOUSE1` referenciadas nos dicionários.
- Cada mapa foi "achatado" em duas grades de caracteres (parede/casas/árvores
  = sólido; grama/decoração = andável) e gerado em
  `Data/Generated/GeneratedMapLayouts.cs`.
- **Portas reais** (`dors` do Python) viraram transições de mapa de verdade:
  andar sobre uma porta muda de mapa e reposiciona o jogador nas coordenadas
  exatas de destino (`GameEngine.TryMove` → `ChangeMap`).
- **Bolas no chão** (`balls`) dão um Poketeball real ao serem coletadas.
- O ponto de entrada do jogador num mapa novo usa o `special_dors` original
  (a "porta principal"), com fallback pra primeira célula andável encontrada.

Validado no `SmokeTest`: `playmap_1` ("Nice Town", 91x25, 533 objetos) tem uma
porta real em (90,12) que leva a `playmap_51` ("Route 0", 110x40) exatamente
como no jogo original, e a bola em (54,4) existe conforme os dados reais.

**O que ainda não foi portado dessa parte**: cores exatas dos objetos (usei
heurística simples: árvores em verde escuro, resto em cinza — o original usa
`scrap_engine` com paletas por objeto que não estão no `map_data.py`), e NPCs
posicionados no mapa (ficam em `data/npcs.py`/`data/trainers.py`, não
`map_data.py` — dá pra portar com o mesmo approach de codegen se quiser).
