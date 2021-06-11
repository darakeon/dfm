@echo off

cls

IF EXIST node_modules_windows (
	MOVE node_modules node_modules_linux
	MOVE node_modules_windows node_modules
)

IF NOT EXIST node_modules (
	call npm install > %~dp0\log\node.log
)

cd %~dp0

cd ..\..\MVC
libman restore > %~dp0\log\libman.log
dotnet.exe publish -c Release -o ..\Tests\Browser\server > %~dp0\log\dotnet.log
cd ..\Tests\Browser

SET ASPNETCORE_ENVIRONMENT=CircleCI
SET ASPNETCORE_URLS=http://+:2709

cd server
del tests.db 2> NUL
taskkill /IM "DFM.MVC.exe" 2> NUL
start DFM.MVC.exe
cd ..
timeout 10
curl -s http://localhost:2709 > %~dp0\log\site.log

call node contract.js

REM Run sequentially because of page sessions
REM There are tests that login and logoff
call npm test -- %1

taskkill /IM "DFM.MVC.exe"
timeout 2
rmdir server /s /q
