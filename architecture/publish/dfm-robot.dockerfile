FROM darakeon/netcore
LABEL maintainer="Dara Keon <laboon@darakeon.com>"
RUN maintain

COPY core /var/dfm/core
COPY robot /var/dfm/robot
RUN cd /var/dfm/robot/Robot \
	&& dotnet publish Robot.csproj -o /var/robot \
	&& apt-get remove -y dotnet-sdk-6.0 \
	&& maintain \
	&& rm -r /var/dfm

ENV ASPNETCORE_ENVIRONMENT=amazon

WORKDIR /var/robot

CMD cp /var/cfg/* . && ./DFM.Robot $TASK
