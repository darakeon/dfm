@echo off

set dir=%~dp0
set machine=%~1
set push=%~2

docker build .. -t darakeon/dfm-%machine% -f "%dir%dfm-%machine%.dockerfile"

if "%push%" neq "" (
	docker %push% darakeon/dfm-%machine%
)
