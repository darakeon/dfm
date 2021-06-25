const puppy = require('./puppy.js')

describe('Configs', () => {
	let user = {};

	beforeAll(async () => {
		user = await puppy.logon('configs@dontflymoney.com')
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

	test('E-mail', async () => {
		await puppy.call('')
		await page.click('#settings')
		await page.click('#settings_email', { visible: true })

		const header = await puppy.content('.panel .header')
		await expect(header).toContain('E-mail')

		await puppy.submit(`/Configs/Email`)

		const message = await puppy.content('.alert')
		await expect(message).toContain('Senha errada para acesso atual')
	})

	test('Theme', async () => {
		await puppy.call('')
		await page.click('#settings')
		await page.click('#settings_theme', { visible: true })

		const header = await puppy.content('.panel .header')
		await expect(header).toContain('Cores')

		await puppy.submit(`/Configs/Theme`)

		const message = await puppy.content('.alert')
		await expect(message).toContain('Configurações alteradas')
	})

	test('TFA', async () => {
		await puppy.call('')
		await page.click('#settings')
		await page.click('#settings_tfa', { visible: true })

		const header = await puppy.content('.panel .header')
		await expect(header).toContain('Login mais seguro')

		await puppy.submit(`/Configs/TFA`)

		const message = await puppy.content('.alert')
		await expect(message).toContain('Código inválido')
	})
})
