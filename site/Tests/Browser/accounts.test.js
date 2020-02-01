const puppeteer = require('puppeteer')
const fs = require('fs')

const db = require('./db.js')
const puppy = require('./puppy.js')

describe('Accounts', () => {
	let user = {};

	beforeAll(async () => {
		await db.cleanup()
		user = await puppy.logon('accounts@dontflymoney.com')
	}, 30000)

	test('Empty List', async () => {
		await puppy.call('Accounts')

		const warning = await puppy.content('.alert')
		expect(warning).toContain('Não há Contas ainda.')
	})

	test('Create', async () => {
		await puppy.call('Accounts/Create')
		await page.waitForSelector('#body form')

		await page.type('#Account_Name', 'Account Create')
		await page.type('#Account_Url', '')
		await page.click('#body form button[type="submit"]')

		const table = await puppy.content('#body .table')
		expect(table).toContain(
			'<a href="/Account/account_create">Account Create</a>'
		)
	})

	test('Edit', async () => {
		const name = 'Account Edit'
		const url = await db.createAccountIfNotExists(name, user)

		await puppy.call(`Accounts/Edit/${url}`)
		await page.waitForSelector('#body form')

		await puppy.clear('#Account_Name')
		await page.type('#Account_Name', 'Account 2')
		await page.click('#body form button[type="submit"]')

		const table = await puppy.content('#body .table')
		expect(table).toContain(
			'<a href="/Account/account_edit">Account 2</a>'
		)
	})

	test('Delete', async () => {
		const name = 'Account Delete'
		const url = await db.createAccountIfNotExists(name, user)

		await puppy.call('Accounts')
		
		await puppy.submit(`/Accounts/Delete/${url}`)

		const body = await puppy.content('#body')
		expect(body).not.toContain(
			'<a href="/Account/account_delete">Account Delete</a>'
		)
	})

	test('Close', async () => {
		const name = 'Account Close'
		const url = await db.createAccountIfNotExists(name, user)

		const category = await db.createCategoryIfNotExists(
			'Cat Acc Close', user
		)

		await puppy.createMove(
			db,
			'Move to close Account', '2019-11-16', '1,00',
			category, url, null,
			user
		)

		await puppy.call('Accounts')

		await puppy.submit(`/Accounts/Close/${url}`)

		let body = await puppy.content('#body')
		expect(body).not.toContain(
			'<a href="/Account/account_close">Account Close</a>'
		)

		await puppy.call('Accounts/ListClosed')

		body = await puppy.content('#body')

		const date = new Date()
		const month = date.getMonth() + 1
		const year = date.getYear() + 1900
		const report = year * 100 + month

		expect(body).toContain(
			`<a href="/Account/account_close/Reports/ShowMoves/${report}">Account Close</a>`
		)
	})
})
