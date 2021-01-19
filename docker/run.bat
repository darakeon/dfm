@echo off

set machine=%~1
set name=%~1
set type=%~2

if "%type%" neq "start" (
	set type=rm-and-run
)

REM [android] andy
	if "%machine%" == "andy" (
		set machine=android
	)

	if "%name%" == "android" (
		set name=andy
	)

	if "%name%" == "andy" (
		set parameters=
		set work_dir=
	)

REM [netcore] beedle
	if "%machine%" == "beedle" (
		set machine=netcore
	)

	if "%name%" == "netcore" (
		set name=beedle
	)

	if "%name%" == "beedle" (
		set parameters=
		set work_dir=
	)

REM [browser-tests] yoshi
	if "%machine%" == "yoshi" (
		set machine=browser-tests
	)

	if "%name%" == "browser-tests" (
		set name=yoshi
	)

	if "%name%" == "yoshi" (
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
