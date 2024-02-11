@echo off

cls

SET TESTS_PATH=%~dp0..\
cd %TESTS_PATH%

IF EXIST node_modules_windows (
	MOVE node_modules node_modules_linux
	MOVE node_modules_windows node_modules
)

IF NOT EXIST node_modules (
	call npm install > %TESTS_PATH%\log\node.log
)

cd ..\..\MVC
libman restore > %TESTS_PATH%\log\libman.log
dotnet.exe publish -c Release -o ..\Tests\Browser\server > %TESTS_PATH%\log\dotnet.log
cd ..\Tests\Browser

SET ASPNETCORE_ENVIRONMENT=CircleCI
SET ASPNETCORE_URLS=http://+:2703

cd server
del tests.db 2> NUL
taskkill /IM "DFM.MVC.exe" 2> NUL
start DFM.MVC.exe
cd ..

call %TESTS_PATH%\scripts\check-server .

call node contract.js

REM Run sequentially because of page sessions
REM There are tests that login and logoff
call npm test -- %1

taskkill /IM "DFM.MVC.exe"
timeout 2
rmdir server /s /q
