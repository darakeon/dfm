@echo off

set push=%~1

single-build.bat netcore %push%
single-build.bat netcore-libman %push%
single-build.bat netcore-libman-node %push%
single-build.bat netcore-libman-node-chrome %push%

single-build.bat browser-tests %push%

single-build.bat android %push%
