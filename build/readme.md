# (Mostly) automated release via GitHub actions
_since v39, 2026-08_

It's way overdue for this repo to get some kind of CI for even more transparency: anybody can check (for the first 90 days GitHub retains the logs) that the binaries the worker built from the published sources are attached to a release, instead of me building it locally and uploading the archive.

It also allows me to automate even more work than the old `pack_release.sh` script did for v30–38. The only manual parts in this workflow are stating the pipeline and completing the release notes. 

# The workflow
See [the YAML](../.github/workflows/release.yml) for the defined steps. Some context on why I did something may be present in the [my test repo](https://github.com/bnfour/workflow-testing) I used to get myself familiar with GitHub's flavor of CI/CD.

I tried to make steps as atomic as possible, so there are quite a bit of them. They also run commands verbosely where possible so that the logs could be easily inspected.

# This folder contents
This file, .NET solution filters, and helper bash scripts.

## Filters
This subfolder contains [solution filters](https://learn.microsoft.com/en-us/visualstudio/msbuild/solution-filters?view=visualstudio) used to filter out some projects from the builds to not waste resources; filters are named after the config they're used with.

- [`Experimental`](../Experimental/) is never build via CI — its sole purpose is to be there when I need to drop a test patch (probably named `XddPatch`) somewhere to test something without messing up any other existing mods.
- [`Tests`](../Tests/) are only build in `Debug` to run the tests, not in `Release`.
- Rest of the projects are regular mods and are build in both configs: `Debug` first to ensure they do build, then `Release` to produce the actual binaries to publish.

>[!TIP]
>Don't forget to update the filters when adding a new mod to the solution.

Waiting for [.NET 11 SDK](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview3/sdk.md#solution-filters-can-now-be-edited-from-the-cli) to instead create these programmatically in a better way — by removing one or two projects instead of listing all others to be included.

Solution filters are, by Microsoft customs, JSON with comments. The only purpose of `.vscode` folder is to make the editor aware of that. 
<!-- inb4 i'll forget to update this section when i'll add more stuff there -->

## Scripts

While the steps are supposed to be atomic, I don't really like writing bash in YAML without my beloved [Shellcheck](https://www.shellcheck.net/) or even syntax highlighting, so basically:

if it's more than one command, it's in a script file.

# The other folder
The scripts assume all build is run directly in the repo's root folder, the environment is considered disposable. The created in the process are gitignored for local troubleshooting.

`release` folder in repo's root is the place where the assets go. It is then zip-compressed and attached to the release. The structure remains the same.
