@echo off

call single-build.bat netcore-libman %1
call single-build.bat netcore-libman-node %1
call single-build.bat netcore-libman-node-chrome %1
call single-build.bat browser-tests %1
