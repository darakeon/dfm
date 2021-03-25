@echo off

set push=%~1

call single-build.bat netcore-libman %push%
call single-build.bat netcore-libman-node %push%
call single-build.bat netcore-libman-node-chrome %push%

call single-build.bat dfm-browser-tests %push%

call single-build.bat android %push%
