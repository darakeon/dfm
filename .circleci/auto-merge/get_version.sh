#!/bin/bash

ABS=$(realpath "$0")
cd $(dirname $ABS)
cd ../..

export VERSION=$(head -n 8 docs/RELEASES.md | tail -n 1 | cut -d "#" -f 2 | cut -d ")" -f 1)

if [ "$(git branch -al *$$VERSION)" == "" ]; then
    export VERSION=$(head -n 7 docs/RELEASES.md | tail -n 1 | cut -d "#" -f 2 | cut -d ")" -f 1);
fi

echo $VERSION
