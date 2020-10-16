clear

if [ -d "node_modules_linux" ]; then
	mv node_modules node_modules_windows
	mv node_modules_linux node_modules
fi

npm install

pkill -e DFM.MVC || echo "Site not running yet"
rm -rf server

cd ../../MVC
libman restore
dotnet publish -c Release -o ../Tests/Browser/server

cd ../Tests/Browser/server
mkdir data
mkdir data/log_files

cd ../../../../

.circleci/browser/run-tests.sh $1
