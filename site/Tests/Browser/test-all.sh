clear

pkill -e DFM.MVC || echo "Site not running yet"
rm -rf server

cd ../../MVC
libman restore
dotnet publish -c Release -o ../Tests/Browser/server

cd ../Tests/Browser/server
mkdir data
mkdir data/log_files

cd ../../../../

.circleci/browser/run-tests.sh
