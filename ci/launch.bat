@echo off

set name=%~1
set type=%~2

if "%type%" neq "rebuild" (
	set type=start
)

REM [android] fi
	if "%name%" == "fi" (
		cd ..\android
		del /Q /S .gradle > NUL
		del /Q /S .idea > NUL
		del /Q /S App\build > NUL
		del /Q /S App\log > NUL
		del /Q /S build > NUL
		del /Q /S ErrorLogs\build > NUL
		del /Q /S Lib\build > NUL
		del /Q /S Lib\log > NUL
		del /Q /S Lib\src\debug\res\xml\network_security_config.xml > NUL 2> NUL
		del /Q /S Lib\src\debug\res\values\site-address.xml > NUL 2> NUL
		del /Q /S TestUtils\build > NUL
		cd ..\ci

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
