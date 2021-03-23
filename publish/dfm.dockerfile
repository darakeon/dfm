FROM darakeon/netcore-libman
MAINTAINER Dara Keon
RUN apt upgrade -y && apt update && apt autoremove -y

COPY site /var/dfm
RUN cd /var/dfm/MVC \
	&& libman restore \
	&& dotnet publish MVC.csproj -o /var/www \
	&& apt remove -y dotnet-sdk-5.0 \
	&& apt autoremove -y \
	&& rm -r /var/dfm \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

ENV ASPNETCORE_ENVIRONMENT=amazon

ENV ASPNETCORE_URLS=http://+:2106;https://+:2011
EXPOSE 2011

WORKDIR /var/www

CMD cp /var/cfg/* . && ./DFM.MVC
