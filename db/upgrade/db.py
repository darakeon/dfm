import os
import time

from mysql.connector import connect
from mysql.connector.errors import InterfaceError


class Db:
	MYSQL_HOST=os.environ.get("MYSQL_HOST")
	MYSQL_DATABASE=os.environ.get("MYSQL_DATABASE")
	MYSQL_USERNAME=os.environ.get("MYSQL_USERNAME")
	MYSQL_PASSWORD=os.environ.get("MYSQL_PASSWORD")

	def execute_multi(self, queries):
		connection = self.connect_db(0)
		cursor = connection.cursor()
		
		execution = cursor.execute(queries, multi=True)

		statement = 0
		for _ in execution:
			statement += 1
			print("statement", statement)

			sub_statement = 0
			for (result,) in cursor.fetchall():
				if not isinstance(result, str):
					continue

				sub_statement += 1
				print("sub statement", sub_statement)
				self.execute_uni(result)

		connection.commit()

	def execute_uni(self, query):
		connection = self.connect_db(0)
		cursor = connection.cursor()
		cursor.execute(query)
		return cursor.fetchall()

	def connect_db(self, count):
		try:
			return connect(
				host=self.MYSQL_HOST,
				database=self.MYSQL_DATABASE,
				user=self.MYSQL_USERNAME,
				password=self.MYSQL_PASSWORD,
			)
		except InterfaceError:
			if count == 9:
				raise

			print(f"Try: {count+2} (retry {count+1})")
			time.sleep(2)
			return self.connect_db(count + 1)
