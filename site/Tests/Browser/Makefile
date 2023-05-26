MAKEFLAGS += --no-print-directory

MACHINE_NAME=dfm_bt
IS_MACHINE_RUNNING:=$$(make _is_machine_running)

help:
	@echo ""
	@echo "Hi!"
	@echo ""
	@echo "The tests are easier to run using docker."
	@echo "This make is prepared to create the machine AND to be used inside of it."
	@echo ""
	@echo "At your terminal"
	@echo "    machine      start the docker container to run tests"
	@echo "    rm           remove the docker container"
	@echo ""
	@echo "Inside the docker machine"
	@echo "    setup        build and run the site"
	@echo "    clear        stop site and clear db"
	@echo "    resetup      call clear then setup"
	@echo "    test         run the automated browser tests"

machine:
	@if [ "${IS_MACHINE_RUNNING}" = "" ]; then \
		docker run -it \
			--name ${MACHINE_NAME} \
			-v ${PWD}/../../..:/var/dfm \
			-p 2709:2709 \
			-w /var/dfm/site/Tests/Browser \
			darakeon/dfm-browser-tests; \
	else \
		docker start -i ${MACHINE_NAME}; \
	fi

rm:
	@docker rm ${MACHINE_NAME}


# below commands are recommended to run inside docker machine

restore_site:
	@cd ../.. && dotnet restore Site.sln
	@cd ../../MVC && libman restore

publish_site: restore_site
	@cd ../../MVC && dotnet publish -c Release MVC.csproj -o ../Tests/Browser/server --no-restore

run_site:
	@cd server && ./DFM.MVC > ../log/server.log &

install_proj_deps:
	@npm install

create_contract: install_proj_deps
	@node contract.js 2> /dev/null

setup: publish_site run_site _wait create_contract

clear:
	@cd server && rm tests.db
	@pkill DFM.MVC

resetup: clear setup

test:
	@npm test

is_site_running:
	@npm test -- open


_is_machine_running:
	@docker ps -a --format "table {{.ID}}\t{{.Names}}" | grep ${MACHINE_NAME} || :; 

_wait:
	@sleep 2
