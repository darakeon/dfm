const db = require('./db.js')
const puppy = require('./puppy.js')

describe('Categories', () => {
	let user = {};

	beforeAll(async () => {
		user = await puppy.logon('categories@dontflymoney.com')
	})

	test('Empty List', async () => {
		await puppy.call('Categories')

		const warning = await puppy.content('#body .well')
		expect(warning).toContain('Não há Categorias ainda.')

		const buttons = await puppy.content('#table-buttons')
		expect(buttons).toContain('Criar Categoria')
	})

	test('Filled List', async () => {
		const name = 'Category One'
		const url = await db.createCategoryIfNotExists(name, user)

		await puppy.call('Categories')

		const table = await puppy.content('table')
		expect(table).toContain(name)
		expect(table).toContain(url)

		const buttons = await puppy.content('#table-buttons')
		expect(buttons).toContain('Criar outra Categoria')
	})

	test('Create', async () => {
		const name = 'Category Create'

		await puppy.call('Categories/Create')
		await puppy.waitFor('#body form')

		await page.type('#Category_Name', name)
		await page.click('#body form button[type="submit"]')

		const table = await puppy.content('#body .table')
		const url = name.replace(' ', '%20')
		expect(table).toContain(
			`/Categories/Edit/${url}`
		)
	})

	test('Edit', async () => {
		let name = 'Category Edit'
		await db.createCategoryIfNotExists(name, user)

		await puppy.call(`Categories/Edit/${name}`)
		await puppy.waitFor('#body form')

		name = 'Category 2'
		await puppy.clear('#Category_Name')
		await page.type('#Category_Name', name)
		await page.click('#body form button[type="submit"]')

		const table = await puppy.content('#body .table')
		const url = name.replace(' ', '%20')
		expect(table).toContain(
			`/Categories/Edit/${url}`
		)
	})

	test('Disable', async () => {
		const name = 'Category Disable'
		await db.createCategoryIfNotExists(name, user)

		await puppy.call('Categories')

		const url = name.replace(' ', '%20')

		await puppy.submit(`/Categories/Disable/${url}`)

		const body = await puppy.content('#body')
		expect(body).toContain(
			`/Categories/Enable/${url}`
		)
	})

	test('Enable', async () => {
		const name = 'Category Enable'
		await db.createCategoryIfNotExists(name, user, true)

		await puppy.call('Categories')

		const url = name.replace(' ', '%20')

		await puppy.submit(`/Categories/Enable/${url}`)

		const body = await puppy.content('#body')
		expect(body).toContain(
			`/Categories/Disable/${url}`
		)
	})
})