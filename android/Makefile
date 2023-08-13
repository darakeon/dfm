PROJECTS=/mnt/c/Users/${WINDOWS_USER}/Projects
PROJECT=${PROJECTS}/keon/dfm

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
	@docker exec -it dkdroid \
		gradle :app:bundleRelease

	@cp App/build/outputs/bundle/release/* \
		${PROJECT}/android/publish/ \
		|| echo "windows folder not found"

verify:
	@docker exec -it dkdroid \
		gradle dependencyCheckAnalyze \
		|| cp -r build/reports \
			${PROJECT}/android/ \
			|| echo "windows folder not found"

write-metadata:
	@if [ -e gradle/verification-metadata.xml ]; then rm gradle/verification-metadata.xml; fi
	@docker exec -it dkdroid \
		gradle --write-verification-metadata sha256 help
