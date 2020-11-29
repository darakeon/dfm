docker rm andy
del ..\android\Lib\src\debug\res\xml\network_security_config.xml
del ..\android\Lib\src\debug\res\values\site-address.xml
docker run --name andy -it -v %~dp0..\:/dfm -w /dfm/android circleci/android:api-29
