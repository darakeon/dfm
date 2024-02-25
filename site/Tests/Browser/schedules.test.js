const db = require('./db')
const puppy = require('./puppy')
const { rand } = require('./utils')


describe('Schedules', () => {
	let user = {};
	let accountIn = {};
	let accountOut = {};
	let category = {};

	beforeAll(async () => {
		user = await puppy.logon(`schedules${rand()}@dontflymoney.com`)
		accountIn = await db.createAccountIfNotExists('Account In', user)
		accountOut = await db.createAccountIfNotExists('Account Out', user)
		category = await db.createCategoryIfNotExists('Category Move', user)
	})

	test('Index', async () => {
		await puppy.call('Schedules')

		const warning = await puppy.content('#body .well')
		expect(warning).toContain('Não há Agendamentos ainda.')
	})

	test('Create', async () => {
		await puppy.call(`Account/${accountOut}/Schedules/Create`)
		await puppy.waitFor('#body form')

		await page.click('#ShowInstallment', category)
		await page.type('#Description', 'Schedule Create')
		await puppy.setValue('#Date', '2019-11-16')
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
		const id = await db.createSchedule(
			1, 0, 0, 1,
			'Schedule Delete', '2019-11-16', 0, 1,
			category, accountOut, null,
			user
		)

		await puppy.call('Schedules')

		await puppy.content('#body .table')
		await puppy.submit(`/Schedules/Delete/${id}`)

		const body = await puppy.content('#body')
		expect(body).not.toContain(
			`<td>Schedule Delete</td>`
		)
	})
})
