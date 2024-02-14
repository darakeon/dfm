FROM darakeon/alpine
LABEL maintainer="Dara Keon <laboon@darakeon.com>"

RUN apk add python3 py3-pip py3-mysqlclient
RUN apk add pkgconfig

COPY midna/src/requirements.txt requirements.txt

RUN apk add --no-cache mariadb-connector-c-dev; \
	apk add --no-cache --virtual .build-deps build-base mariadb-dev; \
	pip install mysqlclient; \
	apk del .build-deps;

RUN apk add gcc libc-dev python3-dev; \
	python3 -m pip install --break-system-packages -r requirements.txt

COPY midna/src /var/midna/src

EXPOSE 8627

WORKDIR /var/midna/src

RUN mkdir /var/midna/static
RUN mkdir /var/midna/static/inside
RUN mkdir /var/midna/static/outside
RUN SECRET_KEY=collectstatic \
	DATABASE_NAME= \
	DATABASE_USER= \
	DATABASE_PASS= \
	DATABASE_HOST= \
	DATABASE_PORT= \
	DOMAIN=collectstatic \
	python3 manage.py collectstatic

CMD cp -r ../static/inside/* ../static/outside \
	&& python3 manage.py migrate \
	&& python3 manage.py createsuperuser --noinput \
	&& gunicorn -c ../config/prod.py
