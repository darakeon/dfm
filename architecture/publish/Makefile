MAKEFLAGS += --no-print-directory
RED=\033[01;31m
PURPLE=\033[01;35m
RESET=\033[00m
VERSION=$$(git branch --show-current)


all: site api robot midna

all_push: site_push api_push robot_push midna_push

robot:
	@make single MACHINE=robot

robot_push: robot
	@make push MACHINE=robot

site:
	@make single MACHINE=site

site_push: site
	@make push MACHINE=site

api:
	@make single MACHINE=api

api_push: api
	@make push MACHINE=api

midna:
	@make single MACHINE=midna

midna_push: midna
	@make push MACHINE=midna

single:
	@make echo COLOR="${RED}" TEXT="Building ${MACHINE}..."
	@docker build ../.. --pull --progress plain -t darakeon/dfm-${MACHINE} -f "${PWD}/dfm-${MACHINE}.dockerfile"
	@make echo COLOR="${PURPLE}" TEXT="${MACHINE} built!"

push:
	@make echo COLOR="${RED}" TEXT="Pushing ${MACHINE}..."
	@docker tag darakeon/dfm-${MACHINE} darakeon/dfm-${MACHINE}:${VERSION}
#	@docker push darakeon/dfm-${MACHINE}
	@docker push darakeon/dfm-${MACHINE}:${VERSION}
	@make echo COLOR="${PURPLE}" TEXT="${MACHINE} pushed!"

echo:
	@echo
	@echo "${COLOR}"
	@echo ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	@echo ~~~~~~~~~ ${TEXT}
	@echo ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	@echo "${RESET}"
	@echo

run:
	@cd ../../db && pwd && make run
	@docker compose -f tests/docker-compose.yml up -d
	@docker compose -f tests/docker-compose.yml logs -f

stop:
	@docker compose -f tests/docker-compose.yml down
