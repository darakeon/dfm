@echo off

set name=%~1
set type=%~2

if "%type%" neq "rebuild" (
	set type=start
)

REM [android] andy
	if "%name%" == "andy" (
		del ..\android\Lib\src\debug\res\xml\network_security_config.xml
		del ..\android\Lib\src\debug\res\values\site-address.xml

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

REM [rust] doki
	if "%name%" == "doki" (
		set machine=rust
		set parameters=
		set work_dir=version
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

docker-encapsulated.bat %type% %name% %machine% "%work_dir%" "%parameters%"
