run:
	@rm -f all.sql
	@cat [0-9].*.sql > all.sql
	@cat [0-9][0-9].*.sql >> all.sql
	@docker compose up -d

clear:
	@docker compose down
	@sudo rm -r ~/mysql/data

reset: clear run
