# Architecture & Design Documentation

This document explains **how the codebase is put together and why**, for anyone
picking this project up cold. `PORTING_NOTES.md` is a changelog of *what* was
built across sessions; this file is the map of *how it fits together* and the
reasoning behind the structural decisions, so new content (moves, Posharp,
trainers, maps) and new features can be added consistently.

## Table of contents

1. [Project layout](#1-project-layout)
2. [Core philosophy](#2-core-philosophy)
3. [Design patterns used, and why](#3-design-patterns-used-and-why)
4. [Abstractions catalogue](#4-abstractions-catalogue-abstract-classes--interfaces)
5. [Module-by-module tour](#5-module-by-module-tour)
6. [The rendering model](#6-the-rendering-model-no-consoleclear-almost-anywhere)
7. [The battle system, end to end](#7-the-battle-system-end-to-end)
8. [How to extend the game](#8-how-to-extend-the-game)
9. [Known limitations / deliberately out of scope](#9-known-limitations--deliberately-out-of-scope)

---

## 1. Project layout

```
src/Pokete/
├── Program.cs              entry point - deliberately tiny, see §5
├── Core/                   application/orchestration layer (no game *rules* live here)
├── World/                  the map/tile data model
├── Models/                 domain objects: Posharp, trainers, items, map metadata
├── Moves/                  the move & move-effect system
│   └── Effects/            Strategy-pattern move behaviors (Interfaces + Implementations)
├── Battle/                 the battle engine and its rendering/UI
├── Data/                   map assembly logic (SampleMapBuilder) + …
│   └── Generated/          …the actual game content, as plain data dictionaries
└── Menu/                   the one generic overworld menu widget
```

`src/SmokeTest/` is a separate console project (see its own `Program.cs`) that
exercises the domain layer directly - no UI, no `Console.ReadKey` - as a fast
correctness check for stat formulas, save/load, and content-table integrity
that runs in under a second. Prefer extending it over manually clicking through
the game when validating new content.

---

## 2. Core philosophy

Four decisions shape almost everything else in this codebase:

**Content is data, not code.** Every move, Posharp species, trainer, and map is
a plain object literal in a `Data/Generated/Generated*.cs` dictionary. There is
no per-species subclass, no per-move method override, no per-trainer script.
Adding content is adding a dictionary entry, never writing new control flow.
See [§8](#8-how-to-extend-the-game).

**Domain logic doesn't know about the console.** `Models/`, `Moves/`, and the
non-rendering half of `Battle/` never call `Console.*` directly (the one
structural exception, `BattleInstance`, is explained in §7). Stats, XP curves,
catch odds, damage formulas - all of it is testable by just calling methods and
reading return values, which is exactly what `SmokeTest` does.

**One low-level place owns the screen.** Every pixel the game ever draws goes
through `Core/ConsoleScreen.cs`. Nothing else calls `Console.SetCursorPosition`
or `Console.Write` on its own. See [§6](#6-the-rendering-model-no-consoleclear-almost-anywhere).

**Wild encounters and trainer battles are the same code path.** `BattleInstance`
takes a `TrainerBase` opponent, not a single Posharp. A wild encounter is just
a one-member `TrainerBase` (`WildEncounter`). This was a deliberate refactor
(see the porting notes for "Trainers") specifically to avoid two parallel,
slowly-diverging battle loops.

---

## 3. Design patterns used, and why

### Static Factory Method
*Where:* `PosharpInstance`'s constructor (species + level → a fully-statted
instance), `NpcTrainer.FromDefinition(TrainerDefinition)`, `PlayerFactory.CreateNew()`.

*Why:* Constructing a battle-ready object from a data blueprint takes several
steps (roll individual values, compute six stats from a formula, derive the
starting moveset) that must always happen together and in the right order. A
factory method makes "there is exactly one correct way to build this" explicit
and keeps that logic in one place instead of copy-pasted at every call site.

### Strategy Pattern
*Where:* `Moves/Effects/` - `IMoveEffect` plus the narrower capability
interfaces `IDamageEffect`, `IHealSelfEffect`, `IStatusOnEnemy`, implemented by
`NormalDamageEffect`, `SpecialDamageEffect`, `HealSelfEffect`,
`StatDebuffEffect`, `DamageWithDebuffEffect`.

*Why:* A `Move` needs one of several *shapes* of behavior (pure damage, pure
heal, pure debuff, or damage-plus-chance-of-debuff), and new shapes may be
added later. Rather than a `Move` class with a `MoveKind` enum and a growing
`switch` statement, each shape is its own small class behind `IMoveEffect`, and
`MoveInstance.Execute` just calls `BaseMove.Effect.Execute(user, target)`
without caring which one it got. Adding a sixth move-effect shape (e.g. a
multi-hit move) means writing one new class, not editing five others.

The narrower interfaces (`IDamageEffect`, `IHealSelfEffect`, `IStatusOnEnemy`)
exist so effects can be composed - `DamageWithDebuffEffect` implements *three*
interfaces because it genuinely does three things - without forcing every
implementation to also implement methods it has no meaningful answer for
(Interface Segregation: a pure heal doesn't need to answer "how much damage do
you do").

### Template-driven content tables (a lightweight Repository)
*Where:* Every `Data/Generated/Generated*.cs` file - `GeneratedMoves.All`,
`GeneratedPosharpEspecies.All`, `GeneratedTrainers.All`, `GeneratedMaps.All`,
`GeneratedItems.All`, etc.

*Why:* These are `Dictionary<string, T>` acting as an in-memory content
repository, keyed by a stable string id. Everything downstream (a
`PosharpInstance`, an `NpcTrainer`, a `GameMap`) is *built from* an entry in one
of these tables rather than hdeveloper-written control flow, which is what
makes content additions mechanical rather than architectural. See §8 for the
concrete "add one entry" recipe for each content type.

### Blueprint / Instance split
*Where:* `PosharpSpecies` (immutable blueprint) vs. `PosharpInstance` (a
specific, owned, mutable Posharp with individual values, current HP, XP, and a
concrete move list) - and the same shape again for `TrainerDefinition`
(blueprint) vs. `NpcTrainer` (a built, battle-ready trainer with a real
`List<PosharpInstance> Deck`).

*Why:* "What a Fire-type starter's base stats are" and "this specific level-14
Fire starter that fainted twice and knows Ember" are different concerns with
different lifetimes - one is shared, read-only, defined once; the other is
created per-encounter and mutated during play. Splitting them means the
blueprint can be trivially data-driven (see the pattern above) while the
instance can carry all the runtime state without polluting the species table
with fields like `CurrentHealthPoints` that only make sense for one specific
Posharp.

### Shared abstract base + composition over inheritance
*Where:* `abstract class TrainerBase` (`Models/Trainer.cs`), extended by both
`Player` and `NpcTrainer`.

*Why:* A player and an NPC trainer are both, fundamentally, "something with a
team of up to 6 Posharp that can fight" - `Deck`, `UsablePosharps`,
`HasUsablePosharp` belong on both and should behave identically on both, which
is exactly what an abstract base class is for. Everything that *isn't* shared
(a `Player`'s inventory, money, and map position vs. an `NpcTrainer`'s
map placement and dialogue) stays on the concrete subclass rather than being
forced up into the base "just in case" - composition (a `Player` *has* an
`Inventory`) is preferred over cramming unrelated concerns into the hierarchy.

### Immutable value objects / records
*Where:* `MoveResult` and its subtypes (`DamageResult`, `HealResult`,
`StatusResult`, `MissDamageResult`), `BattleLogEntry`, `LevelUpOutcome`,
`IconAnimation` (a `readonly record struct`).

*Why:* These are all "here is exactly what happened" snapshots - the outcome of
one move, one level-up, one animation frame - that get read once and never
mutated. C# records give value equality and a compact declaration for free,
and using a small type hierarchy for `MoveResult` (rather than one class with a
bunch of nullable fields) means `BattleInstance.BuildLogEntry` can pattern-match
(`result as DamageResult`) instead of checking which fields happen to be set.

### Stateless static utility / "Singleton by construction"
*Where:* `ConsoleScreen`, `MenuSystem`, `BattleRenderer`, `BattleMenu`,
`SampleMapBuilder`, `CatchSystem`.

*Why:* None of these classes have meaningful per-instance state - "render this
HUD state" or "build this map id" doesn't need an object identity, just a
function of its inputs. Making them `static class`es avoids the ceremony of a
singleton instance (or worse, an injected-everywhere service) for something
that's really just a namespaced group of pure(-ish) functions. `BattleInstance`
and `GameEngine`, by contrast, *do* carry meaningful per-battle/per-session
state (whose turn, current HP, current map) and are ordinary instantiated
classes.

### Enum-driven state / outcome signaling
*Where:* `GameAction`, `BattleAction`, `BattleResult`, `MoveOutcome`,
`MoveCategory`.

*Why:* These are small, closed sets of "what just happened" or "what should
happen next" that are checked with a `switch`/pattern match at exactly one or
two call sites. A full state-machine framework would be overkill; a plain enum
keeps the possible states greppable and exhaustive-switch-checkable by the
compiler.

### Builder-style assembly (not the formal GoF Builder, but the same spirit)
*Where:* `SampleMapBuilder.Build(mapId)`.

*Why:* A playable `GameMap` is assembled from several independent data sources
in sequence - the real ported ASCII layout (`GeneratedMapLayouts`), doors and
balls from that same layout, hand-added extras that aren't part of the
original data (`AddCustomFeatures` - ponds, the Posharp Center's door, its
nurse), and finally any trainers whose `TrainerDefinition.MapId` matches
(`AddTrainers`). Keeping this as one method with clearly-named, single-purpose
private steps (rather than scattering map-construction logic across the
codebase) means "what does building a map actually do, in order" is answerable
by reading one method top to bottom.

---

## 4. Abstractions catalogue (abstract classes & interfaces)

| Type | Kind | Implementors / subclasses | Purpose |
|---|---|---|---|
| `TrainerBase` | `abstract class` | `Player`, `NpcTrainer` | Shared "has a team and can fight" contract. |
| `IMoveEffect` | `interface` | `NormalDamageEffect`, `SpecialDamageEffect`, `HealSelfEffect`, `StatDebuffEffect`, `DamageWithDebuffEffect` | The one method every move effect must have: `Execute(user, target) → MoveResult`. |
| `IDamageEffect` | `interface` | `NormalDamageEffect`, `SpecialDamageEffect`, `DamageWithDebuffEffect` | "This effect can compute a base damage number" - used internally by the implementations, not by outside callers. |
| `IHealSelfEffect` | `interface` | `HealSelfEffect` | "This effect can compute a heal amount." |
| `IStatusOnEnemy` | `interface` | `StatDebuffEffect`, `DamageWithDebuffEffect` | "This effect can compute a debuff percentage." |
| `MoveResult` | `abstract record` | `DamageResult`, `HealResult`, `StatusResult`, `MissDamageResult` | "What actually happened when a move was used" - one shape per kind of outcome. |

Everything else in the domain layer (`PosharpSpecies`, `PosharpInstance`,
`Move`, `MoveInstance`, `TrainerDefinition`, `GameMap`, `MapObject`, …) is a
concrete class with no inheritance hierarchy - they didn't need one, and
introducing one "for consistency" would only add indirection without adding
flexibility anywhere it's actually used.

---

## 5. Module-by-module tour

### `Program.cs`
Deliberately as short as possible: parse `--help`, show the main menu, dispatch
to `PlayerFactory.CreateNew()` or `SaveManager.LoadPlayer()`, build the current
map, run the game loop, save on exit. It intentionally contains **no** logic
that could instead live on a more specific class - see §8's "keep `Program.cs`
thin" note if you're tempted to add a private helper method here.

### `Core/`
The orchestration layer - things that coordinate other layers but don't own
game rules themselves.
- `GameEngine` - the overworld loop: read input, move the player, notice tall
  grass / trainers / the healer / doors, hand off to `BattleInstance` when a
  fight starts.
- `ConsoleScreen` - see §6.
- `Renderer` - draws the world-map viewport using a dirty-cell diff (§6).
- `InputHandler` - keybindings, and the fix for held-key "teleporting" (drains
  to the most recent buffered key before acting - see its doc comment).
- `PlayerFactory` - builds a brand-new `Player` (name prompt, starter, starting
  items, spawn point, intro dialogue).
- `SaveManager` - both the raw JSON I/O *and* the full `SaveData ⇄ Player`
  conversion (individual values, per-move PP, money, defeated trainers - all
  of it). Nothing outside this file needs to know the save file's shape.
- `IntroDialogue`, `DialogueBar` - the small non-modal dialogue bar (§6) and
  the specific intro speech shown on character creation.

### `World/`
The map/tile data model, with no rendering or input logic in it.
- `GameMap` - a 2D grid of `MapObject?` plus door/ball lookup tables.
- `MapObject` - one tile's worth of state: symbol, color, solidity, and the
  various interaction flags (`IsTallGrass`, `IsWater`, `IsMud`, `TrainerId`,
  `IsHealer`). A tile is data, not a class hierarchy - "what kind of tile is
  this" is answered by which flags are set, since tiles can combine
  attributes (e.g. a trainer could in principle stand on grass) more easily as
  flags than as a fixed subtype per kind.
- `Npc`/`DialogueNode` - scaffolding for branching, non-trainer NPC dialogue.
  Not currently wired into any map; kept for a future pass.

### `Models/`
The domain objects proper.
- `PosharpSpecies` / `PosharpInstance` - see the Blueprint/Instance pattern above.
- `Move` / `MoveInstance` / `MoveResult` (+ `LevelUpOutcome`) - the move system's
  data types (the *behavior* lives in `Moves/Effects/`, imported via `Move.Effect`).
- `TrainerDefinition` / `Trainer.cs` (`TrainerBase`, `Player`, `NpcTrainer`) -
  see the Blueprint/Instance pattern above.
- `Item.cs` (`Inventory`) - a simple id→count map with `Add`/`TryUse`.
- `Metadata.cs` - the smaller, mostly-static data-shape records that don't
  need their own file: `AchievementInfo`, `WeatherInfo`, `PokeSpawnInfo`,
  `MapInfo`, `DoorInfo`, `MapLayout`, `ItemInfo`.

### `Moves/`
The move system. `Move` is the static definition (power, accuracy, PP, the
level it's learned at, and an `IMoveEffect`); `MoveInstance` is one Posharp's
copy of a move with its own current PP. `Effects/` holds the Strategy-pattern
behaviors described in §3.

### `Battle/`
The battle engine and everything needed to run one battle to completion:
- `BattleInstance` - owns the turn loop, all game-rule decisions (who attacks
  first, whether a catch/flee succeeds, XP and evolution), *and* drives the
  presentation (it's the one place domain logic and console output are
  intentionally not separated - see §7 for why).
- `BattleRenderer` - pure rendering: given a battle state, draw it. No turn
  logic, no input.
- `BattleMenu` - pure input: read keys, move a cursor, return a choice. No
  rendering of its own (it asks `BattleRenderer` to redraw after every cursor
  move) and no game rules.
- `CatchSystem` - the catch-chance formula, `static`, no state.
- `WildEncounter` - see §2/§7: wraps one wild Posharp as a one-member
  `TrainerBase` so the main loop never has a "wild vs. trainer" branch.
- `BattleAction` / `BattleResult` / `BattleLogEntry` - small supporting types.

### `Data/`
- `SampleMapBuilder` - see the Builder-style pattern in §3.
- `Generated/` - the actual content tables (§3's Repository pattern). Despite
  the folder name, not everything here was literally code-generated from the
  original Python project - some tables (Posharp, moves, trainers, and the
  Posharp Center's interior layout) are hand-authored new content that simply
  follows the same "one big dictionary" shape as the ported ones, for
  consistency and so the same extension recipe (§8) works for all of it.

### `Menu/`
`MenuSystem` - the one generic "pick an option from a list" widget used
outside of battle (the main menu, the deck view). Battle has its own richer
popup system in `Battle/BattleRenderer.cs` / `BattleMenu.cs` because it needs
things `MenuSystem` doesn't (a persistent HUD behind the popup, a
description-toggle panel for moves) - see §6 for why they don't share one
widget.

---

## 6. The rendering model: no `Console.Clear()` (almost) anywhere

Every screen in the game writes through `Core/ConsoleScreen.cs`, which owns:

- `Width` / `Height` - the fixed column/row budget every screen lays out
  within.
- `WriteRow` / `WriteAt` - write text at an absolute row/column, always padded
  to `Width` so a shorter new string fully overwrites a longer old one.
- `WrapText` - shared greedy word-wrap, used by both the battle move
  description panel and the dialogue bar, so no dialogue line silently loses
  its last few words for being a character too long.
- `ClearRows` - "clear" a row range by overwriting it with blanks. This is the
  default way to clear *anything* - it never causes the terminal-repaint flash
  a real `Console.Clear()` does, because it's just an ordinary write, not a
  full buffer reset.
- `ClearScreen` - a thin, clearly-named wrapper around the *real*
  `Console.Clear()`, used **only** at genuine screen transitions: entering or
  leaving a battle, loading a new map, opening a top-level menu. Calling the
  real `Clear()` once at a real transition is fine and guarantees no artifact
  survives it; calling it every frame or every keypress is what caused the
  original flicker this design avoids. If you're about to add a new
  `Console.Clear()` call anywhere, ask first whether it's a real screen
  transition (call `ConsoleScreen.ClearScreen()`) or an in-place update (use
  `WriteRow`/`ClearRows` instead).

Three renderers sit on top of this shared foundation, each suited to a
different update pattern rather than sharing one widget:

- **`Renderer`** (the world map) repaints via a **dirty-cell diff** against a
  cached previous frame (`_lastFrame`), because the world redraws every single
  loop iteration and most cells don't change between one step and the next -
  diffing is the cheapest way to keep that smooth.
- **`BattleRenderer`** repaints its **entire fixed frame unconditionally**
  every call, because a battle HUD is small, changes almost every field on
  almost every redraw (HP bars, animations), and unconditional writes are
  simpler to reason about than diffing when there's little to save by diffing.
- **`DialogueBar`** only ever touches its own small fixed row range at the
  bottom of the screen and is explicitly *not* a screen change - showing a
  line of dialogue never disturbs whatever's drawn above it (the world map,
  mid-battle HUD, etc.), which is why it's a stateless couple of `WriteRow`
  calls rather than going through `ClearScreen`.

`EnsureSize()` requests a couple of columns beyond the logical `Width` when
sizing the console buffer/window. Writing exactly `Width` characters starting
at column 0 reaches the terminal's literal last column, and some terminals
auto-wrap the cursor the instant that happens, which can visibly eat the last
character or two of a line - reserving real margin columns that are never
written to avoids that class of bug entirely.

---

## 7. The battle system, end to end

`BattleInstance` is the one place in the codebase where domain logic and
console output are deliberately **not** separated the way they are everywhere
else. This is a conscious exception, not an oversight: a battle is a tight
loop of "decide something → immediately show the player what happened →
decide the next thing", and splitting "decide" from "show" into two objects
communicating through an event queue would add a layer of indirection with no
real payoff for a single-player, turn-based, one-thing-happens-at-a-time
battle. `BattleRenderer`/`BattleMenu` still exist as separate classes for the
*rendering* and *input* concerns specifically because those genuinely are
reusable/replaceable independent of turn logic (see §5) - it's specifically
"what should happen this turn" and "print it" that stay fused.

**Symmetric opponents.** `BattleInstance`'s constructor takes a
`Player trainer` and a `TrainerBase opponent` - never a bare `PosharpInstance`.
A wild encounter constructs a `WildEncounter` (one-member `TrainerBase`)
first. This means the entire turn loop, fainting/force-switch handling, and
victory condition (`while (_trainer.HasUsablePosharp && _opponent.HasUsablePosharp)`)
is written once and is correct for both a 1-vs-1 wild fight and a full 4-vs-6
trainer battle without an `if (isTrainerBattle)` branch anywhere in the loop
itself - the only branching on `_isWildBattle` is in the two places where wild
and trainer battles are *supposed* to differ: catching is only legal in a wild
battle, and you can't flee from a trainer.

**Per-knockout consequences.** XP, leveling, move-learning prompts, and
evolution checks all happen the instant an opponent's Posharp faints
(`AwardKnockoutXp`), not once at the end of the whole battle. This is what
makes a multi-Posharp trainer battle correctly let you level up (and even
evolve) mid-fight, from KOs earlier in that same battle, before the trainer's
next Posharp is sent out.

**Level-up move learning never silently overwrites a move.**
`PosharpInstance.GainXp` auto-learns a newly-eligible move straight into a free
slot; once all 4 slots are full, it's returned instead as a *pending* id in
`LevelUpOutcome`, and it's `BattleInstance.PromptLearnMove` - not the model
layer - that turns that into a popup asking the player what to forget. Keeping
the "ask the player" step out of `PosharpInstance` is what keeps the model
layer free of `Console` calls (§2).

---

## 8. How to extend the game

Every one of these is "add one dictionary entry", by design (§3):

**A new move** → add an entry to `GeneratedMoves.All` in
`Data/Generated/GeneratedMoves.cs`. Pick a `MoveCategory`, a base power/accuracy/PP,
the level it's learned at, and construct the matching `IMoveEffect`
implementation (`NormalDamageEffect`, `SpecialDamageEffect`, `HealSelfEffect`,
`StatDebuffEffect`, or `DamageWithDebuffEffect`). No other file needs to change.

**A new Posharp species** → add an entry to `GeneratedPosharpEspecies.All` in
`Data/Generated/GeneratedPosharpEspecies.cs`. Base stats should sum to roughly
100-200 across HP/Attack/Defense/SpecialAttack/SpecialDefense/Initiative for a
normal species; final-stage evolutions and other deliberately-special cases can
run higher (see the existing roster for examples of both). Set
`idPosharpEvolvesInto`/`evolveLvl` if it evolves. Add its id to at least one
map's wild-encounter pool in `GeneratedMaps.cs` if it should be catchable, or
to a trainer's team in `GeneratedTrainers.cs`.

**A new trainer** → add an entry to `GeneratedTrainers.All` in
`Data/Generated/GeneratedTrainers.cs`: an id, a map + position + display
symbol, a team as `(speciesId, level)` pairs, and pre-/post-fight dialogue.
`SampleMapBuilder.AddTrainers` automatically places any trainer whose
`MapId` matches the map being built - nothing else needs to change. Double
check the chosen `(x, y)` is walkable in the target map before picking it
(see any of the existing trainers' placement comments for the pattern of
verifying this against the real map layout first).

**A new map feature that isn't part of the original ported data** (a pond, a
building door, an NPC that isn't a trainer) → add a `case` to
`SampleMapBuilder.AddCustomFeatures` for that map id. This is the sanctioned
place for hand-added content on top of the real ported layouts - see its own
doc comment.

**A new item** → add an entry to `GeneratedItems.All` in
`Data/Generated/GeneratedItems.cs`, and (if it should do something in battle)
a case in `BattleInstance.TryUseItem`'s `switch` on `item.Fn`.

If you find yourself writing new control flow instead of a new data entry for
any of the above, that's usually a sign the change belongs in one of the
`Generated*` tables instead - or, if it's genuinely a new *kind* of content
(not a new instance of an existing kind), a sign that a new small data type
plus dictionary (following the existing `TrainerDefinition`/`GeneratedTrainers`
shape) is the more consistent way to add it than one-off code.

**Keep `Program.cs` thin.** If a new top-level feature needs setup logic,
give it its own class in `Core/` (the way `PlayerFactory` and `SaveManager`
already are) rather than adding a private static method to `Program`. The
entry point should stay readable top-to-bottom as "here's the sequence of
things that happen", not accumulate helpers.

---

## 9. Known limitations / deliberately out of scope

- **No status effects beyond stat debuffs** (no poison/paralysis/sleep-style
  turn-based conditions). `StatDebuffEffect`/`DamageWithDebuffEffect` cover
  everything currently in the move roster; adding a real status-effect system
  would mean giving `PosharpInstance` a notion of an active condition and
  teaching `BattleInstance`'s turn loop to apply it each turn - a bigger,
  deliberate feature addition rather than a data-only one.
- **No type effectiveness chart.** The ported Posharp battle math (borrowed
  from the original Pokete-Pokemon prototype) never had one; types are tracked
  per-species/move for flavor only.
- **Trainers currently battle once.** `Player.DefeatedTrainerIds` is a flat
  set, not a per-trainer "rebattle available after X" rule, so there's no
  built-in support for a rival-style trainer who fights you again later in the
  game - that would need its own re-triggerable trainer state, not just a
  second `TrainerDefinition` entry.
- **`World/Npc.cs`'s branching-dialogue scaffolding (`DialogueNode`) isn't
  wired into any map yet.** Non-trainer NPCs currently have no way to appear
  in the world; only trainers (via `TrainerId`) and the Center's healer (via
  `IsHealer`) are implemented as tile interactions.
- **No shop / currency sink.** `Player.Money` accumulates from trainer battles
  with nowhere to spend it yet.
