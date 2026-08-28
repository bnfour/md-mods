#!/usr/bin/env bash

# creates asset archive with proper relative paths
# (archive root = release/ folder root)

cd release || exit 2;
# shellcheck disable=SC2154 # it's set in get-version earlier in the workflow
zip -r "../$filename" ./*
