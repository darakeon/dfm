FROM darakeon/netcore
LABEL maintainer="Dara Keon <laboon@darakeon.com>"
RUN maintain

RUN dotnet tool install -g Microsoft.Web.LibraryManager.Cli
ENV PATH="/root/.dotnet/tools:${PATH}"
