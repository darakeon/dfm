const db = require('./db.js')
const puppy = require('./puppy.js')

describe('Reports', () => {
	let user = {};
	let account = {};
	let category = {};

	describe('Screen', () => {
		beforeAll(async () => {
			user = await puppy.logon('reports@dontflymoney.com')
			account = await db.createAccountIfNotExists('Account Reports', user)
			category = await db.createCategoryIfNotExists('Category Reports', user)
		})
	
		test('Index', async () => {
			const url = `Account/${account}/Reports`
	
			await puppy.call(url)
			const alert = await puppy.content('#body .alert')
			expect(alert).toContain(
				`Não há movimentações para este mês.`
			)
		})
	
		test('Month', async () => {
			const url = `Account/${account}/Reports/Month/201911`
	
			await puppy.call(url)
			const alert = await puppy.content('#body .alert')
			expect(alert).toContain(
				`Não há movimentações para este mês.`
			)
	
			const id = await puppy.createMove(
				db,
				'Move for Month', '2019-11-16', '1,00',
				category, account, null,
				user
			)
	
			await puppy.call(url)
			const table = await puppy.content('#body .table')
			expect(table).toContain(`<td>Move for Month</td>`)
	
			const editUrl = `Account/${account}/Moves/Edit/${id}`
			expect(table).toContain(editUrl)
	
			const deleteUrl = `Account/${account}/Moves/Delete/${id}`
			expect(table).toContain(deleteUrl)
		})
	
		test('Year', async () => {
			const url = `Account/${account}/Reports/Year/2018`
	
			await puppy.call(url)
			const alert = await puppy.content('#body .alert')
			expect(alert).toContain(
				`Não há movimentações para este ano.`
			)

			await puppy.createMove(
				db,
				'Move for Year', '2018-11-16', '1,00',
				category, account, null
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
				category, account, null
			)
	
			await puppy.call()
	
			await page.click('ul.nav li:nth-child(4) a')
			await puppy.waitFor(
				'#search-modal',
				{ visible: true }
			)
	
			await page.type('#search-modal .modal-body input', 'target')
			await page.click('#search-modal .modal-body button')
	
			const line = await puppy.content(`#body .table #m${id}`)
			expect(line).toContain('Search target')
	
			const reportUrl = `Account/${account}/Reports/Month/202010`
			expect(line).toContain(reportUrl)
		})
	
		test('Categories', async () => {
			const yearUrl = `Account/${account}/Reports/Categories/2021`
			const monthUrl = `${yearUrl}08`
	
			await puppy.call(monthUrl)
			const monthAlert = await puppy.content('#body .alert')
			expect(monthAlert).toContain(`Não há movimentações para este mês.`)
	
			await puppy.call(yearUrl)
			const yearAlert = await puppy.content('#body .alert')
			expect(yearAlert).toContain(`Não há movimentações para este ano.`)
	
			const valueOut = '19,86'
			await puppy.createMove(
				db, 'Move for Categories Out', '2021-08-08', valueOut,
				category, account, null, user
			)
	
			const valueIn = '27,03'
			await puppy.createMove(
				db, 'Move for Categories In', '2021-08-08', valueIn,
				category, null, account, user
			)
	
			await puppy.call(monthUrl)
	
			let monthTitle = await puppy.content("h1")
			expect(monthTitle).toContain(`Agosto de 2021`)
	
			let monthChartOut = await puppy.content('#body #container-out')
			monthChartOut = monthChartOut.replace('<tspan dy="14" x="5">​</tspan>', ' ')
			expect(monthChartOut).toContain(`${category}: ${valueOut}`)
	
			let monthChartIn = await puppy.content('#body #container-in')
			monthChartIn = monthChartIn.replace('<tspan dy="14" x="5">​</tspan>', ' ')
			expect(monthChartIn).toContain(`${category}: ${valueIn}`)
	
			await puppy.call(yearUrl)
	
			let yearTitle = await puppy.content("h1")
			expect(yearTitle).toContain(`2021`)
	
			let yearChartOut = await puppy.content('#body #container-out')
			yearChartOut = yearChartOut.replace('<tspan dy="14" x="5">​</tspan>', ' ')
			expect(yearChartOut).toContain(`${category}: ${valueOut}`)
	
			let yearChartIn = await puppy.content('#body #container-in')
			yearChartIn = yearChartIn.replace('<tspan dy="14" x="5">​</tspan>', ' ')
			expect(yearChartIn).toContain(`${category}: ${valueIn}`)
		})
	})

	test('Tips', async () => {
		user = await puppy.logon('reports-tips@dontflymoney.com')
		const tipContainer = '<aside class="tip">'

		const bodyInitial = await puppy.content('body')
		expect(bodyInitial).not.toContain(tipContainer)

		for(let i = 2; i < 27; i++) {
			await puppy.call()
			const bodyFor = await puppy.content('body')
			expect(bodyFor).not.toContain(tipContainer)
		}

		// show tip first time
		await puppy.call()
		const bodyTip = await puppy.content('body')
		expect(bodyTip).toContain(tipContainer)

		// repeat tip
		await puppy.call()
		const bodyRepeatTip = await puppy.content('body')
		expect(bodyRepeatTip).toContain(tipContainer)

		// dismiss tip
		await page.click('.tip .balloon .tip-close')
		await puppy.waitFor('.tip.hidden-tip');

		// third should not show
		await puppy.call()
		const bodyNoTip = await puppy.content('body')
		expect(bodyNoTip).not.toContain(tipContainer)

		// call some times again to reactivate tip
		for(let i = 2; i < 27; i++) {
			await puppy.call()
			const body = await puppy.content('body')
			expect(body).not.toContain(tipContainer)
		}

		// new contract
		await db.createContract();

		await puppy.call();

		const bodyAfterContract = await puppy.content('body')
		expect(bodyAfterContract).not.toContain(tipContainer)

	}, 60000)
})
