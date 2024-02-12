FROM darakeon/netcore-libman as builder
LABEL maintainer="Dara Keon <laboon@darakeon.com>"

COPY core /var/dfm/core
RUN cd /var/dfm/core/BusinessLogic \
	&& dotnet build BusinessLogic.csproj -o /var/www

COPY api /var/dfm/api
RUN cd /var/dfm/api/API \
	&& dotnet publish API.csproj -o /var/www


FROM darakeon/netcore-server

COPY --from=builder /var/www /var/www

COPY --from=builder /root/.dotnet/corefx/cryptography/x509stores/my/*.pfx  /var/https/certificate.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/var/https/certificate.pfx

ENV ASPNETCORE_ENVIRONMENT=amazon

ENV ASPNETCORE_URLS=http://+:2232;https://+:2023
EXPOSE 2023

WORKDIR /var/www

CMD cp /var/cfg/* . && ./DFM.API
