#!/usr/bin/env bash

# copies build mod DLLs to be included in the release

# TODO waiting patiently for .NET 11 to dotnet sln *.slnf list to just work

DO_NOT_SHIP=("Experimental" "Tests")
projects=$(dotnet sln Bnfour.MuseDashMods.slnx list | sed -nE 's/^(.*)\/.*\.csproj/\1/p')
for project in $projects
do
    if echo "${DO_NOT_SHIP[*]}" | grep -qw "$project";
    then
        echo "Not packing $project (configured skip)"
    else
        echo "Packing $project"
        cp --verbose "$project/bin/Release/net6.0/$project.dll" build/Mods/
    fi
done
