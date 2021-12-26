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

	test('Misc', async () => {
		await puppy.call('')
		await page.click('#settings')
		await page.click('#settings_misc', { visible: true })

		const header = await puppy.content('.panel .header')
		await expect(header).toContain('Imagem de segurança')

		await puppy.imageLog('misc-original', true)

		await puppy.submit(`/Configs/Misc`)

		const alertError = await puppy.content('.alert')
		await expect(alertError).toContain('Senha errada para acesso atual')

		await puppy.imageLog('misc-wrong-pass', true)

		await page.type('#Password', 'password')
		await puppy.submit(`/Configs/Misc`)

		const alertSuccess = await puppy.content('.alert')
		await expect(alertSuccess).toContain('Configurações alteradas')

		await puppy.imageLog('misc-changed', true)
	})

	test('Wipe', async () => {
		await puppy.call('')
		await page.click('#settings')
		await page.click('#settings_wipe', { visible: true })

		const header = await puppy.content('.panel .header')
		await expect(header).toContain('Remover meus dados')

		await puppy.submit(`/Configs/Wipe`)

		const alertError = await puppy.content('.alert')
		await expect(alertError).toContain('Senha errada para acesso atual')

		await page.type('#Password', 'password')
		await puppy.submit(`/Configs/Wipe`)

		const alertSuccess = await puppy.content('.alert')
		await expect(alertSuccess).toContain('Agradecemos por ter usado nosso sistema.')

		const menu = await puppy.content('.navbar-nav')
		await expect(menu).toContain('/Users/LogOn')
	})
})
