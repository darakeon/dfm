FROM darakeon/ubuntu
LABEL maintainer="Dara Keon <laboon@darakeon.com>"
RUN maintain

RUN apt-get install -y python3 python3-pip default-libmysqlclient-dev

COPY midna/src/requirements.txt requirements.txt
RUN python3 -m pip install -r requirements.txt

COPY midna/src /var/midna/src

EXPOSE 8627

WORKDIR /var/midna/src

CMD gunicorn -c ../config/prod.py
