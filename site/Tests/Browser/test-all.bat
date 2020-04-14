@echo off

taskkill /IM "DFM.MVC.exe"

dotnet.exe publish -c Release ..\..\MVC\MVC.csproj -o site

SET ASPNETCORE_ENVIRONMENT=CircleCI

cd site
start DFM.MVC.exe p2709
cd ..
timeout 10

call node contract.js

REM Run sequentially because of page sessions
REM There are tests that login and logoff
call npm test -- --runInBand

taskkill /IM "DFM.MVC.exe"
timeout 2
rmdir site /s /q
