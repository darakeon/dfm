FROM darakeon/alpine
LABEL maintainer="Dara Keon <laboon@darakeon.com>"

RUN apk add python3 py3-pip py3-mysqlclient
RUN apk add pkgconfig

COPY src/requirements.txt requirements.txt

RUN apk add --no-cache mariadb-connector-c-dev; \
	apk add --no-cache --virtual .build-deps build-base mariadb-dev; \
	pip install mysqlclient; \
	apk del .build-deps;

RUN apk add gcc libc-dev python3-dev; \
	python3 -m pip install --break-system-packages -r requirements.txt

WORKDIR /var/midna/src

SHELL ["/bin/bash", "-c"]
CMD python manage.py runserver 0.0.0.0:8000
