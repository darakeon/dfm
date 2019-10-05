const puppeteer = require('puppeteer')
const fs = require('fs')

const db = require('./db.js')
const puppy = require('./puppy.js')

describe('Accounts', () => {
	let user = {};

	beforeAll(async () => {
		await db.cleanup()

		const email = 'accounts@dontflymoney.com'
		user = await db.createUserIfNotExists(email, 1)

		const cookies = await page.cookies()
		
		if (cookies.length == 0) await logon(email)
			
		const dfmCookie = cookies
			.filter(c => c.name == 'DFM')[0]

		if (!dfmCookie || !dfmCookie.value) await logon(email)
	}, 30000)
	
	async function logon(email) {
		await puppy.call('Users/Logon')
		await page.waitForSelector('#body form')

		await page.type('#Email', email)
		await page.type('#Password', db.password.plain)
		await page.click('#RememberMe')
		await page.click('#body form button[type="submit"]')
	}

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
		
		await page.click(`#body form[action="/Accounts/Delete/${url}"] button[type="submit"]`)

		const body = await puppy.content('#body')
		expect(body).not.toContain(
			'<a href="/Account/account_delete">Account Delete</a>'
		)
	})
})