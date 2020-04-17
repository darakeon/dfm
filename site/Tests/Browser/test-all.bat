@echo off

taskkill /IM "DFM.MVC.exe"

cd ..\..\MVC
libman restore
dotnet.exe publish -c Release -o ..\Tests\Browser\server
cd ..\Tests\Browser

SET ASPNETCORE_ENVIRONMENT=CircleCI

cd server
start DFM.MVC.exe p2709
cd ..
timeout 10

call node contract.js

REM Run sequentially because of page sessions
REM There are tests that login and logoff
call npm test -- --runInBand

taskkill /IM "DFM.MVC.exe"
timeout 2
rmdir server /s /q
