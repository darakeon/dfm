@echo off
docker container prune %1
docker run -it -v %~dp0..\:/var/dfm -w /var/dfm/site/Tests/Browser --expose 2709 -P darakeon/browser-tests
