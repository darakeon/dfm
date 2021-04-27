@echo off

set dir=%~dp0
set push=%~1

call single-build.bat site %push%
call single-build.bat robot %push%
