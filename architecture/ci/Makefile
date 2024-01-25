MAKEFLAGS += --no-print-directory

ERROR=\033[0;31m
CLEAR=\033[0m

build_browser:
	@make single_build MACHINE=netcore-libman-node PUSH=${PUSH}
	@make single_build MACHINE=netcore-libman-node-chrome PUSH=${PUSH}

	@make single_build MACHINE=dfm-browser-tests PUSH=${PUSH}

build_android:
	@make single_build MACHINE=android TAG=gradle821-jdk17 PUSH=${PUSH}

single_build:
	@if [ "${MACHINE}" = "" ]; then \
		make raise_error TEXT="please set the MACHINE to build"; \
	fi

	@if [ -e ${MACHINE}.dockerfile ]; then \
		docker context use default; \
		docker login; \
		docker build . --pull -f ${MACHINE}.dockerfile -t darakeon/${MACHINE} --network=host; \
		docker tag darakeon/${MACHINE} ${MACHINE}; \
		docker image prune -f > /dev/null; \
		\
		if [ "${TAG}" ]; then \
			docker tag darakeon/${MACHINE} darakeon/${MACHINE}:${TAG}; \
		fi; \
		\
		if [ "${PUSH}" ]; then \
			docker push -a darakeon/${MACHINE}; \
		fi; \
	fi

rebuild_fi:
	@make rebuild NAME=fi

rebuild_beedle:
	@make rebuild NAME=beedle

rebuild_yoshi:
	@make rebuild NAME=yoshi

rebuild:
	@make launch TYPE=rebuild

start_fi:
	@make start NAME=fi

start_beedle:
	@make start NAME=beedle

start_yoshi:
	@make start NAME=yoshi

start:
	@make launch TYPE=start

launch:
	@if [ "${TYPE}" = "" ]; then \
		echo ; \
		echo "TYPE parameter is mandatory calling 'make launch' directly."; \
		echo "Use commands 'make rebuild' or 'make start' instead."; \
		echo ; \
		exit 1; \
	fi

# [android] fi
	@if [ "${NAME}" = "fi" ]; then \
		make _clear_android_build; \
		make _launch MACHINE=android PARAMETERS= WORK_DIR=android; \
	fi

# [netcore] beedle
	@if [ "${NAME}" = "beedle" ]; then \
		make _launch MACHINE=netcore PARAMETERS= WORK_DIR=; \
	fi

# [browser-tests] yoshi
	@if [ "${NAME}" = "yoshi" ]; then \
		make _launch MACHINE=dfm-browser-tests PARAMETERS="--expose 2709 -P" WORK_DIR=; \
	fi

# [rust] doki
	@if [ "${NAME}" = "doki" ]; then \
		make _launch MACHINE=rust PARAMETERS= WORK_DIR=version; \
	fi

_launch:
	@echo .
	@echo ..
	@echo ...
	@echo ====================================================================================================
	@echo "[${TYPE}] ${NAME} (${MACHINE})"
	@if [ "${PARAMETERS}" != "" ]; then \
		echo "${PARAMETERS}"; \
	fi
	@echo ====================================================================================================
	@echo ...
	@echo ..
	@echo .

	@if [ "${TYPE}" = "rebuild" ]; then \
		make single_build MACHINE=${MACHINE}; \
		docker rm ${NAME} > /dev/null 2> /dev/null; \
		docker run --name ${NAME} -it -v ${PWD}/../../:/var/dfm -w /var/dfm/${WORK_DIR} ${PARAMETERS} ${MACHINE}; \
	else \
		docker start -i ${NAME}; \
	fi

_clear_android_build:
	@rm -rf ../android/.gradle > /dev/null
	@rm -rf ../android/.idea > /dev/null
	@rm -rf ../android/App/build > /dev/null
	@rm -rf ../android/App/log > /dev/null
	@rm -rf ../android/build > /dev/null
	@rm -rf ../android/ErrorLogs/build > /dev/null
	@rm -rf ../android/Lib/build > /dev/null
	@rm -rf ../android/Lib/log > /dev/null
	@rm  -f ../android/Lib/src/debug/res/xml/network_security_config.xml > /dev/null 2> /dev/null
	@rm  -f ../android/Lib/src/debug/res/values/site-address.xml > /dev/null 2> /dev/null
	@rm -rf ../android/TestUtils/build > /dev/null
