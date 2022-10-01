apk:
	docker run -it \
		-v ${PWD}:/var/android \
		-v /mnt/c/Users/lcsal/Projects/keon/access:/var/keys \
		-w /var/android \
		darakeon/android \
		gradle :app:bundleRelease
