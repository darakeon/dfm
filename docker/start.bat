if "%~2" == "start" (
	docker start -i beedle
) else (
	docker rm beedle
	docker run --name beedle -it darakeon/netcore
)

if "%~1" == "browser-tests" || "%~1" == "yoshi" (
	docker rm yoshi
	docker run --name yoshi  -it -v %~dp0..\:/var/dfm -w /var/dfm/site/Tests/Browser --expose 2709 -P darakeon/browser-tests
)
