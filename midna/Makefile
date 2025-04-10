MAKEFLAGS += --no-print-directory
.DEFAULT_GOAL=run
.PHONY: env build db run push stop rm logs bash python migrate midna test audit


-include env/dev.env
export

EXEC=docker exec -it midna

env:
	@if [ ! -e env/dev.env ]; then \
		cp env/example.env env/dev.env; \
	fi

build:
	@docker login
	@docker build . -t midna

db:
	@cd ../db && docker compose up -d

run: env stop rm build db
	@docker run -v ${PWD}/src:/var/midna/src \
		--env-file=env/dev.env \
		-p 8000:8000 \
		--name midna \
		-d \
		midna
	@echo
	@echo "Site running. Go to http://localhost:8000/twilight/"

push: build
	@docker push midna

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
	@${EXEC} python manage.py test plans

audit:
	@${EXEC} pip-audit
