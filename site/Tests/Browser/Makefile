MAKEFLAGS += --no-print-directory
.PHONY: help machine rm \
	restore_site publish_site run_site install_proj_deps create_contract \
	setup clear resetup test is_site_running

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
			-p 2703:2703 \
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
	@cd server && ./DFM.MVC > ../../../../outputs/logs/site/Browser/server.log 2> ../../../../outputs/logs/site/Browser/error.log &

install_proj_deps:
	@npm install

create_contract: install_proj_deps
	@node helpers/setup.js 2> /dev/null

setup: publish_site run_site _wait create_contract

clear:
	@cd server && rm -f tests.db
	@pkill DFM.MVC || echo "site is not running"

resetup: clear setup

test:
	@if [ "$(FILE)" = "" ]; then \
		npm test; \
	else \
		npm test -- $(FILE); \
	fi

is_site_running:
	@curl -o /dev/null -s http://localhost:2703 && echo Ok || echo Nop


_is_machine_running:
	@docker ps -a --format "table {{.ID}}\t{{.Names}}" | grep ${MACHINE_NAME} || :; 

_wait:
	@sleep 2
