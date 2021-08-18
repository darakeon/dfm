@echo off

set dir=%~dp0
set machine=%~1
set push=%~2

docker build .. --pull -t darakeon/dfm-%machine% -f "%dir%dfm-%machine%.dockerfile"

if "%push%" == "push" (
	docker push darakeon/dfm-%machine%
)
