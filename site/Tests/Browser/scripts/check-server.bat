@echo off

echo %1

curl -s http://localhost:2709 > NUL
if errorlevel 1 (
	if "%1" == ".........." (
		exit 1
	) else (
		call %~dp0\check-server %1.
	)
) else (
	echo "Woke up!"
)
