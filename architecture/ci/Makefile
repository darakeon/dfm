MAKEFLAGS += --no-print-directory

CONTEXT=.


build_browser:
	@make single_build MACHINE=netcore-libman-node PUSH=${PUSH}
	@make single_build MACHINE=netcore-libman-node-chrome PUSH=${PUSH}
	@make single_build MACHINE=dfm-browser-tests PUSH=${PUSH}

build_android:
	@make single_build MACHINE=android TAG=gradle821-jdk17 PUSH=${PUSH}

build_sql_runner:
	@make single_build MACHINE=sql-runner CONTEXT=../.. PUSH=${PUSH}

build_sql_runner_ci:
	@make single_build MACHINE=sql-runner CONTEXT=../.. TAG=ci ONLY_TAG=1 PUSH=${PUSH}

single_build:
	@if [ "${MACHINE}" = "" ]; then \
		echo "please set the MACHINE to build"; \
	fi

	@docker build ${CONTEXT} -f ${MACHINE}.dockerfile -t darakeon/${MACHINE} --network=host
	@docker tag darakeon/${MACHINE} ${MACHINE}
	@docker image prune -f > /dev/null

	@if [ "${TAG}" ]; then \
		docker tag darakeon/${MACHINE} darakeon/${MACHINE}:${TAG}; \
	fi

	@if [ "${PUSH}" ]; then \
		if [ "${TAG}" ] && [ "${ONLY_TAG}" = "1" ]; then \
			echo push ${TAG}; \
			docker push darakeon/${MACHINE}:${TAG}; \
		else \
			echo push all; \
			docker push -a darakeon/${MACHINE}; \
		fi \
	fi


launch_android:
	@make _clear_android_build
	@make _launch NAME=fi MACHINE=android WORK_DIR=android

launch_dfm_browser_tests:
	@make _launch NAME=yoshi MACHINE=dfm-browser-tests PARAMETERS="--expose 2703 -P"

_launch:
	@docker ps -a -f name=${NAME} -q
	@if [ "$$(docker ps -a -f name=${NAME} -q)" = "" ]; then \
		make single_build MACHINE=${MACHINE}; \
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
