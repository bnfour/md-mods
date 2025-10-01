#!/usr/bin/env bash

# a script to automate the creation of a new release (it was done manually all this time)
# if you're not me, there's probably no reason to run this -- it's in the repo for transparency

# puts the archive to upload into /tmp, also provides the list of checksums to paste

CONFIG="Release"
FRAMEWORK="net6.0"
# bash btw; thankfully, proton exists
RUNTIME="win-x64"

BUILDROOT=$(mktemp --directory)
MODS="$BUILDROOT/Mods"
USERLIBS="$BUILDROOT/UserLibs"

mkdir "$MODS" "$USERLIBS"

# copy text files
cp LICENSE "$BUILDROOT/"
cp release-bundle/* "$BUILDROOT/"

# build and copy mod DLLs
dotnet build --configuration "$CONFIG" Bnfour.MuseDashMods.sln

# figure out the project names,
# the sed magical invocation is basically "print stuff before the slash from lines matching something/something.csproj"
projects=$(dotnet sln list | sed -nE 's/^(.*)\/.*\.csproj/\1/p')
# copy dlls for everything except Experimental and Tests
# (easier to keep a list to exclude)
for project in $projects
do
    if [[ "$project" == "Experimental" || "$project" == "Tests" ]]
    then
        echo "Not packing $project (manually configured skip)"
    else
        cp "$project/bin/$CONFIG/$FRAMEWORK/$project.dll" "$MODS/"
    fi
done

# we also need to pack SkiaSharp binaries for Scoreboard characters
# this builds it again, --no-build did not work
# hopefully msbuild is still smart enough to reuse the DLL we just built
dotnet publish --configuration "$CONFIG" --runtime "$RUNTIME" --no-self-contained ScoreboardCharacters/ScoreboardCharacters.csproj

cp ScoreboardCharacters/bin/$CONFIG/$FRAMEWORK/$RUNTIME/publish/*SkiaSharp.dll "$USERLIBS/"

# figure out the version we publishing as latest tag + 1
# i put tags when creating a release on github, so its not present when packing ¯\_(ツ)_/¯
# (note to self: please don't forget to pull tags afterwards)
last_version=$(git tag --list 'v*' | colrm 1 1 | sort --numeric-sort --reverse | head --lines=1)
new_version=$(( "$last_version" + 1 ))
archive_path="/tmp/md-mods-${new_version}.zip"
# remove archive if present from previous builds
rm "$archive_path"

# cd into the buildroot for proper output of the last commands (clean relative paths)
cd "$BUILDROOT" || exit 1

zip -r $archive_path ./*

# checksums for dlls
mods_sums=$(sha256sum Mods/*)
libs_sums=$(sha256sum UserLibs/*)

# end results banner
cat <<EOF
================================================================================
Packing for v$new_version done (hopefully)! (°_°)_b
================================================================================
Archive available at $archive_path
================================================================================
Checksums:
\`\`\`
$mods_sums

$libs_sums
\`\`\`
================================================================================
EOF
