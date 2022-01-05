#!/bin/bash
set -e

MACHINE=$1
PUSH=$2

echo "Building $MACHINE..."
docker build .. --pull --progress plain -t darakeon/dfm-$MACHINE -f "$PWD/dfm-$MACHINE.dockerfile"
echo "$MACHINE built!"

if [ "$PUSH" = "push" ]; then
	echo "Pushing $MACHINE..."
	docker push darakeon/dfm-$MACHINE
	echo "$MACHINE pushed!"
fi
