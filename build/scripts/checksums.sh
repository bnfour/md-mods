#!/usr/bin/env bash

# shows sha256 sums for dlls in both mods and userlibs folders,
# stores them into a file to be listed in the release notes draft

cd release || exit 2;

mods=$(sha256sum Mods/*)
libs=$(sha256sum UserLibs/*)

# don't forget to go back to the root
cd ..

# just into the root folder
cat > checksums <<EOF
$mods

$libs
EOF

# show in the log as well
cat checksums
