const db = require('./db')
const puppy = require('./puppy')
const { rand } = require('./utils')


describe('Moves', () => {
	let user = {};
	let accountIn = {};
	let accountOut = {};
	let category = {};

	beforeAll(async () => {
		user = await puppy.logon(`moves${rand()}@dontflymoney.com`)
		accountIn = await db.createAccountIfNotExists('Account In', user)
		accountOut = await db.createAccountIfNotExists('Account Out', user)
		category = await db.createCategoryIfNotExists('Category Move', user)
	})

	test('Index', async () => {
		await puppy.call(`Account/${accountOut}/Moves`)

		const panelHead = await puppy.content('.panel-heading')
		expect(panelHead).toContain('Criar Movimentações')
	})

	test('Create Out', async () => {
		await puppy.call(`Account/${accountOut}/Moves/Create`)
		await puppy.waitFor('#body form')

		await page.type('#Description', 'Move Create Out')
		await puppy.setValue('#Date', '2019-11-16')
		await page.select('#CategoryName', category)
		await page.select('#AccountOutUrl', accountOut)
		await page.type('#Value', '1,00')

		await page.click('#body form button[type="submit"]')

		const table = await puppy.content('#body .table')
		expect(table).toContain(
			`<td>Move Create Out</td>`
		)
	})

	test('Create In', async () => {
		await puppy.call(`Account/${accountIn}/Moves/Create`)
		await puppy.waitFor('#body form')

		await page.type('#Description', 'Move Create In')
		await puppy.setValue('#Date', '2019-11-16')
		await page.select('#CategoryName', category)
		await page.click('#Nature_In')
		await page.select('#AccountInUrl', accountIn)
		await page.type('#Value', '1,00')

		await page.click('#body form button[type="submit"]')

		const table = await puppy.content('#body .table')
		expect(table).toContain(
			`<td>Move Create In</td>`
		)
	})

	test('Create Transfer', async () => {
		await puppy.call(`Account/${accountOut}/Moves/Create`)
		await puppy.waitFor('#body form')

		await page.type('#Description', 'Move Create Transfer')
		await puppy.setValue('#Date', '2019-11-16')
		await page.select('#CategoryName', category)
		await page.click('#Nature_Transfer')
		await page.select('#AccountOutUrl', accountOut)
		await page.select('#AccountInUrl', accountIn)
		await page.type('#Value', '1,00')

		await page.click('#body form button[type="submit"]')

		const table = await puppy.content('#body .table')
		expect(table).toContain(
			`<td>Move Create Transfer</td>`
		)
	})

	test('Create Detailed', async () => {
		await puppy.call(`Account/${accountOut}/Moves/Create`)
		await puppy.waitFor('#body form')

		await page.type('#Description', 'Move Create Detailed')
		await puppy.setValue('#Date', '2019-11-16')
		await page.select('#CategoryName', category)
		await page.select('#AccountOutUrl', accountOut)

		await page.click('#Detailed_True')
		await page.type('#DetailList_0__Detail_Description', 'Detail')
		await page.type('#DetailList_0__Value', '1,00')
		await page.click('.btn-add-detail')
		await page.type('#DetailList_1__Detail_Description', 'Detail')
		await page.type('#DetailList_1__Value', '1,00')

		await page.click('#body form button[type="submit"]')

		const table = await puppy.content('#body .table')
		expect(table).toContain(
			`<td>Move Create Detailed</td>`
		)
	})

	test('Edit', async () => {
		const id = await puppy.createMove(
			db,
			'Move Edit', '2019-11-16', '1,00',
			category, accountOut, null
		)

		const form = `Account/${accountOut}/Moves/Edit/${id}`
		await puppy.call(form)
		await puppy.waitFor('#body form')

		await puppy.clear('#Description')
		await page.type('#Description', 'Move Edited')
		await page.type('#Value', '2,00')
		await puppy.submit('/' + form)

		const table = await puppy.content('#body .table')
		expect(table).toContain(`<td>Move Edited</td>`)
	})

	test('Delete', async () => {
		const id = await puppy.createMove(
			db,
			'Move Delete', '2019-11-16', '1,00',
			category, accountOut, null
		)

		await puppy.call(`Account/${accountOut}/Reports/Month/201911`)
		await puppy.waitFor('#body .table')

		const deleteUrl = `/Account/${accountOut}/Moves/Delete/${id}`
		await puppy.submit(deleteUrl)

		const table = await puppy.content('#body .table')
		expect(table).not.toContain(
			`<td>Move Delete</td>`
		)
	})

	test('Check', async () => {
		const id = await puppy.createMove(
			db,
			'Move Check', '2019-11-16', '1,00',
			category, accountOut, null
		)

		await puppy.call(`Account/${accountOut}/Reports/Month/201911`)
		await puppy.waitFor('#body .table')

		const checkUrl = `/Account/${accountOut}/Moves/Check/${id}`
		await puppy.submit(checkUrl)

		await page.waitForFunction(
			`document.getElementById("m${id}").innerHTML.indexOf("Moves/Check") == -1`
		)

		const tr = await puppy.content(`#m${id}`)
		const uncheckUrl = `/Account/${accountOut}/Moves/Uncheck/${id}`
		expect(tr).toContain(uncheckUrl)
	})

	test('Uncheck', async () => {
		const id = await puppy.createMove(
			db,
			'Move Uncheck', '2019-11-16', '1,00',
			category, accountOut, null
		)

		await db.checkMove('Move Uncheck', 2019, 11, 16, 'Out')

		await puppy.call(`Account/${accountOut}/Reports/Month/201911`)
		await puppy.waitFor('#body .table')

		const uncheckUrl = `/Account/${accountOut}/Moves/Uncheck/${id}`
		await puppy.submit(uncheckUrl)

		await page.waitForFunction(
			`document.getElementById("m${id}").innerHTML.indexOf("Moves/Uncheck") == -1`
		)

		const tr = await puppy.content(`#m${id}`)
		const checkUrl = `/Account/${accountOut}/Moves/Check/${id}`
		expect(tr).toContain(checkUrl)
	})

	test('Just numbers field', async () => {
		const form = `Account/${accountOut}/Moves/Create`
		await puppy.call(form)
		await puppy.waitFor('#body form')

		await page.type('#Value', 'A')

		const error = await puppy.content('#Value ~ .number-error', { visible: true })
		expect(error).toContain(`digite apenas números neste campo`)
	})
})
