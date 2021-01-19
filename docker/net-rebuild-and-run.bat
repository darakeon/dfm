@echo off

if "%~1" == "push" (
	SET push=push
)

call net-build-all.bat %push% > NUL
call run.bat browser-tests
