run:
	@docker compose up -d

clear:
	@docker compose down
	@sudo rm -r data

reset: clear run

logs:
	@docker logs -f dfm-sql-runner
