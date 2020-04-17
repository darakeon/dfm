FROM mcr.microsoft.com/dotnet/core/sdk:latest
MAINTAINER Dara Keon
RUN apt-get update
RUN dotnet tool install -g Microsoft.Web.LibraryManager.Cli
ENV PATH="/root/.dotnet/tools:${PATH}"
