docker rm andy
docker run --name andy -i -t -v %~dp0..\:/dfm -w /dfm/android circleci/android:api-29
