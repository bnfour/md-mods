#!/usr/bin/env bash

# this script creates an archive of stripped* assemblies required to build&test
# to provide to action runners via an undisclosed url to keep the (public) build
# process as free from proprietary dlls as possible, while still building

# this makes a list of all dlls from references folder referenced by any project,
# and makes an archive of those only, but stripped* of any actual code,
# only keeping signatures to build against, so even if it somehow leaks, it's nothing

# *MelonLoader assemblies are not stripped because some tests rely use them
# it's free software anyway, shouldn't be a problem

# requirements (pretty much tied to my setup):
# - ripgrep
# - fd-find
# - BepInEx.AssemblyPublicizer.Cli as a global dotnet tool

# please please run the script from the repo's root
# alternatively, just build with the dlls provided by MelonLoader installation,
# i'm just trying to be as clean as possible with the stuff I'm moving to the
# runners over internets

separator () {
    echo "==========================="
    echo "$1"
    echo "==========================="
}

echo "References zip builder script! Did you run it from repo's root?"

ROOT=$(mktemp -d)

# captures "references/*.dll" part, relative to repo's root
referenced=$(rg -o --no-filename --no-line-number --no-heading "\.\./(references/.*\.dll)" -r '$1' | sort -u)

separator "Copying..."
# shellcheck disable=SC2086 # the splitting is intentional
cp --verbose $referenced "$ROOT"

cd "$ROOT" || exit 2;

separator "Stripping..."
# strip all non-MelonLoader assemblies; do not publicize anything though
fd -t f --glob "*.dll" -E "*MelonLoader*" | xargs assembly-publicizer --strip-only --overwrite

# zip it up, removing any previous artifacts on the hardcoded path
# (it's a fire-and-forget script that is not supposed to be run often anyway)

ARCHIVE="/tmp/md.zip"
if [ -f "$ARCHIVE" ]
then
    rm $ARCHIVE
fi

separator "Zipping..."
zip $ARCHIVE ./*.dll

separator "Done, upload $ARCHIVE"
