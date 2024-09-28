from json import loads
from os import environ

from MySQLdb._mysql import connect


def update_contract():
	contracts = get_contracts()

	if not contracts:
		print('No contract to update')
		return

	md = create_md(contracts)

	with open('README.md') as readme:
		original = readme.read()

	if original == md:
		print('Contracts already up to date')
		return

	with open('README.md', 'w') as readme:
		readme.write(md)

	print('Contracts updated')


def get_contracts():
	db = get_db()

	version = environ['VERSION']

	db.query(f"""
		SELECT Json, Language
			FROM terms t
				INNER JOIN contract c
					ON t.contract_id = c.id
		WHERE version = '{version}'
	""")

	# row by row: db.use_result()
	result = db.store_result()
	rows = result.fetch_row(maxrows=0, how=1)

	if not rows:
		return None

	contracts = {}

	for row in rows:
		contracts[row['Language']] = loads(row['Json'])

	return contracts


def get_db():
	return connect(
		host = environ['DATABASE_HOST'],
		database = environ['DATABASE_NAME'],
		user = environ['DATABASE_USER'],
		password = environ['DATABASE_PASS'],
	)


def create_md(contracts):
	md = "# Don't fly money\n\n"

	titles = {
		b"pt-BR": "Termos de uso",
		b"en-US": "Terms of use",
	}

	for lang in titles:
		md = md + "## " + titles[lang] + "\n\n"

		contract = contracts[lang]

		md = md + read_all(contract, 0)

	return md


def read_all(item, level):
	if level == 0:
		result = ""
	else:
		result = "".ljust((level - 1) * 4) + "- "

	result = result + item["Text"] + "\n\n"

	if "Items" in item:
		for subitem in item["Items"]:
			result = result + read_all(subitem, level + 1)

	return result



update_contract()
