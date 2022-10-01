PROJECTS=/mnt/c/Users/lcsal/Projects

apk:
	@docker run -it \
		-v ${PWD}:/var/android \
		-v ${PROJECTS}/keon/access:/var/keys \
		-w /var/android \
		darakeon/android \
		gradle :app:bundleRelease

copy-windows:
	@cp App/build/outputs/bundle/release/* \
		${PROJECTS}/keon/dfm/android/publish/
