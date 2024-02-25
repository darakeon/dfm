const db = require('./db')
const puppy = require('./puppy')


describe('Ops', () => {
	beforeAll(async () => {
		await db.cleanupTickets()
		await puppy.call()

		const langItem = 'ul.nav li:nth-child(1)'
		const langButton = `${langItem} a`
		const chosenLang = db.language.toLowerCase()
		let flag = await puppy.content(langItem)

		while (flag.indexOf(chosenLang) < 0) {
			await page.click(langButton)
			await page.click('#language-modal #language-pt-br')
			flag = await puppy.content(langItem)
		}
	})

	test('Index', async () => {
		await puppy.call('Ops')

		const warning = await puppy.content('.alert')
		expect(warning).toContain('Ocorreu um erro interno.')
	})

	test('Code 404', async () => {
		await puppy.call('Ops/Code/404')

		const warning = await puppy.content('.alert')
		expect(warning).toContain('Página não encontrada.')
	})

	test('Code 500', async () => {
		await puppy.call('Ops/Code/500')

		const warning = await puppy.content('.alert')
		expect(warning).toContain('Ocorreu um erro interno. Por favor, entre em contato conosco.')
	})

	test('Code Unknown', async () => {
		await puppy.call('Ops/Code/100')

		const warning = await puppy.content('.alert')
		expect(warning).toContain('Ocorreu um erro interno. Por favor, entre em contato conosco.')
	})

	test('Error Uninvited', async () => {
		await puppy.call('Ops/Error/207')

		const warning = await puppy.content('.alert')
		expect(warning).toContain('Tchau!')
	})

	test('Do not hack', async () => {
		await puppy.call('Ops/Help')

		const warning = await puppy.content('.panel-body')
		expect(warning).toContain('https://github.com/darakeon/dfm')
	})
})