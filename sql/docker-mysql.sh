docker stop dfm-mysql
docker rm dfm-mysql
docker run --name dfm-mysql -e MYSQL_ROOT_PASSWORD=password -v $PWD/data:/var/lib/mysql -p 3306:3306 -d mysql
