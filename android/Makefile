MAKEFLAGS += --no-print-directory

PROJECTS=/mnt/c/Users/${WINDOWS_USER}/Projects
PROJECT=${PROJECTS}/keon/dfm
DKDROID=docker exec -it dkdroid
GRADLE=docker exec -it dkdroid gradle

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

apk:
	@${GRADLE} :app:bundleRelease

	@cp App/build/outputs/bundle/release/* \
		${PROJECT}/android/publish/ \
		|| echo "windows folder not found"

verify:
	@${GRADLE} dependencyCheckAnalyze \
		|| cp -r build/reports \
			${PROJECT}/android/ \
			|| echo "windows folder not found"

write-metadata:
	@if [ -e gradle/verification-metadata.xml ]; then rm gradle/verification-metadata.xml; fi
	@${GRADLE} --write-verification-metadata sha256 androidDependencies

restart: stop sleep start

clean:
	@rm -rf .gradle
	@rm -rf .idea
	@rm -rf build
	@rm -rf build-cache
	@rm -rf App/build
	@rm -rf ErrorLogs/build
	@rm -rf Lib/build
	@rm -rf TestUtils/build
	@rm -f local.properties
	@rm -f Lib/src/debug/res/values/site-address.xml
	@rm -f Lib/src/debug/res/xml/network_security_config.xml
	@mv keystore.properties keystore-$$(date +%Y%m%d%H%M%S%N).properties

ci:
	@if [ ! -e keystore.properties ]; then \
		echo "storeFile=\nstorePassword=\nkeyAlias=\nkeyPassword=" > keystore.properties; \
	fi
	@${GRADLE} androidDependencies --stacktrace
	@${GRADLE} robolectricSdkDownload
	@${GRADLE} build -x test -x androidDependencies -x robolectricSdkDownload -x packageRelease -x packageDebug
	@${GRADLE} configure
