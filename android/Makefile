PROJECTS=/mnt/c/Users/${WINDOWS_USER}/Projects
PROJECT=${PROJECTS}/keon/dfm
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
	@docker stop dkdroid

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

