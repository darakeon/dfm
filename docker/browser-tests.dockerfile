FROM darakeon/netcore-libman-node-chrome
MAINTAINER Dara Keon
ENV ASPNETCORE_ENVIRONMENT=circleCI
ENV MSBUILDSINGLELOADCONTEXT=1
RUN echo 'export PS1="\n\n[\A] \u@\W\$ "' >> ~/.bashrc
