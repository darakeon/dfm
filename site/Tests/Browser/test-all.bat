@echo off

cd %~dp0

cd ..\..\MVC

msbuild MVC.csproj /p:DeployOnBuild=True /p:PublishProfile="Browser Tests" /p:Configuration=BrowserTests -verbosity:quiet

cd ..\Tests\Browser

REM Run sequentially because of page sessions
REM There are tests that login and logoff
npm test -- --runInBand

pause
