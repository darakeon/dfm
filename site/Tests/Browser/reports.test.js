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
		const url = `Account/${accountOut}/Reports/Month/201911`

		await puppy.call(url)
		const alert = await puppy.content('#body .alert')
		expect(alert).toContain(
			`Não há Movimentações para este mês.`
		)

		const id = await puppy.createMove(
			db,
			'Move for Month', '2019-11-16', '1,00',
			category, accountOut, null,
			user
		)

		await puppy.call(url)
		const table = await puppy.content('#body .table')
		expect(table).toContain(`<td>Move for Month</td>`)

		const editUrl = `Account/${accountOut}/Moves/Edit/${id}`
		expect(table).toContain(editUrl)

		const deleteUrl = `Account/${accountOut}/Moves/Delete/${id}`
		expect(table).toContain(deleteUrl)
	})

	test('Year', async () => {
		const url = `Account/${accountOut}/Reports/Year/2018`

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

		const cell = async (r, c) => {
			return await puppy.content(
				`#body .table tr:nth-child(${r}) td:nth-child(${c})`
			)
		}

		const novemberOut = await cell(11, 3)
		expect(novemberOut).toContain('1,00')
		const novemberTotal = await cell(11, 4)
		expect(novemberTotal).toContain('minus-sign')
		expect(novemberTotal).toContain('1,00')
	})

	test('Search', async () => {
		const id = await puppy.createMove(
			db,
			'Search target', '2020-10-01', '24,00',
			category, accountOut, null
		)

		await puppy.call()

		await page.click('ul.nav li:nth-child(4) a')
		await page.waitForSelector(
			'#search-modal',
			{ visible: true }
		)

		await page.type('#search-modal .modal-body input', 'target')
		await page.click('#search-modal .modal-body button')

		const line = await puppy.content(`#body .table #m${id}`)
		expect(line).toContain('Search target')

		const reportUrl = `Account/${accountOut}/Reports/Month/202010`
		expect(line).toContain(reportUrl)
	})
})
