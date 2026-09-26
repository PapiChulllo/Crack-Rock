# Crack Rock

**A 2D top-down exploration / turn-based battle prototype in Unity.** Walk a tile map, trigger random grass encounters, then fight with mana-cost abilities, meditation, and flee. Includes a main menu, pause, music, and a simple achievement hook.

---

## What it does

- **Overworld (`MainScene`):** grid-style movement (`PlayerController`) with collision against a solid layer, animator walk directions, footstep SFX, and a **10%** random battle chance when stepping on the grass layer → loads `BattleScene`.
- **Battle (`BattleScene`):** `BattleManager` spawns player/opponent prefabs, drives turn UI (ability buttons from `PlayerProgression`, meditate, flee), tracks health/mana TMP fields, and returns to the overworld when the battle ends.
- **Meta:** `MainMenuManager`, `PauseManager`, `MusicManager`, `CameraFollow`, `AchievementManager` (first-win style unlock path referenced from battle flow).

**Status / limitations:** student/prototype scope. Filename typo `PlayerContoller.cs` (class is still `PlayerController`). Abilities start as Punch/Kick only; progression unlock surface is minimal. A `BuildForWebGL/` folder exists in the repo (WebGL build output present), but this documentation pass did **not** re-run or verify that build in Unity. No automated tests.

## Tech stack

| Area | What it uses |
|---|---|
| Engine | **Unity** `2022.3.46f1` |
| Rendering / 2D | Built-in 2D feature set (`com.unity.feature.2d`) |
| UI | uGUI + **TextMesh Pro** |
| Input | Legacy `Input.GetAxisRaw` |
| Audio | `AudioSource` footsteps + `MusicManager` |

## What's in the project

Authored game scripts live under `Assets/_Scripts/` (~9 files). Much of the repo is sprites, tiles, animations, music, prefabs (`Prefapbs` folder name as committed), and TextMesh Pro examples.

| System | Key files |
|---|---|
| Overworld move, grass encounters | `Assets/_Scripts/PlayerContoller.cs` |
| Turn battle, abilities, flee/meditate | `Assets/_Scripts/BattleManager.cs`, `Ability.cs`, `PlayerProgression.cs` |
| Menu / pause / music / camera / achievements | `MainMenuManager.cs`, `PauseManager.cs`, `MusicManager.cs`, `CameraFollow.cs`, `AchievementManager.cs` |

### Code / system highlights

- **Encounter gate:** after each completed step, `Physics2D.OverlapCircle` on `grassLayer`; `Random.Range(1, 101) <= 10` loads `BattleScene`.
- **Battle UI binding:** ability buttons labeled from `UnlockedAbilities` with mana gating; meditate restores mana; flee ends battle.
- **Progression static store:** `PlayerProgression` holds the unlocked ability list in memory (not a full save system).

## Scenes

| Build order (Editor Build Settings) | Scene | Purpose |
|---|---|---|
| 0 | `Assets/Scenes/MainMenuScene.unity` | Main menu |
| 1 | `Assets/Scenes/MainScene.unity` | Overworld exploration |
| 2 | `Assets/Scenes/BattleScene.unity` | Encounter battle |
| — | `Assets/Scenes/SampleScene.unity` | Leftover sample; not in Build Settings |

TextMesh Pro **Examples & Extras** scenes are also present; they are package demos, not game content.

## Third-party assets

| Asset | Notes |
|---|---|
| **TextMesh Pro** (including Examples & Extras) | UI text + large example scene/script tree |
| Sprites / tiles / music under `Assets/` | Used by the game; no separate license manifest is committed — do not assume redistributable rights without checking source |

## About this repository

Public prototype / portfolio project by **Melih Burak** (PapiChulllo). Educational/indie experiment depth — not a polished commercial release. Documentation is source-derived; Unity Play mode was not re-run for this README.
