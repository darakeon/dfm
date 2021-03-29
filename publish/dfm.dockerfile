FROM darakeon/netcore-libman
MAINTAINER Dara Keon <laboon@darakeon.com>
RUN maintain

COPY site /var/dfm
RUN cd /var/dfm/MVC \
	&& libman restore \
	&& dotnet publish MVC.csproj -o /var/www \
	&& apt remove -y dotnet-sdk-5.0 \
	&& apt autoremove -y \
	&& rm -r /var/dfm \
    && apt clean \
    && rm -rf /var/lib/apt/lists/*

ENV ASPNETCORE_ENVIRONMENT=amazon

ENV ASPNETCORE_URLS=http://+:2106;https://+:2011
EXPOSE 2011

WORKDIR /var/www

CMD cp /var/cfg/* . && ./DFM.MVC
