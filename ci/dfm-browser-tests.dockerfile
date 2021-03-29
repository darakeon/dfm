FROM darakeon/netcore-libman-node-chrome
MAINTAINER Dara Keon <laboon@darakeon.com>
RUN maintain

ENV ASPNETCORE_ENVIRONMENT=circleCI
ENV MSBUILDSINGLELOADCONTEXT=1
ENV ASPNETCORE_URLS=http://+:2709
