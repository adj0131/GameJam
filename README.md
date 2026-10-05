# Welcome to the Junkyard (GMTK Game Jam 2026)

This repository contains the Unity source project for **Welcome to the Junkyard**, created for the **GMTK Summer Game Jam 2026**.

- Team size: 4
- Jam result: Top 18%
- Engine: Unity `6000.4.7f1`

## Current repository purpose

This repo is intended to preserve and collaborate on the **project source files** (assets, scripts, package manifest, and project settings), not generated local files or build output.

If you only want to play the game, use the itch.io page:

- https://dark-subtilizer.itch.io/welcome-to-the-junkyard

## Prerequisites

- Unity Hub
- Unity Editor `6000.4.7f1`

## Open the project

1. Clone or download this repository.
2. In Unity Hub, choose **Add/Open project**.
3. Select: `GameJam/` (the Unity project folder inside the repo).
4. Let Unity finish importing.
5. Open a scene from `Assets/Scenes/` (for example `MainMenu.unity`).

## Important folders/files

Inside `GameJam/`:

- `Assets/` — game content (scenes, scripts, prefabs, sprites, sounds, animations, videos)
- `Packages/manifest.json` and `Packages/packages-lock.json` — Unity package definitions
- `ProjectSettings/` — shared Unity project configuration

## Scene files currently present

- `MainMenu.unity`
- `OpeningMovie.unity`
- `MainScene.unity`
- `Level-1.unity`
- `Level2.unity`
- `Level3.unity`
- `Level 4.unity`
- `FirstCelebration.unity`
- `SecondCelebration.unity`
- `Congrats.unity`

## Contribution notes

Please avoid committing local-only/generated artifacts such as:

- Unity cache/build folders (`Library/`, `Temp/`, `Logs/`, `Build*/`, etc.)
- IDE-generated files (`.vs/`, `.idea/`, `.vscode/`, `*.csproj`, `*.sln*`)
- OS-specific files (`.DS_Store`, `Thumbs.db`, `Desktop.ini`)
- Build archives or exported binaries (`*.zip`, standalone build folders)

## Unknowns / intentionally not assumed

This repository does not currently include automated tests, CI build scripts, or a documented command-line build pipeline. Use the Unity Editor for running and building unless a workflow is added later.

## License

No license has been specified in this repository. Until one is added, all rights remain with the project owners.
