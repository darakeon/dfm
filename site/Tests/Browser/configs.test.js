const puppy = require('./puppy.js')

describe('Configs', () => {
	let user = {};

	beforeAll(async () => {
		user = await puppy.logon('configs@dontflymoney.com')
	})

	test('Visit', async () => {
		await puppy.call('Configs/Config')

		const header = await puppy.content('.panel .header')
		await expect(header).toContain('Configurações')
	})

	test('Index', async () => {
		await puppy.call('')
		await page.click('#settings')
		await page.click('#settings_main', { visible: true })

		const header = await puppy.content('.panel .header')
		await expect(header).toContain('Configurações')

		await puppy.submit(`/Configs`)

		const message = await puppy.content('.alert')
		await expect(message).toContain('Configurações alteradas')
	})

	test('Password', async () => {
		await puppy.call('')
		await page.click('#settings')
		await page.click('#settings_password', { visible: true })

		const header = await puppy.content('.panel .header')
		await expect(header).toContain('Senha')

		await puppy.submit(`/Configs/Password`)

		const message = await puppy.content('.alert')
		await expect(message).toContain('A senha não pode ser em branco')
	})
})
