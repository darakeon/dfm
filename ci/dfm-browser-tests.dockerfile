FROM darakeon/netcore-libman-node-chrome
MAINTAINER Dara Keon
RUN apt upgrade -y && apt update && apt autoremove -y

ENV ASPNETCORE_ENVIRONMENT=circleCI
ENV MSBUILDSINGLELOADCONTEXT=1
ENV ASPNETCORE_URLS=http://+:2709
