@echo off

REM Run sequentially because of page sessions
REM There are tests that login and logoff
npm test -- --runInBand

pause
