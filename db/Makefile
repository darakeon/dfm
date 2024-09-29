run:
	@docker compose up -d

stop:
	@docker compose down

clear: stop
	@sudo rm -r data

reset: clear run

logs:
	@docker logs -f dfm-migrator
