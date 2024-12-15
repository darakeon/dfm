from json import loads
from os import environ

from MySQLdb._mysql import connect


def update_contract():
	copy_static()

	contracts = get_contracts()

	if not contracts:
		print('No contract to update')
		return

	with open('terms-template.html') as file:
		template = file.read()

	html = create_html(contracts, template)

	with open('static/index.html') as file:
		original = file.read()

	if original == html:
		print('Contracts already up to date')
		return

	with open('static/index.html', 'w') as file:
		file.write(html)

	print('Contracts updated')


def copy_static():
	copy('styles/contract.css', 'contract.css', '')
	copy('images/pig-on.ico', 'dfm.ico', 'b')
	copy('images/face-pig-on.png', 'dfm.png', 'b')


def copy(origin, destiny, mode):
	with open(f'../site/MVC/Assets/{origin}', f'r{mode}') as file:
		content = file.read()

	with open(f'static/{destiny}', f'w{mode}') as file:
		file.write(content)


def get_contracts():
	db = get_db()

	version = environ['VERSION']

	db.query(f'''
		SELECT Json, Language
			FROM terms t
				INNER JOIN contract c
					ON t.contract_id = c.id
		WHERE version = '{version}'
	''')

	# row by row: db.use_result()
	result = db.store_result()
	rows = result.fetch_row(maxrows=0, how=1)

	if not rows:
		return None

	contracts = {}

	for row in rows:
		lang = row['Language'].decode("utf-8")
		contracts[lang] = loads(row['Json'])

	return contracts


def get_db():
	return connect(
		host = environ['DATABASE_HOST'],
		database = environ['DATABASE_NAME'],
		user = environ['DATABASE_USER'],
		password = environ['DATABASE_PASS'],
	)


def create_html(contracts, html):
	contents = {}

	titles = {
		'pt-BR': 'Termos de uso',
		'en-US': 'Terms of use',
	}

	for lang in titles:
		contents[f'{lang}-title'] = titles[lang]
		contents[f'{lang}-content'] = read_all(contracts[lang], 0)

	for item in contents:
		html = html.replace(f'{{{{{item}}}}}', contents[item])

	return html


def read_all(item, level):	
	result = item['Text']

	if 'Items' in item:
		result = result + '<ol>'

		for subitem in item['Items']:
			result = result + '<li>' + read_all(subitem, level + 1) + '</li>'

		result = result + '</ol>'

	return result + '</li>'



update_contract()
