# (Mostly) automated release via GitHub actions

TODO brief description, including why
- fail fast
- small steps
- external deps

# This folder contents

## Filters
`filters` subfolder contains [solution filters](https://learn.microsoft.com/en-us/visualstudio/msbuild/solution-filters?view=visualstudio) used to filter out some projects from the builds to not waste resources; filters are named after the config they're used with.

- [`Experimental`](../Experimental/) is never build via CI — its sole purpose is to be there when I need to drop a test patch (probably named `XddPatch`) somewhere to test something without messing up any other existing mods.
- [`Tests`](../Tests/) are only build in `Debug` to run the tests, not in `Release`.
- Rest of the projects are regular mods and are build in both configs: `Debug` first to ensure they do build, then `Release` to produce the actual binaries to publish.

>[!TIP]
>Don't forget to update the filters when adding a new mod to the solution.

Waiting for [.NET 11 SDK](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview3/sdk.md#solution-filters-can-now-be-edited-from-the-cli) to instead create these programmatically in a better way — by removing one or two projects instead of listing all others to be included.

## Scripts

if it's more than one command, it's a script
small steps if possible
show results in logs
