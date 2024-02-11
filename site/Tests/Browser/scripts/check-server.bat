@echo off

echo %1

curl -s http://localhost:2703 > NUL
if errorlevel 1 (
	if "%1" == ".........." (
		echo Cannot find server
		pause
	) else (
		call %~dp0\check-server %1.
	)
) else (
	echo Woke up!
)
