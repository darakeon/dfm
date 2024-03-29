FROM darakeon/ubuntu
LABEL maintainer="Dara Keon <laboon@darakeon.com>"
RUN maintain

RUN apt-get install -y python3 python3-pip default-libmysqlclient-dev pkg-config

RUN pip install --upgrade pip

COPY midna/src/requirements.txt requirements.txt
RUN python3 -m pip install -r requirements.txt

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
