import os

from db import Db
from migration import Migration
from ui import UI


def main():
	db = Db()

	done_migrations = get_done_migrations(db)
	pending_migrations = get_pending_migrations(done_migrations)

	for migration in pending_migrations:
		if not migration.is_data or os.environ.get("RUN_DATA"):
			execute_migration(db, migration)


def get_done_migrations(db: Db):
	result = db.execute_uni("SHOW TABLES LIKE 'migrations'")
	if len(result) == 0:
		return []

	result = db.execute_uni("SELECT name FROM migrations")
	return [ name for (name,) in result ]


def get_pending_migrations(done_migrations):
	sqls = os.listdir(".")
	migrations = []

	for sql in sqls:
		migration = Migration.create(sql)

		if migration and migration.name not in done_migrations:
			with open(sql) as migration_file:
				migration.set_queries(migration_file.read())
				migrations.append(migration)

	migrations.sort(key=lambda mig: mig.code)

	return migrations


def execute_migration(db: Db, migration: Migration):
	migration_insert = f"INSERT INTO migrations (name) VALUES ('{migration.name}')"

	UI.print_black(f"{migration.name} (data: {migration.is_data})")

	if migration.insert_itself and migration_insert.lower() not in migration.queries.lower():
		print("Migration must insert itself in migrations table")
		print("You can add this to the end of the file:")
		print(migration_insert)
		exit(1)

	db.execute_multi(migration.queries)


if __name__ == "__main__":
	UI.print_red("STARTING")
	main()
	UI.print_purple("ENDED")
