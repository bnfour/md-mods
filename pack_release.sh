#!/usr/bin/env bash

# a script to automate the creation of a new release (it was done manually before v30)
# if you're not me, there's probably no reason to run this -- it's in the repo for transparency

# puts the archive to upload into /tmp, also provides the list of checksums to paste
# requirements: dotnet (obviously), jq

echo "bnfour's md-mods OMEGA release script, reporting for duty (• - •)ゝ"

CONFIG="Release"
FRAMEWORK="net6.0"
# bash btw; thankfully, proton exists
RUNTIME="win-x64"

# internal projects that have nothing to do with a release
# (easier to keep a list to exclude)
DO_NOT_SHIP=("Experimental" "Tests")

BUILDROOT=$(mktemp --tmpdir --directory md-mods-release.XXXXXXXXXX)
echo "Working directory: $BUILDROOT"

LOGS="$BUILDROOT/logs"
ARCHIVE_ROOT="$BUILDROOT/pub"

mkdir "$LOGS" "$ARCHIVE_ROOT"

MODS="$ARCHIVE_ROOT/Mods"
USERLIBS="$ARCHIVE_ROOT/UserLibs"

mkdir "$MODS" "$USERLIBS"

#region log files auto-numbering

# file to store current counter value
LOG_COUNTER="$BUILDROOT/log-counter"
echo 0 > "$LOG_COUNTER"

# increments the counter, writes the new value back to the file;
# "returns" the new value formatted with leading zero, ready to be used
log_number () {
    local i

    i=$(< "$LOG_COUNTER")
    ((i++))
    echo $i > "$LOG_COUNTER"

    printf "%02d" $i
}

#endregion

# copy text files
cp LICENSE "$ARCHIVE_ROOT/"
cp release-bundle/*.txt "$ARCHIVE_ROOT/"

# build and copy mod DLLs
echo "Building..."
dotnet build --configuration "$CONFIG" \
    &>"$LOGS/$(log_number)-dotnet-build.log" || { echo "Build error, check the log in $LOGS"; exit 1; }

# figure out the project names,
# the sed magical invocation is basically "print stuff before the slash from lines matching something/something.csproj"
projects=$(dotnet sln list | sed -nE 's/^(.*)\/.*\.csproj/\1/p')
for project in $projects
do
    if echo "${DO_NOT_SHIP[*]}" | grep -qw "$project";
    then
        echo "Not packing $project (configured skip)"
    else
        echo "Packing $project"
        cp "$project/bin/$CONFIG/$FRAMEWORK/$project.dll" "$MODS/"
    fi
done

# used to copy non-mod DLLs only to userlibs, disabled immediately after
shopt -s extglob

# get (relative path to) projects with nuget references
projects_to_publish=$(dotnet package list --framework "$FRAMEWORK" --no-restore --format json\
    | jq -r '.projects.[] | select(.frameworks[0].topLevelPackages != null) | .path'\
    | xargs realpath --relative-to=".")
# --framework option should exclude Tests by itself (it uses newer version), and Experimental changes are never merged
# still better safe than sorry
for project in $projects_to_publish
do
    # same sed magic as above
    short_name=$(echo "$project" | sed -nE 's/^(.*)\/.*\.csproj/\1/p')
    if echo "${DO_NOT_SHIP[*]}" | grep -qw "$short_name";
    then
        echo "Not publishing $short_name (this message should not appear)"
    else
        echo "Publishing $short_name for extra DLLs..."
        # this builds it again, --no-build did not work
        # hopefully msbuild is still smart enough to reuse the DLLs we just built
        dotnet publish --configuration "$CONFIG" --runtime "$RUNTIME" --no-self-contained "$project" \
            &>"$LOGS/$(log_number)-dotnet-publish-$short_name.log" || { echo "$short_name publish error, check the log in $LOGS"; exit 1; }
        # copy non-mod DLLs only
        cp "$short_name"/bin/$CONFIG/$FRAMEWORK/$RUNTIME/publish/!("$short_name").dll "$USERLIBS/"
    fi
done

shopt -u extglob

# figure out the version we publishing as latest tag + 1
# i put tags when creating a release on github, so its not present when packing ¯\_(ツ)_/¯
# (note to self: please don't forget to pull tags afterwards)
last_version=$(git tag --list 'v*' | colrm 1 1 | sort --numeric-sort --reverse | head --lines=1)
new_version=$(( "$last_version" + 1 ))
archive_path="$BUILDROOT/md-mods-$new_version.zip"

# cd into the archive root for proper output for last commands (clean relative paths)
# exit is there to keep shellcheck happy
cd "$ARCHIVE_ROOT" || exit 2

echo "Zipping..."
zip -r "$archive_path" ./* &>"$LOGS/$(log_number)-zip.log"

# checksums for DLLs
mods_sums=$(sha256sum Mods/*)
libs_sums=$(sha256sum UserLibs/*)

# end results banner
cat <<EOF

Packing for v$new_version done! d_(>ᵕ<)_b
Archive available at $archive_path

Copypaste:

## Checksums
Don't forget to verify your downloads! SHA256 checksums for DLLs:
\`\`\`
$mods_sums

$libs_sums
\`\`\`

EOF
