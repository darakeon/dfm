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
		darakeon/dfm-migrator-ci \
		python3 update_contract_readme.py

update-contract-html:
	@docker run \
		-it \
		-v .:/var/dfm \
		-w /var/dfm \
		--env-file ../architecture/publish/tests/cfg/python.env \
		-e VERSION=${VERSION} \
		--rm \
		darakeon/dfm-migrator-ci \
		python3 update_contract_html.py

publish-contract-html:
	@docker run \
		-it \
		-v ${PWD}/..:/var/dfm \
		-w /var/dfm/terms \
		-e S3_TERMS_ACCESS_KEY=${S3_TERMS_ACCESS_KEY} \
		-e S3_TERMS_SECRET_KEY=${S3_TERMS_SECRET_KEY} \
		-e S3_TERMS_BUCKET=${S3_TERMS_BUCKET} \
		--rm \
		darakeon/dfm-migrator-ci \
		python3 publish_contract_html.py
