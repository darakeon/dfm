MAKEFLAGS += --no-print-directory

PROJECTS=/mnt/c/Users/${WINDOWS_USER}/Projects
PROJECT=${PROJECTS}/keon/dfm
DKDROID=docker exec -it dkdroid
GRADLE=docker exec -it dkdroid gradle --no-daemon

start:
	@docker run -d --rm \
		-v ${PWD}:/var/android \
		-v ${PROJECTS}/keon/access:/var/keys \
		-w /var/android \
		--name dkdroid \
		darakeon/android \
		tail -f /dev/null

stop:
	@docker stop dkdroid || echo "no dkdroid to stop"

sleep:
	@sleep 2

bash:
	@docker exec -it dkdroid bash

apk:
	@${GRADLE} :app:bundleRelease -Phide-test-deps

	@cp App/build/outputs/bundle/release/* \
		${PROJECT}/android/publish/ \
		|| echo "windows folder not found"

verify:
	@${GRADLE} dependencyCheckAnalyze \
		|| cp -r build/reports \
			${PROJECT}/android/ \
			|| echo "windows folder not found"

write-metadata:
	@${GRADLE} build --write-verification-metadata sha256

rewrite-metadata:
	@if [ -e gradle/verification-metadata.xml ]; then rm gradle/verification-metadata.xml; fi
	@make write-metadata

restart: stop sleep start

clean:
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

ci-keystore:
	@if [ ! -e fake.keystore ]; then \
		echo "" > fake.keystore; \
	fi
	@if [ ! -e keystore.properties ]; then \
		echo "storeFile=fake.keystore\nstorePassword=\nkeyAlias=\nkeyPassword=" > keystore.properties; \
	fi

ci-androidDeps:
	@${GRADLE} androidDependencies

ci-build:
	@${GRADLE} build -x test -x androidDependencies -x packageRelease -x packageDebug

ci-configure:
	@${GRADLE} configure

ci: ci-keystore ci-androidDeps ci-build ci-configure

test:
	@${GRADLE} test -x build -x androidDependencies -x packageRelease -x packageDebug
