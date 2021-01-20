@echo off

set name=%~1
set type=%~2

if "%type%" neq "start" (
	set type=rm-and-run
)

REM [android] andy
	if "%name%" == "andy" (
		set machine=android
		set parameters=
		set work_dir=
	)

REM [netcore] beedle
	if "%name%" == "beedle" (
		set machine=netcore
		set parameters=
		set work_dir=
	)

REM [browser-tests] yoshi
	if "%name%" == "yoshi" (
		set machine=browser-tests
		set parameters=--expose 2709 -P
		set work_dir=site/Tests/Browser
	)

echo .
echo ..
echo ...
echo ====================================================================================================
echo [%type%] %name% (%machine%)
if "%parameters%" neq "" (
	echo %parameters%
)
echo ====================================================================================================
echo ...
echo ..
echo .

start-or-rm-and-run.bat %type% %name% %machine% "%work_dir%" "%parameters%"
