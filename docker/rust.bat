docker rm doki
docker run --name doki -it -v %~dp0..\:/dfm -w /dfm/version rust
