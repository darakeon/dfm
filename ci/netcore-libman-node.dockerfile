FROM darakeon/netcore-libman
MAINTAINER Dara Keon
RUN maintain

RUN curl -sL https://deb.nodesource.com/setup_12.x | bash -
RUN apt install -y nodejs
