run:
	@docker compose up -d

clear:
	@docker compose down
	@sudo rm -r data

reset: clear run

logs:
	@docker logs dfm-sql-runner
