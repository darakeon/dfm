.DEFAULT_GOAL=run

include .env
export

EXEC=docker exec -it midna

build:
	@docker login
	@docker build . -t darakeon/midna

db:
	@cd ../sql && docker-compose up -d

run: stop build db
	@docker run -v ${PWD}:/var/midna \
		--env-file=.env \
		-p 8000:8000 \
		--name midna \
		-d --rm \
		darakeon/midna
	@echo
	@echo "Site running. Go to http://localhost:8000/twilight/"

push: build
	@docker push darakeon/midna

stop:
	@docker stop midna 2> /dev/null || echo 'no midna'

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
