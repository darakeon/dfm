@echo off

cd %windir%\system32\inetsrv\

appcmd stop apppool "DfM Test"
appcmd stop sites "DfM Test"
appcmd start apppool "DfM Test"
appcmd start sites "DfM Test"

cd %~dp0

cd ..\..\MVC

msbuild MVC.csproj /p:DeployOnBuild=True /p:PublishProfile=browser /p:Configuration=BrowserTests -verbosity:quiet

cd ..\Tests\Browser

REM Run sequentially because of page sessions
REM There are tests that login and logoff
npm test -- --runInBand

pause
