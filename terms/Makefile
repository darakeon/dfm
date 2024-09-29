VERSION=$$(git branch --show-current)

run-db:
	@cd ../db && make run

update-contract-readme:
	@docker run \
		-it \
		-v .:/var/dfm \
		-w /var/dfm \
		--env-file ../architecture/publish/tests/cfg/python.env \
		-e VERSION=${VERSION} \
		--rm \
		darakeon/dfm-midna \
		python3 update_contract_readme.py

update-contract-html:
	@docker run \
		-it \
		-v .:/var/dfm \
		-w /var/dfm \
		--env-file ../architecture/publish/tests/cfg/python.env \
		-e VERSION=${VERSION} \
		--rm \
		darakeon/dfm-midna \
		python3 update_contract_html.py
