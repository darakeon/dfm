MAKEFLAGS += --no-print-directory
RED=\033[01;31m
PURPLE=\033[01;35m
RESET=\033[00m
VERSION=$$(git branch --show-current)

-include .env


env:
	@if [ ! -e .env ] && [ "${AWS_ACCOUNT}" = "" ]; then \
		echo "AWS_ACCOUNT=" >> .env; \
		echo "AWS_ECR_REGION=" >> .env; \
		echo "AWS_ECR_REPO=" >> .env; \
	fi


aws_login: env
	@if [ "${AWS_ACCOUNT}" = "" ]; then \
		echo "Fill .env file or set environment variables in the terminal"; \
		exit 1; \
	fi
	@if [ ! -e ~/.aws/config ]; then \
		echo "[default]\nregion = ${AWS_ECR_REGION}\noutput = json" > ~/.aws/config; \
	fi
	@if [ ! -e ~/.aws/credentials ]; then \
		echo "[default]\naws_access_key_id = ${AWS_ECR_USER}\naws_secret_access_key = ${AWS_ECR_PASS}" >> ~/.aws/credentials; \
	fi
	@aws ecr get-login-password --region ${AWS_ECR_REGION} | docker login --username AWS --password-stdin ${AWS_ACCOUNT}.dkr.ecr.${AWS_ECR_REGION}.amazonaws.com


all: site api robot midna

all_push: site_push api_push robot_push robot_aws_push midna_push

robot:
	@make single MACHINE=robot

robot_push: robot
	@make push MACHINE=robot

robot_aws_push: robot
	@docker tag darakeon/dfm-robot:latest ${AWS_ACCOUNT}.dkr.ecr.${AWS_ECR_REGION}.amazonaws.com/${AWS_ECR_REPO}:latest
	@docker push ${AWS_ACCOUNT}.dkr.ecr.${AWS_ECR_REGION}.amazonaws.com/${AWS_ECR_REPO}:latest

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

migrator:
	@make single MACHINE=migrator

migrator_push: migrator
	@make push MACHINE=migrator

single:
	@make echo COLOR="${RED}" TEXT="Building ${MACHINE}..."
	@docker build ../.. --pull --progress plain -t darakeon/dfm-${MACHINE} -f "${PWD}/dfm-${MACHINE}.dockerfile"
	@make echo COLOR="${PURPLE}" TEXT="${MACHINE} built!"

push:
	@make echo COLOR="${RED}" TEXT="Pushing ${MACHINE}..."
	@docker tag darakeon/dfm-${MACHINE} darakeon/dfm-${MACHINE}:${VERSION}
	@docker push darakeon/dfm-${MACHINE}
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
