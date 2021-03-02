FROM darakeon/netcore-libman
MAINTAINER Dara Keon
RUN apt update

RUN curl -sL https://deb.nodesource.com/setup_12.x | bash -
RUN apt-get install -y nodejs
