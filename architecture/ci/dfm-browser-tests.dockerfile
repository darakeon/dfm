FROM darakeon/netcore-libman-node-chrome
LABEL maintainer="Dara Keon <laboon@darakeon.com>"

ENV ASPNETCORE_ENVIRONMENT=circleCI
ENV MSBUILDSINGLELOADCONTEXT=1
ENV ASPNETCORE_URLS=http://+:2703
