docker rm andy
docker run --name andy -it -v %~dp0..\:/dfm -w /dfm/android circleci/android:api-29
