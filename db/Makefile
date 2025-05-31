VERSION:=$$(git branch --show-current)
VERSION_SQL:=$$(echo migrations/${VERSION}.sql)

run:
	@chmod +x localstack.sh
	@docker compose up -d

stop:
	@docker compose down

clear: stop
	@sudo rm -r data

reset: clear run

logs:
	@docker logs -f dfm-migrator

sql:
	@if [ ! -e ${VERSION_SQL} ]; then \
		echo "insert into migrations (name) values ('${VERSION}');" > ${VERSION_SQL}; \
		echo "" >> ${VERSION_SQL}; \
		echo "" >> ${VERSION_SQL}; \
	fi
	@code ${VERSION_SQL}

check_cloud_migration:
	@cd engine && python3 verify.py
