FROM python
LABEL maintainer="Dara Keon <laboon@darakeon.com>"

COPY src/requirements.txt requirements.txt
RUN pip install -r requirements.txt

WORKDIR /var/midna/src

CMD gunicorn -c ../config/dev.py && tail -f /dev/null
