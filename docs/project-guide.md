# Project guide

## Repository layout

```text
GameJam/
  Assets/
    Animations/       Animation clips and animator controllers
    InputSystem/      Input actions and rendering-related assets
    Prefabs/          Player, enemies, projectiles, and obstacles
    Scenes/           Menus, levels, movies, and celebrations
    Scripts/          C# gameplay components
    Settings/         URP renderer settings and build profiles
    Sounds/           Music and sound effects
    Sprites/          Player, enemy, environment, and health artwork
    Videos/           Story and upgrade videos
  Packages/           Package manifest and lockfile
  ProjectSettings/    Shared Unity settings and scene order
```

Paths in the rest of this guide are relative to the inner Unity project directory.

## Main gameplay components

| Component | Source under `Assets/Scripts/` | Responsibility |
| --- | --- | --- |
| `PlayerMovement` | `Player Scripts/PlayerMovement.cs` | Movement, facing, jump, shoot and parry callbacks, animation timing, and level-end celebration |
| `PlayerHealth` | `Player Scripts/PlayerHealth.cs` | Health initialization, damage, invincibility checks, and death sequence |
| `HealthBar` | `Player Scripts/HealthBar.cs` | Player health display |
| `PlayerGunUpgrades` | `Player Scripts/PlayerGunUpgrades.cs` | Enables triple shot and ice shot according to scene build index |
| `Bullet` | `Player Scripts/Bullet.cs` | Projectile movement, enemy hit handling, and optional ice tint |
| `SceneControl` | `SceneControl.cs` | Persistent scene transitions, fades, game-over flow, and return to the menu |
| `MovieController` | `MovieController.cs` | Prepares and plays a video, then advances when it ends |
| `EnemySpawnTrigger` | `EnemySpawnTrigger.cs` | Disables configured enemies initially and activates them when the player enters the trigger |
| `Barrier` | `Object Scripts/Barrier.cs` | Applies temporary knockback on player collision |

Enemy behavior lives under `Assets/Scripts/Enemy Scripts/`, including `Firetank/Firetank_Enemy.cs`, `fridge enemy/FridgeEnemy.cs`, `Small Enemy/RoboEnemy.cs`, and `ChainsawEnemy.cs`. Prefabs and scene Inspector values can override component defaults, so inspect the actual scene object before changing gameplay numbers.

## Scene progression

The checked-in global build scene list is:

| Build index | Scene |
| --- | --- |
| 0 | `MainMenu` |
| 1 | `OpeningMovie` |
| 2 | `Level 4` |
| 3 | `FirstCelebration` |
| 4 | `Level2` |
| 5 | `SecondCelebration` |
| 6 | `Level3` |
| 7 | `Congrats` |

`MainScene` and `Level-1` also exist in `Assets/Scenes/`, but are not enabled in this global list. The name `Level 4` does not indicate its position in progression: it is currently the first gameplay scene in the build list.

`SceneControl.LoadNextScene()` advances by build index. `PlayerGunUpgrades.Start()` enables triple shot at index 4 and above, and ice shot at index 6 and above. Reordering scenes therefore changes gameplay behavior. Check for profile-specific scene overrides when changing a build profile.

During gameplay, `PlayerMovement` looks for an object named `Enemies`. When that object's child count reaches zero, it starts a celebration, disables player input, and eventually requests the next scene. Enemy hierarchy membership is therefore part of the level setup.

## Combat and upgrades

- A normal shot spawns one projectile at the player's fire point.
- Triple shot spawns three projectiles using the configured spread angle.
- Ice shot marks projectiles with `isIce`; a hit looks for `IcedEffect` on the enemy or its children and applies a temporary blue tint. Although `Bullet` contains slow-related fields, its current ice-hit code does not slow enemy movement.
- `PlayerHealth.TakeDamage()` ignores damage while `PlayerMovement.isInvincible` is true. Parrying and celebration set this flag.
- On death, player input is disabled before scene control starts the game-over flow and returns to `MainMenu`.

## Editing safely

Keep asset `.meta` files with their assets so Unity can preserve GUID references. Prefer moving assets in the Unity Editor, which moves their metadata too. Check Inspector references after changing prefabs, animations, or scripts.

The project uses the Universal Render Pipeline and Unity Input System. Dependency versions are tracked in `Packages/manifest.json` and `Packages/packages-lock.json`. Keep package changes deliberate and separate from unrelated content changes.

Do not infer runtime behavior from script comments alone. Verify the implementation, serialized scene values, and a Play mode run before documenting or changing a mechanic.
