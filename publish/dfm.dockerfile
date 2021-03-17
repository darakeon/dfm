FROM darakeon/netcore-libman
MAINTAINER Dara Keon
RUN apt update

COPY site /var/dfm
RUN cd /var/dfm/MVC && libman restore
RUN dotnet publish /var/dfm/MVC/MVC.csproj -o /var/www
RUN apt remove -y dotnet-sdk-5.0
RUN rm -r /var/dfm

ENV ASPNETCORE_ENVIRONMENT=amazon

ENV ASPNETCORE_URLS=http://+:2106;https://+:2011
EXPOSE 2011

WORKDIR /var/www

CMD cp /var/cfg/* . && ./DFM.MVC
