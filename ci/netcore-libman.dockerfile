FROM darakeon/netcore
MAINTAINER Dara Keon
RUN apt upgrade -y && apt update && apt autoremove -y

RUN dotnet tool install -g Microsoft.Web.LibraryManager.Cli
ENV PATH="/root/.dotnet/tools:${PATH}"
