FROM darakeon/netcore-libman
LABEL maintainer="Dara Keon <laboon@darakeon.com>"
RUN maintain

COPY core /var/dfm/core
COPY api /var/dfm/api
RUN cd /var/dfm/api/API \
	&& libman restore \
	&& dotnet publish API.csproj -o /var/www \
	&& maintain \
	&& rm -r /var/dfm

ENV ASPNETCORE_ENVIRONMENT=amazon

ENV ASPNETCORE_URLS=http://+:2232;https://+:2023
EXPOSE 2023

WORKDIR /var/www

CMD cp /var/cfg/* . && ./DFM.API
