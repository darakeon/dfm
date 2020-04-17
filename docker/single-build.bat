@echo off

docker build . -f %1.dockerfile -t darakeon/%1

if "%2" == "push" (
	docker push darakeon/%1
)
