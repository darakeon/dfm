const puppeteer = require('puppeteer')
const fs = require('fs')

const db = require('./db.js')
const puppy = require('./puppy.js')

describe('Schedules', () => {
	let user = {};
	let accountIn = {};
	let accountOut = {};
	let category = {};

	beforeAll(async () => {
		await db.cleanup()
		user = await puppy.logon('schedules@dontflymoney.com')
		accountIn = await db.createAccountIfNotExists('Account In', user)
		accountOut = await db.createAccountIfNotExists('Account Out', user)
		category = await db.createCategoryIfNotExists('Category Move', user)
	}, 30000)

	test('Index', async () => {
		await puppy.call('Schedules')

		const warning = await puppy.content('.alert')
		expect(warning).toContain('Não há Agendamentos ainda.')
	})

	test('Create', async () => {
		await puppy.call(`Account/${accountOut}/Schedules/Create`)
		await page.waitForSelector('#body form')

		await page.click('#ShowInstallment', category)
		await page.type('#Description', 'Schedule Create')
		await page.type('#Date', '16/11/2019')
		await page.select('#CategoryName', category)
		await page.select('#AccountOutUrl', accountOut)
		await page.type('#Value', '1,00')
		
		await page.click('#body form button[type="submit"]')

		const table = await puppy.content('#body .table')
		expect(table).toContain(
			`<td>Schedule Create [1/1]</td>`
		)
	})

	test('Delete', async () => {
		const result = await db.createSchedule(
			1, 0, 0, 1,
			'Schedule Delete', '16/11/2019', 0, 1,
			category, accountOut, null,
			user
		)
		const id = result.insertId

		await puppy.call('Schedules')

		await puppy.content('#body .table')
		await puppy.submit(`/Schedules/Delete/${id}`)

		const body = await puppy.content('#body')
		expect(body).not.toContain(
			`<td>Schedule Delete</td>`
		)
	})
})
