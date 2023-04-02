MAKEFLAGS += --no-print-directory

MACHINE_NAME=dfm_bt
IS_MACHINE_RUNNING:=$$(make _is_machine_running)

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


_is_machine_running:
	@docker ps -a --format "table {{.ID}}\t{{.Names}}" | grep ${MACHINE_NAME} || :; 

_wait:
	@sleep 2
