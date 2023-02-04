from deleted_users.models import Wipe

class MySqlRouter():
	def db_for_read(self, model, **hints):
		if model == Wipe:
			return 'mysql'
		return None

	def db_for_write(self, model, **hints):
		if model == Wipe:
			return 'mysql'
		return None
