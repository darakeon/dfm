const puppeteer = require('puppeteer')
const fs = require('fs')

const db = require('./db.js')
const puppy = require('./puppy.js')

describe('Categories', () => {
	let user = {};

	beforeAll(async () => {
		await db.cleanup()
		user = await puppy.logon('categories@dontflymoney.com')
	}, 30000)

	test('Empty List', async () => {
		await puppy.call('Categories')

		const warning = await puppy.content('.alert')
		expect(warning).toContain('Não há Categorias ainda.')
	})

	test('Create', async () => {
		const name = 'Category Create'

		await puppy.call('Categories/Create')
		await page.waitForSelector('#body form')

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
		await page.waitForSelector('#body form')

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