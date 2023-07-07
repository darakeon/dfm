FROM python
LABEL maintainer="Dara Keon <laboon@darakeon.com>"

COPY src/requirements.txt requirements.txt
RUN pip install -r requirements.txt

WORKDIR /var/midna/src

CMD python manage.py runserver 0.0.0.0:8000
