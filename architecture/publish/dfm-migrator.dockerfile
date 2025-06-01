FROM darakeon/alpine
LABEL maintainer="Dara Keon <laboon@darakeon.com>"

RUN apk add python3 py3-pip py3-mysqlclient
RUN apk add pkgconfig

RUN apk add --no-cache mariadb-connector-c-dev; \
	apk add --no-cache --virtual .build-deps build-base mariadb-dev; \
	pip install mysqlclient; \
	apk del .build-deps;

RUN apk add gcc libc-dev python3-dev; \
	pip install --break-system-packages mysqlclient mysql-connector-python pytest pip-audit tzdata

RUN pip install --break-system-packages boto3

WORKDIR /var/db

COPY ./db /var/db/

SHELL ["/bin/bash", "-c"]
CMD python upgrade/main.py
