#!/usr/bin/env bash

# given the state of the repo (tags), figures out the version number for the new release,
# also defines the final asset archive name base on that

last_version=$(git tag --list 'v*' | colrm 1 1 | sort --numeric-sort --reverse | head --lines=1)
# there is a lot of tags in that format, no fallback for v1 necessary,
# neither is printf for 0-padding versions < 10
new_version=$(( "$last_version" + 1 ))
# to root of the repo you go
filename="md-mods-$new_version.zip"

echo "new_version=$new_version" >> "$GITHUB_ENV"
echo "filename=$filename" >> "$GITHUB_ENV"

echo "This is release version $new_version, apparently"
echo "Will pack into $filename"
