run: stop
	@docker build . --quiet -t darakeon/midna
	@docker run -v ${PWD}:/var/midna -d -p 8000:8000 --rm --name midna darakeon/midna

stop:
	@docker stop midna 2> /dev/null || echo 'no midna'

logs:
	@docker logs midna
