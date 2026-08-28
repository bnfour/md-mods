#!/usr/bin/env bash

# creates asset archive with proper relative paths
# (archive root = build/ folder root)

cd build || exit 2;
# shellcheck disable=SC2154 # it's set in get-version earlier in the workflow
zip -r "../$filename" ./*
