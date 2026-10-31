# Contributing

Thanks for helping improve Welcome to the Junkyard. Start with the [setup guide](docs/getting-started.md) and [project guide](docs/project-guide.md).

## Before making a change

Check existing issues and pull requests for related work. For a large gameplay or dependency change, describe the proposed behavior in an issue so maintainers can discuss scope.

Use Unity Editor **6000.4.7f1**, as recorded in `GameJam/ProjectSettings/ProjectVersion.txt`. This repository has no root license file; do not assume permission to redistribute the project or third-party assets. Ask maintainers about licensing when needed.

## Make a focused branch

If you do not have repository write access, fork the repository first and clone your fork. From your local clone:

```sh
git switch main
git pull --ff-only
git switch -c docs/improve-setup
```

Choose a branch name that describes your change. Keep documentation, gameplay changes, and dependency upgrades in separate pull requests when they can be reviewed independently.

## Files to include

Commit intentional source changes, including Unity `.meta` files for added or moved assets. Inspect `git status` before committing. Avoid generated folders such as `Library`, `Temp`, `Logs`, and `Builds`, exported game binaries, build archives, IDE settings, and operating-system files. The root `.gitignore` lists excluded artifacts.

Preserve scene names, build order, object references, tags, and layers unless the change requires updating them. Scene progression and weapon upgrades depend on the build index; see the [project guide](docs/project-guide.md#scene-progression).

## Validate your change

For documentation changes, verify paths, links, commands, and descriptions against the repository. Clearly distinguish observed behavior from proposed features. Run `git diff --check` to catch whitespace problems.

For gameplay or asset changes, use this manual checklist as appropriate:

- [ ] Open the project with the recorded Unity version and let import finish.
- [ ] Check the Console for compilation errors before entering Play mode.
- [ ] Start from `MainMenu` and verify the opening video reaches gameplay.
- [ ] Check horizontal movement, facing, jumping, shooting, and parrying.
- [ ] Check damage, the health bar, death, and return to the menu.
- [ ] Check affected enemies, spawn triggers, and level-end progression.
- [ ] Check upgrades at the relevant build indices.
- [ ] Check changed audio, animations, or videos.
- [ ] If build behavior changed, export and run the affected platform build.

The Unity Test Framework package is present, but this repository currently has no project test suite or CI build workflow. Do not describe a Play mode check as an automated test. State which checks you performed and which you could not run.

## Open a pull request

Review the diff, commit your changes, and push your branch to your repository:

```sh
git diff --check
git diff
git add README.md docs/getting-started.md
git commit -m "docs: improve setup instructions"
git push -u origin docs/improve-setup
```

The file names above are an example; stage only the files you actually changed. Open a pull request targeting `adj0131/GameJam`'s `main` branch.

Include the problem being addressed, what changed, and how you verified it. Link the relevant issue if one exists. Add screenshots or a short video for visible gameplay or UI changes, and mention any required scene or Inspector configuration.

## Report a bug

Include a descriptive title, Unity version, operating system, scene or build profile, steps to reproduce, expected behavior, actual behavior, and relevant Console messages. A screenshot or video helps with animation and collision problems. Remove credentials and personal paths from shared logs.
