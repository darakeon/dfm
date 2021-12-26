# Running tests

## Creating new machine

```
docker run -it --name bt -v %cd%:/var/dfm -p 2709:2709 -w /var/dfm darakeon/dfm-browser-tests
```

## Starting created machine

```
docker start -i bt
```

## First time inside machine

```
cd /var/dfm
dotnet restore site/Site.sln
cd site/MVC
libman restore
dotnet publish -c Release MVC.csproj -o ../Tests/Browser/server --no-restore
cd ../Tests/Browser
npm install
cd server
./DFM.MVC > ../log/server.log & disown
cd ..
node contract.js 2> /dev/null
```

## Other times inside machine

```
cd server
rm tests.db
pkill DFM.MVC
./DFM.MVC > ../log/server.log & disown
cd ..
sleep 2
node contract.js 2> /dev/null
```
