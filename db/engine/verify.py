# https://dev.mysql.com/doc/connector-python/en/connector-python-example-connecting.html
import mysql.connector

# https://learn.microsoft.com/en-us/sql/connect/python/pyodbc/step-1-configure-development-environment-for-pyodbc-python-development?view=sql-server-ver16&tabs=windows
import pymssql

from prod import (
	aws_hostname,
	aws_database,
	aws_username,
	aws_password,
	az_hostname,
	az_database,
	az_username,
	az_password,
)


tables = [
	'acceptance',
	'account',
	'archive',
	'category',
	'contract',
	'control',
	'detail',
	'line',
	'migrations',
	'move',
	'order_',
	'order_account',
	'order_category',
	'plan',
	'schedule',
	'security',
	'settings',
	'summary',
	'terms',
	'ticket',
	'tips',
	'user',
	'wipe',
]

mssql_connstr = f'DRIVER={{ODBC Driver 18 for SQL Server}};SERVER={az_hostname};DATABASE={az_database};UID={az_username};PWD={az_password}'


with mysql.connector.connect(
	user=aws_username,
	password=aws_password,
	host=aws_hostname,
	database=aws_database,
) as mysql_connection:

	with pymssql.connect(
		server=az_hostname,
		user=az_username,
		password=az_password,
		database=az_database,
		as_dict=True
	) as mssql_connection:

		for table in tables:

			print(table, end='')

			with mysql_connection.cursor( dictionary=True) as mysql_cursor:
				with mssql_connection.cursor() as mssql_cursor:

					mysql_cursor.execute(f'SELECT COUNT(*) as qty FROM {table}')
					mssql_cursor.execute(f'SELECT COUNT(*) as qty FROM "{table}"')

					mysql_row = mysql_cursor.fetchall()[0]
					mssql_row = mssql_cursor.fetchall()[0]

					if mssql_row['qty'] == mysql_row['qty']:
						print(f': {mysql_row["qty"]}')

					else:
						print('\033[31m')
						print(f'MySQL {mssql_row["qty"]}')
						print(f'MSSQL {mysql_row["qty"]}')
						print('\033[m', end='')
						continue


					if not mysql_row["qty"]:
						continue

					mysql_cursor.execute(f'SELECT * FROM {table} order by ID')
					mssql_cursor.execute(f'SELECT * FROM "{table}" order by ID')

					while True:
						mysql_row = mysql_cursor.fetchone()
						mssql_row = mssql_cursor.fetchone()

						if mysql_row == None:
							break

						for col in mysql_row:
							if mssql_row[col] != mysql_row[col]:
								print('\033[31m')
								print(f'    MySQL {mysql_row["ID"]}')
								print(f'    MSSQL {mssql_row["ID"]}')
								print('\033[m', end='')
