.DEFAULT_GOAL=run

-include .env
export

EXEC=docker exec -it midna

env:
	@if [ ! -e .env ]; then \
		cp example.env .env; \
	fi

build:
	@docker login
	@docker build . -t darakeon/midna

db:
	@cd ../sql && docker-compose up -d

run: env stop rm build db
	@docker run -v ${PWD}/src:/var/midna/src \
		-v ${PWD}/gunicorn/config:/var/midna/config \
		-v ${PWD}/gunicorn/log:/var/log/gunicorn/ \
		-v ${PWD}/gunicorn/run:/var/run/gunicorn/ \
		--env-file=.env \
		-p 8000:8000 \
		--name midna \
		-d \
		darakeon/midna
	@echo
	@echo "Site running. Go to http://localhost:8000/twilight/"

push: build
	@docker push darakeon/midna

stop:
	@docker stop midna 2> /dev/null || echo 'no midna'

rm:
	@docker rm midna 2> /dev/null || echo 'no midna'

logs:
	@docker logs midna

bash:
	@${EXEC} bash

python:
	@${EXEC} python manage.py shell

migrate:
	@${EXEC} python manage.py migrate

midna:
	@${EXEC} python manage.py createsuperuser --noinput

test:
	@${EXEC} python manage.py test deleted_users

audit:
	@${EXEC} pip-audit
