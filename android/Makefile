MAKEFLAGS += --no-print-directory

PROJECTS=/mnt/c/Users/${WINDOWS_USER}/Projects
PROJECT=${PROJECTS}/keon/dfm

DKDROID=docker exec -it dkdroid
GRADLE=${DKDROID} gradle --no-daemon

start:
	@echo "start docker machine"
	@docker run -d --rm \
		-v ${PWD}/../:/var/dfm \
		-v ${PROJECTS}/keon/access:/var/keys \
		-v ${PWD}/../outputs/android/gradle:/var/cache/gradle \
		-w /var/dfm/android \
		-e NVD_API_KEY=${NVD_API_KEY} \
		--name dkdroid \
		darakeon/android \
		tail -f /dev/null
	@echo "docker machine started"

start-if-off:
	@if [ "$$(docker ps -a -f name=dkdroid -q)" = "" ]; then \
		make start; \
	fi

stop:
	@echo "stop docker machine"
	@docker stop dkdroid || echo "no dkdroid to stop"

sleep:
	@sleep 2

bash: start-if-off
	@${DKDROID} bash

apk: start-if-off
	@echo "create android installer"

	@${GRADLE} :app:bundleRelease -Phide-test-deps

	@cp App/build/outputs/bundle/release/* \
		${PROJECT}/android/publish/ \
		|| echo "windows folder not found"

verify: start-if-off
	@echo "check android dependencies"
	@${GRADLE} dependencyCheckAnalyze --stacktrace --info \
		|| cp -r build/reports \
			${PROJECT}/android/ \
			|| echo "windows folder not found"

write-metadata: start-if-off
	@echo "update data for check android dependencies"
	@${GRADLE} build --exclude-task :App:packageRelease --write-verification-metadata sha256

rewrite-metadata: start-if-off
	@echo "recreate data for check android dependencies"
	@if [ -e gradle/verification-metadata.xml ]; then rm gradle/verification-metadata.xml; fi
	@make write-metadata

restart: stop sleep start

clean:
	@echo "clear all dependencies and build results"
	@sudo rm -rf .gradle
	@sudo rm -rf .idea
	@sudo rm -rf build
	@sudo rm -rf build-cache
	@sudo rm -rf App/build
	@sudo rm -rf ErrorLogs/build
	@sudo rm -rf Lib/build
	@sudo rm -rf TestUtils/build
	@sudo rm -f local.properties
	@sudo rm -f Lib/src/debug/res/values/site-address.xml
	@sudo rm -f Lib/src/debug/res/xml/network_security_config.xml
	@if [ -e keystore.properties ]; then \
		mv keystore.properties keystore-$$(date +%Y%m%d%H%M%S%N).properties; \
	fi

ci-keystore: start-if-off
	@echo "create fake keys for build installer"
	@if [ ! -e fake.keystore ]; then \
		echo "" > fake.keystore; \
	fi
	@if [ ! -e keystore.properties ]; then \
		echo "storeFile=fake.keystore\nstorePassword=\nkeyAlias=\nkeyPassword=" > keystore.properties; \
	fi

ci-androidDeps: start-if-off
	@echo "install dependencies"
	@${GRADLE} androidDependencies

ci-build: start-if-off
	@echo "build project"
	@${GRADLE} build -x test -x androidDependencies -x packageRelease -x packageDebug

ci-configure: start-if-off
	@echo "configure gradle"
	@${GRADLE} configure

ci: ci-keystore ci-androidDeps ci-build ci-configure

test: start-if-off
	@echo "run all project tests"
	@${GRADLE} test -x build -x androidDependencies -x packageRelease -x packageDebug
