# Crack Rock

**A turn-based combat prototype in Unity**, with player progression, ability buttons, mana/health UI, and a simple battle loop against an opponent.

---

## Status

Educational / personal prototype. Core battle flow is implemented (start battle, abilities, meditate, flee, health/mana UI). Not a finished commercial game.

## Tech stack

| Area | What it uses |
|---|---|
| Engine | **Unity 2022.3.46f1** |
| UI | uGUI + **TextMesh Pro** |
| Language | **C#** |

## What's in the project

| System | Key files |
|---|---|
| Battle loop (turns, health/mana, flee, meditate) | `Assets/_Scripts/BattleManager.cs` |
| Abilities | `Assets/_Scripts/Ability.cs` |
| Player controller / progression | `Assets/_Scripts/PlayerContoller.cs`, `Assets/_Scripts/PlayerProgression.cs` |
| Camera follow | `Assets/_Scripts/CameraFollow.cs` |
| Menus / pause / music / achievements | `Assets/_Scripts/MainMenuManager.cs`, `PauseManager.cs`, `MusicManager.cs`, `AchievementManager.cs` |

## About this repository

Public showcase of a Unity combat prototype. Third-party TextMesh Pro examples may be present in the tree and are not authored gameplay code.
