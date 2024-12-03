from deleted_users.models import Wipe
from plans.models import Plan


class MySqlRouter():
	def db_for_read(self, model, **hints):
		if model == Wipe or model == Plan:
			return 'mysql'
		return None

	def db_for_write(self, model, **hints):
		if model == Wipe or model == Plan:
			return 'mysql'
		return None
