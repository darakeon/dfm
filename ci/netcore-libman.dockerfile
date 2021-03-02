FROM darakeon/netcore
MAINTAINER Dara Keon
RUN apt update

RUN dotnet tool install -g Microsoft.Web.LibraryManager.Cli
ENV PATH="/root/.dotnet/tools:${PATH}"
