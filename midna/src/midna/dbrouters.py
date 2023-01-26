from deleted_users.models import Wipe

class MySqlRouter(object):
	def db_for_read(self, model, **hints):
		print(model)
		if model == Wipe:
			return 'mysql'
		return None

	def db_for_write(self, model, **hints):
		print(model)
		if model == Wipe:
			return 'mysql'
		return None
