import os
import time

from mysql.connector import connect
from mysql.connector.errors import InterfaceError


class Db:
	DATABASE_HOST=os.environ.get("DATABASE_HOST")
	DATABASE_NAME=os.environ.get("DATABASE_NAME")
	DATABASE_USER=os.environ.get("DATABASE_USER")
	DATABASE_PASS=os.environ.get("DATABASE_PASS")

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
				host=self.DATABASE_HOST,
				database=self.DATABASE_NAME,
				user=self.DATABASE_USER,
				password=self.DATABASE_PASS,
			)
		except InterfaceError:
			if count == 9:
				raise

			print(f"Try: {count+2} (retry {count+1})")
			time.sleep(2)
			return self.connect_db(count + 1)
