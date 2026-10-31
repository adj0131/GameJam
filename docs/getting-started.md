# Getting started

## Requirements

Install Unity Hub and Unity Editor **6000.4.7f1**. The exact editor revision is recorded in [`ProjectVersion.txt`](../GameJam/ProjectSettings/ProjectVersion.txt). Use that version when possible to avoid unrelated asset and settings migrations.

Install the Unity build support module for the platform you intend to export to. The repository includes Windows build profiles; other platforms need their own configuration and validation.

## Download and open

```sh
git clone https://github.com/adj0131/GameJam.git
cd GameJam
```

In Unity Hub, add the inner `GameJam/` directory. The selected directory must contain `Assets`, `Packages`, and `ProjectSettings`. Selecting the repository root will not select the Unity project.

Wait for package resolution and asset import to finish. Unity reads dependencies from [`Packages/manifest.json`](../GameJam/Packages/manifest.json) and the corresponding lockfile. No separate npm or Python installation step is required for the game.

Open `Assets/Scenes/MainMenu.unity`, enter Play mode, and use the menu to start. Click inside the Game view before trying the controls. Exit Play mode before editing values you want to save.

## Controls

These bindings come from [`InputSystem_Actions.inputactions`](../GameJam/Assets/InputSystem/InputSystem_Actions.inputactions); the implemented player callbacks are in `PlayerMovement.cs`.

| Action | Keyboard / mouse | Gamepad |
| --- | --- | --- |
| Move horizontally | A / D or left / right arrows | Left stick |
| Jump | Space | South face button |
| Shoot | Left mouse button or Enter | West face button |
| Parry | Right mouse button | No binding configured |

The action asset also contains actions such as Sprint and Interact. Their presence does not mean the player controller implements those abilities. Movement direction determines which way the player faces and fires. Shooting and parrying temporarily stop horizontal movement; some actions are blocked while another animation or upgrade sequence is active.

## Build the game

1. Open the project using the editor version above.
2. Open **File > Build Profiles**.
3. Select a Windows profile from `Assets/Settings/Build Profiles/`, or configure a profile for your target platform.
4. Check the scene list against [`EditorBuildSettings.asset`](../GameJam/ProjectSettings/EditorBuildSettings.asset). Scene order affects progression and upgrades.
5. Choose an output directory outside the tracked project content and build. Use Build and Run if you also want to launch the exported game.
6. Verify the menu, controls, scene transitions, audio, and videos in the exported build.

There is no checked-in command-line build method or CI build workflow. Build through the editor until a supported automation workflow is added.

## Troubleshooting

| Symptom | What to check |
| --- | --- |
| Hub does not recognize the project | Select the inner `GameJam/` directory, which contains `Assets`, `Packages`, and `ProjectSettings`. |
| Package import fails | Confirm the editor version, network access, and the first package error in the Console. Avoid changing dependency versions as a first step. |
| Controls appear unresponsive | Focus the Game view, confirm Play mode, and check for Console errors. Input is intentionally disabled during celebration and death sequences. |
| Scene transition fails | Check enabled scenes and their order in Build Profiles. Look for a missing scene or missing `SceneControl` reference. |
| A gameplay scene opened directly behaves differently | Start from `MainMenu`; scene control persists across transitions, and upgrades depend on the build index. |
| Level does not end | Check the scene's `Enemies` object. The player checks its child count before starting the celebration sequence. |
| Ice upgrade has no visible effect | Check for an `IcedEffect` component and its SpriteRenderer. The current implementation applies a blue tint; it does not implement enemy slowing. |
| Quit button does not close the editor | `Application.Quit()` is intended for a standalone player. Test quitting in an exported build. |

Record the editor version, scene name, reproduction steps, and the first Console error when reporting a problem.
