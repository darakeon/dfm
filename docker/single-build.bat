@echo off

set machine=%~1
set push=%~2

docker context use default
docker build . -f %machine%.dockerfile -t darakeon/%machine% --network=host
docker image prune -f

if "%push%" == "push" (
	docker push darakeon/%machine%
)
