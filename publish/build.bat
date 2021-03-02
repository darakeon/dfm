@echo off

set dir=%~dp0
set push=%~1

docker context use default > NUL

docker stop dfm > NUL 2> NUL
docker rm dfm > NUL 2> NUL

docker build .. -t darakeon/dfm -f "%dir%dfm.dockerfile"

if "%push%" neq "" (
	docker %push% darakeon/dfm
)
