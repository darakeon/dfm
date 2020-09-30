const puppeteer = require('puppeteer')
const fs = require('fs')

const db = require('./db.js')
const puppy = require('./puppy.js')

describe('Reports', () => {
	let user = {};
	let accountIn = {};
	let accountOut = {};
	let category = {};

	beforeAll(async () => {
		user = await puppy.logon('reports@dontflymoney.com')
		accountIn = await db.createAccountIfNotExists('Account In', user)
		accountOut = await db.createAccountIfNotExists('Account Out', user)
		category = await db.createCategoryIfNotExists('Category Move', user)
	})

	test('Index', async () => {
		const url = `Account/${accountOut}/Reports`

		await puppy.call(url)
		const alert = await puppy.content('#body .alert')
		expect(alert).toContain(
			`Não há Movimentações para este mês.`
		)
	})

	test('Month', async () => {
		const url = `Account/${accountOut}/Reports/ShowMoves/201911`

		await puppy.call(url)
		const alert = await puppy.content('#body .alert')
		expect(alert).toContain(
			`Não há Movimentações para este mês.`
		)

		await puppy.createMove(
			db,
			'Move for Month', '2019-11-16', '1,00',
			category, accountOut, null,
			user
		)

		await puppy.call(url)
		const table = await puppy.content('#body .table')
		expect(table).toContain(
			`<td>Move for Month</td>`
		)
	})

	test('Year', async () => {
		const url = `Account/${accountOut}/Reports/SummarizeMonths/2018`

		await puppy.call(url)
		const alert = await puppy.content('#body .alert')
		expect(alert).toContain(
			`Não há Movimentações para este ano.`
		)

		await puppy.createMove(
			db,
			'Move for Year', '2018-11-16', '1,00',
			category, accountOut, null
		)

		await puppy.call(url)
		const table = await puppy.content('#body .table')
		expect(table).toContain('Novembro')
	})
})
