# GMTK Summer Game Jam 2026 Project

This repository contains a Unity game created for the **GMTK Summer Game Jam 2026**.

The project was submitted as a game-jam collaboration and placed in the **top 18% of entries**.

> **Jam:** GMTK Summer Game Jam 2026  
> **Result:** Top 18%

## About the project

`GameJam` is a Unity-based game project featuring multiple levels, animated scenes, enemies, player systems, audio, visual effects, and video sequences. The repository contains the source project as well as Windows builds for playing the submitted game.

## Features and content

- Multiple playable levels
- Main menu and scene transitions
- Player gameplay systems
- Enemy spawning and enemy-related gameplay scripts
- Hit effects and visual feedback
- Celebration and completion scenes
- Opening movie/video sequence
- 2D sprites, animations, sounds, prefabs, and materials
- Windows builds included in the repository

## Technology

- **Game engine:** Unity `6000.4.7f1`
- **Primary language:** C#
- **Rendering and materials:** ShaderLab and HLSL
- **Platform:** Windows build included
- **Project type:** Unity 2D game

## Repository structure

```text
GameJam/
├── GameJam/
│   ├── Assets/
│   │   ├── Animations/       # Animation assets
│   │   ├── InputSystem/      # Input configuration and actions
│   │   ├── Prefabs/          # Reusable Unity prefabs
│   │   ├── Scenes/           # Menus, levels, cutscenes, and endings
│   │   ├── Scripts/          # Gameplay and scene-control scripts
│   │   ├── Settings/         # Project and rendering settings
│   │   ├── Sounds/           # Audio assets
│   │   ├── Sprites/          # 2D artwork
│   │   └── Videos/           # Video assets
│   ├── Packages/             # Unity package configuration
│   ├── ProjectSettings/      # Unity project settings
│   ├── Build 1/              # Development build files
│   ├── Final Windows Build/  # Final Windows build files
│   └── GameJam.slnx          # IDE solution file
└── README.md
```

## Play the game

A Windows build is included in the `GameJam/Final Windows Build/` directory. To play it:

1. Open the `GameJam/Final Windows Build/` folder.
2. Run the included game executable.
3. Keep the executable in the same folder as its accompanying data files.

If you prefer to use the earlier build, check the `GameJam/Build 1/` directory.

> GitHub may not display or run binary build files directly in the browser. Download the repository or the build folder before launching the game.

## Open the project in Unity

To inspect or continue development:

1. Install **Unity 6000.4.7f1** through Unity Hub.
2. Clone or download this repository.
3. In Unity Hub, select **Add** or **Open**.
4. Choose the `GameJam/GameJam/` directory as the Unity project folder.
5. Open the project and allow Unity to import the assets.
6. Start from `Assets/Scenes/MainMenu.unity` or open another scene from the `Assets/Scenes/` directory.

### Suggested scenes

- `MainMenu.unity` — Main menu
- `OpeningMovie.unity` — Opening sequence
- `MainScene.unity` — Main gameplay scene
- `Level-1.unity` — Level 1
- `Level2.unity` — Level 2
- `Level3.unity` — Level 3
- `Level 4.unity` — Level 4
- `Congrats.unity` — Completion scene

## Development notes

The project was created under game-jam time constraints. Some folders contain generated Unity and IDE files because this repository is intended to preserve the complete jam project and shared files. When making changes, edit the source assets under `GameJam/Assets/` and use Unity to create new builds.

## Game Jam result

This project was made for the **GMTK Summer Game Jam 2026** and achieved a placement in the **top 18% of submitted entries**.

## Credits

This repository was created and shared by the `adj0131` team for the GMTK Summer Game Jam 2026.

## License

No license has been specified for this repository. Unless a license is added, all rights are reserved by the copyright holders.
