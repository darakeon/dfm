MAKEFLAGS += --no-print-directory
RED=\033[01;31m
PURPLE=\033[01;35m
RESET=\033[00m
VERSION=$$(git branch --show-current)

-include .env

AWS_IMAGE=${AWS_ACCOUNT}.dkr.ecr.${AWS_ECR_REGION}.amazonaws.com/${AWS_ECR_REPO}:latest


env:
	@if [ ! -e .env ] && [ "${AWS_ACCOUNT}" = "" ]; then \
		echo "AWS_ACCOUNT=" >> .env; \
		echo "AWS_ECR_REGION=" >> .env; \
		echo "AWS_ECR_REPO=" >> .env; \
		echo "" >> .env; \
		echo "AWS_LAMBDA_NAME=" >> .env; \
		echo "AWS_LAMBDA_ROLE=" >> .env; \
		echo "AWS_SUBNETS=" >> .env; \
		echo "AWS_SG=" >> .env; \
		echo "" >> .env; \
		echo "CFG_DB=" >> .env; \
		echo "CFG_LOGIN=" >> .env; \
		echo "CFG_S3=" >> .env; \
		echo "CFG_SMTP=" >> .env; \
		echo "CFG_SQS=" >> .env; \
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

robot_aws_create:
	@aws lambda create-function \
		--function-name ${AWS_LAMBDA_NAME} \
		--role arn:aws:iam::${AWS_ACCOUNT}:role/${AWS_LAMBDA_ROLE} \
		--region ${AWS_ECR_REGION} \
		--package-type Image \
		--code ImageUri=${AWS_IMAGE} \
		--vpc-config SubnetIds=${AWS_SUBNETS},SecurityGroupIds=${AWS_SG},Ipv6AllowedForDualStack=false \
		--environment Variables={CFG_DB=${CFG_DB},CFG_LOGIN=${CFG_LOGIN},CFG_S3=${CFG_S3},CFG_SMTP=${CFG_SMTP},CFG_SQS=${CFG_SQS}} \
		--timeout 600 \
		| grep StateReasonCode \
		| sed 's/"//g' \
		| sed 's/StateReasonCode://g' \
		| sed 's/,//g' \
		| sed 's/ //g'

robot_aws_push: robot
	@docker tag darakeon/dfm-robot:latest ${AWS_IMAGE}
	@docker push ${AWS_IMAGE}

robot_aws_deploy:
	@aws lambda update-function-code \
		--function-name ${AWS_LAMBDA_NAME} \
		--image-uri ${AWS_IMAGE} \
		--publish \
		--region ${AWS_ECR_REGION} \
		| grep StateReasonCode \
		| sed 's/"//g' \
		| sed 's/StateReasonCode://g' \
		| sed 's/,//g' \
		| sed 's/ //g'

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
