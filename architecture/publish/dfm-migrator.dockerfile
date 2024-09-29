FROM darakeon/alpine
LABEL maintainer="Dara Keon <laboon@darakeon.com>"

RUN apk add python3 py3-pip py3-mysqlclient
RUN apk add pkgconfig

RUN apk add --no-cache mariadb-connector-c-dev; \
	apk add --no-cache --virtual .build-deps build-base mariadb-dev; \
	pip install mysqlclient; \
	apk del .build-deps;

RUN apk add gcc libc-dev python3-dev; \
	python3 -m pip install --break-system-packages mysqlclient mysql-connector-python pytest pip-audit tzdata

WORKDIR /var/db

COPY ./db /var/db/

CMD python upgrade/main.py
