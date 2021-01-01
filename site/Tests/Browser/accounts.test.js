const puppeteer = require('puppeteer')
const fs = require('fs')

const db = require('./db.js')
const puppy = require('./puppy.js')

describe('Accounts', () => {
	let user = {};

	beforeAll(async () => {
		user = await puppy.logon('accounts@dontflymoney.com')
	})

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
			'<a href="/Account/account_create" title="Movimentações da Conta">Account Create</a>'
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
			'<a href="/Account/account_edit" title="Movimentações da Conta">Account 2</a>'
		)
	})

	test('Delete', async () => {
		const name = 'Account Delete'
		const url = await db.createAccountIfNotExists(name, user)

		await puppy.call('Accounts')

		await puppy.submit(`/Accounts/Delete/${url}`)

		const body = await puppy.content('#body')
		expect(body).not.toContain(
			'<a href="/Account/account_delete" title="Movimentações da Conta">Account Delete</a>'
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
			'<a href="/Account/account_close" title="Movimentações da Conta">Account Close</a>'
		)

		await puppy.call('Accounts/ListClosed')

		body = await puppy.content('#body')

		const end = await db.getEndDate(url, user)
		const date = new Date(end)
		const month = date.getMonth() + 1
		const year = date.getYear() + 1900
		const report = year * 100 + month

		expect(body).toContain(
			`<a href="/Account/account_close/Reports/Month/${report}" title="Movimentações da Conta">Account Close</a>`
		)
	})

	test('Reopen', async () => {
		const name = 'Account Reopened'
		const url = await db.createAccountIfNotExists(name, user)

		const category = await db.createCategoryIfNotExists(
			'Cat Acc Reopen', user
		)

		await puppy.createMove(
			db,
			'Move to reopen Account', '2020-10-03', '1,00',
			category, url, null,
			user
		)

		await puppy.call('Accounts')
		await puppy.submit(`/Accounts/Close/${url}`)

		await puppy.call('Accounts/ListClosed')
		await puppy.submit(`/Accounts/Reopen/${url}`)

		body = await puppy.content('#body')

		await puppy.call('Accounts')

		expect(body).toContain(
			`<a href="/Account/account_reopened" title="Movimentações da Conta">Account Reopened</a>`
		)
	})
})
