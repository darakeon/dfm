@echo off

if "%1" == "-f" (
	SET force=-f
)
if "%2" == "-f" (
	SET force=-f
)

if "%1" == "push" (
	SET push=push
)
if "%2" == "push" (
	SET push=push
)

call build-all.bat %push% > NUL
call run.bat %force%
