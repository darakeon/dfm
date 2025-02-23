const db = require('./db')
const puppy = require('./puppy')
const { rand } = require('./utils')


describe('Settings', () => {
	let user = {};

	beforeAll(async () => {
		user = await puppy.logon(`settings${rand()}@dontflymoney.com`)
	})

	test('Index', async () => {
		await puppy.call('')
		await page.click('#settings')
		await page.click('#settings_main', { visible: true })

		const header = await puppy.content('.panel .header')
		await expect(header).toContain('Configurações')

		await puppy.submit(`/Settings`)

		const message = await puppy.content('.alert')
		await expect(message).toContain('Configurações alteradas')
	})

	test('Password', async () => {
		await puppy.call('')
		await page.click('#settings')
		await page.click('#settings_password', { visible: true })

		const header = await puppy.content('.panel .header')
		await expect(header).toContain('Senha')

		await puppy.submit(`/Settings/Password`)

		const message = await puppy.content('.alert')
		await expect(message).toContain('A senha não pode ser em branco')
	})

	test('E-mail', async () => {
		await puppy.call('')
		await page.click('#settings')
		await page.click('#settings_email', { visible: true })

		const header = await puppy.content('.panel .header')
		await expect(header).toContain('E-mail')

		await puppy.submit(`/Settings/Email`)

		const message = await puppy.content('.alert')
		await expect(message).toContain('Senha errada para acesso atual')
	})

	test('Theme', async () => {
		await puppy.call('')
		await page.click('#settings')
		await page.click('#settings_theme', { visible: true })

		const header = await puppy.content('.panel .header')
		await expect(header).toContain('Cores')

		await puppy.submit(`/Settings/Theme`)

		const message = await puppy.content('.alert')
		await expect(message).toContain('Configurações alteradas')
	})

	test('TFA', async () => {
		await puppy.call('')
		await page.click('#settings')
		await page.click('#settings_tfa', { visible: true })

		const header = await puppy.content('.panel .header')
		await expect(header).toContain('Login mais Seguro')

		const qrCode = await puppy.content('#qrcode')
		expect(qrCode).not.toBeFalsy()

		await puppy.submit(`/Settings/TFA`)

		const message = await puppy.content('.alert')
		await expect(message).toContain('Código inválido')
	})

	test('TFA - Remove using code', async () => {
		const email = `tfa${rand()}@dontflymoney.com`
		await puppy.logon(email)

		const secret = 'answer to the life universe and everything'
		await db.setSecret(email, secret)
		await db.validateLastTFA(email)

		await puppy.call('Settings/TFA')
		await puppy.waitFor('#body form')
		await expect(page.title()).resolves.toMatch('DfM - Login mais Seguro')

		await page.click('#UseTFA')

		await page.type('#TFA_Password', db.password)
		await page.type('#TFA_Code', tfa.code(secret))
		await page.click('#body form button[type="submit"]')

		const successMessage = await puppy.content('.alert')
		expect(successMessage).toContain(
			`Configuração de Login mais Seguro alterada com sucesso!`
		)

		const qrCode = await puppy.content('#qrcode')
		expect(qrCode).not.toBeFalsy()
	})

	test('TFA - Use as password', async () => {
		const email = `tfa${rand()}@dontflymoney.com`
		await puppy.logon(email)

		const secret = 'answer to the life universe and everything'
		await db.setSecret(email, secret)
		await db.validateLastTFA(email)

		await puppy.call('Settings/TFA')
		await puppy.waitFor('#body form')
		await expect(page.title()).resolves.toMatch('DfM - Login mais Seguro')

		await page.click('#UseTFA')

		await page.type('#TFA_Password', db.password)
		await page.type('#TFA_Code', tfa.code(secret))
		await page.click('#body form button[type="submit"]')

		const successMessage = await puppy.content('.alert')
		expect(successMessage).toContain(
			`Configuração de Login mais Seguro alterada com sucesso!`
		)

		const qrCode = await puppy.content('#qrcode')
		expect(qrCode).toBeFalsy()

		const useTFA = await puppy.waitFor('#UseTFA', { checked: true })
		expect(useTFA).not.toBeFalsy()
	})

	test('Misc', async () => {
		await puppy.call('')
		await page.click('#settings')
		await page.click('#settings_misc', { visible: true })

		const header = await puppy.content('.panel .header')
		await expect(header).toContain('Imagem de segurança')

		await puppy.imageLog('misc-original', true)

		await puppy.submit(`/Settings/Misc`)

		const alertError = await puppy.content('.alert')
		await expect(alertError).toContain('Senha errada para acesso atual')

		await puppy.imageLog('misc-wrong-pass', true)

		await page.type('#Password', db.password)
		await puppy.submit(`/Settings/Misc`)

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

		await puppy.submit(`/Settings/Wipe`)

		const alertError = await puppy.content('.alert')
		await expect(alertError).toContain('Senha errada para acesso atual')

		await page.type('#Password', db.password)
		await puppy.submit(`/Settings/Wipe`)

		const alertSuccess = await puppy.content('.alert')
		await expect(alertSuccess).toContain('Agradecemos por ter usado nosso sistema.')

		const menu = await puppy.content('.navbar-nav')
		await expect(menu).toContain('/Users/LogOn')
	})
})
