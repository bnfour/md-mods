#!/usr/bin/env bash

# copies okay to ship dependency DLLs to be included in the release

# TODO combines publish and copying

# used to copy non-mod DLLs only to userlibs, disabled immediately after
shopt -s extglob

# get (relative path to) projects with nuget references, via filter
projects_to_publish=$(dotnet package list --project build/filters/release.slnf --no-restore --format json\
    | jq -r '.projects.[] | select(.frameworks[0].topLevelPackages != null) | .path'\
    | xargs realpath --relative-to=".")
for project in $projects_to_publish
do
    short_name=$(echo "$project" | sed -nE 's/^(.*)\/.*\.csproj/\1/p')
    echo "Publishing $short_name for extra DLLs..."
    dotnet publish --configuration Release --no-build --no-self-contained "$project"
    # copy non-mod DLLs only
    pub_root="$short_name/bin/Release/net6.0/publish"
    # aaaaa these quotes
    cp --verbose "$pub_root"/!("$short_name").dll build/UserLibs/
    # also check for native libraries for win-x64 (also used with proton) (skia)
    native_root="$pub_root/runtimes/win-x64/native"
    if [ -d "$native_root" ]
    then
        cp --verbose "$native_root/*.dll" build/UserLibs/
    fi
done

shopt -u extglob
