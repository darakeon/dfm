@echo off

set type=%~1
set name=%~2
set machine=%~3
set work_dir=%~4
set parameters=%~5

if "%type%" == "start" (
 	docker start -i %name%
)

if "%type%" == "rebuild" (
	call single-build.bat %machine%
 	docker rm %name% > NUL 2> NUL
 	docker run --name %name% -it -v %~dp0..\:/var/dfm -w /var/dfm/%work_dir% %parameters% darakeon/%machine%
)
