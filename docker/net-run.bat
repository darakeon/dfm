docker rm yoshi
docker run --name yoshi -it -v %~dp0..\:/var/dfm -w /var/dfm/site/Tests/Browser --expose 2709 -P darakeon/browser-tests
