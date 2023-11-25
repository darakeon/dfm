MAKEFLAGS += --no-print-directory

all: site_push robot_push midna_push

robot:
	@make single MACHINE=robot

robot_push: robot
	@make push MACHINE=robot

site:
	@make single MACHINE=site

site_push: site
	@make push MACHINE=site

midna:
	@make single MACHINE=midna

midna_push: midna
	@make push MACHINE=midna

single:
	@echo "Building ${MACHINE}..."
	@docker build .. --pull --progress plain -t darakeon/dfm-${MACHINE} -f "${PWD}/dfm-${MACHINE}.dockerfile"
	@echo "${MACHINE} built!"

push:
	@echo "Pushing ${MACHINE}..."
	@docker push darakeon/dfm-${MACHINE}
	@echo "${MACHINE} pushed!"
