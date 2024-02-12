FROM darakeon/netcore-libman as builder
LABEL maintainer="Dara Keon <laboon@darakeon.com>"

COPY core /var/dfm/core
RUN cd /var/dfm/core/BusinessLogic \
	&& dotnet build BusinessLogic.csproj -o /var/www

COPY robot /var/dfm/robot
RUN cd /var/dfm/robot/Robot \
	&& dotnet publish Robot.csproj -o /var/www


FROM darakeon/netcore-server

COPY --from=builder /var/www /var/www

COPY --from=builder /root/.dotnet/corefx/cryptography/x509stores/my/*.pfx  /var/https/certificate.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/var/https/certificate.pfx

ENV ASPNETCORE_ENVIRONMENT=amazon

WORKDIR /var/www

CMD cp /var/cfg/* . && ./DFM.Robot $TASK
