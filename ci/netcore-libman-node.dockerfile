FROM darakeon/netcore-libman
MAINTAINER Dara Keon
RUN apt upgrade -y && apt update && apt autoremove -y

RUN curl -sL https://deb.nodesource.com/setup_12.x | bash -
RUN apt-get install -y nodejs
