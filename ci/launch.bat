@echo off

set name=%~1
set type=%~2

if "%type%" neq "rebuild" (
	set type=start
)

REM [android] fi
	if "%name%" == "fi" (
		del ..\android\Lib\src\debug\res\xml\network_security_config.xml
		del ..\android\Lib\src\debug\res\values\site-address.xml

		set machine=android
		set parameters=
		set work_dir=android
	)

REM [netcore] beedle
	if "%name%" == "beedle" (
		set machine=netcore
		set parameters=
		set work_dir=
	)

REM [browser-tests] yoshi
	if "%name%" == "yoshi" (
		set machine=dfm-browser-tests
		set parameters=--expose 2709 -P
		set work_dir=
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

call docker-encapsulated.bat %type% %name% %machine% "%work_dir%" "%parameters%"
