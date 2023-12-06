FROM darakeon/netcore-libman
LABEL maintainer="Dara Keon <laboon@darakeon.com>"
RUN maintain

COPY core /var/dfm/core
COPY site /var/dfm/site
RUN cd /var/dfm/site/MVC \
	&& libman restore \
	&& dotnet publish MVC.csproj -o /var/www \
	&& maintain \
	&& rm -r /var/dfm

ENV ASPNETCORE_ENVIRONMENT=amazon

ENV ASPNETCORE_URLS=http://+:2106;https://+:2011
EXPOSE 2011

WORKDIR /var/www

CMD cp /var/cfg/* . && ./DFM.MVC
