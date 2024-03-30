import os

from mysql.connector import connect


class Db:
	MYSQL_HOST=os.environ.get("MYSQL_HOST")
	MYSQL_DATABASE=os.environ.get("MYSQL_DATABASE")
	MYSQL_USERNAME=os.environ.get("MYSQL_USERNAME")
	MYSQL_PASSWORD=os.environ.get("MYSQL_PASSWORD")

	def execute_multi(self, queries):
		connection = self.connect_db()
		cursor = connection.cursor()
		
		execution = cursor.execute(queries, multi=True)

		statement = 0

		for _ in execution:
			statement += 1
			print('statement', statement)

		connection.commit()

	def execute_uni(self, query):
		connection = self.connect_db()
		cursor = connection.cursor()
		cursor.execute(query)
		return cursor.fetchall()

	def connect_db(self):
		if self.MYSQL_PASSWORD:
			return connect(
				host=self.MYSQL_HOST,
				database=self.MYSQL_DATABASE,
				user=self.MYSQL_USERNAME,
				password=self.MYSQL_PASSWORD,
			)
		else:
			return connect(
				host=self.MYSQL_HOST,
				database=self.MYSQL_DATABASE,
				user=self.MYSQL_USERNAME,
			)	
