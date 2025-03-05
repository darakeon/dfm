VERSION:=$$(git branch --show-current)

run:
	@docker compose up -d

stop:
	@docker compose down

clear: stop
	@sudo rm -r data

reset: clear run

logs:
	@docker logs -f dfm-migrator

sql:
	@if [ ! -e ${VERSION}.sql ]; then \
		echo "insert into migrations (name) values ('${VERSION}');" > ${VERSION}.sql; \
		echo "" >> ${VERSION}.sql; \
		echo "" >> ${VERSION}.sql; \
	fi
	@code ${VERSION}.sql
