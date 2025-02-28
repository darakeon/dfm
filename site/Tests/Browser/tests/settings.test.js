const db = require('../helpers/db')
const puppy = require('../helpers/puppy')
const tfa = require('../helpers/tfa')
const { rand } = require('../helpers/math')


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

	test('Password - with TFA', async () => {
		const secret = 'answer to the life universe and everything'
		await db.setSecret(user, secret)
		await db.validateLastTFA(user)

		await puppy.call('')
		await page.click('#settings')
		await page.click('#settings_password', { visible: true })

		const header = await puppy.content('.panel .header')
		await expect(header).toContain('Senha')

		await page.type('#Password_CurrentPassword', db.password)
		await page.type('#Password_Password', db.password)
		await page.type('#Password_RetypePassword', db.password)
		await page.type('#Password_TFACode', tfa.code(secret))
		await page.click('#body form button[type="submit"]')

		const message = await puppy.content('.alert')
		await expect(message).toContain('Senha atualizada com sucesso.')
	})

	test('Password - lost TFA', async () => {
		const secret = 'answer to the life universe and everything'
		await db.setSecret(user, secret)
		await db.validateLastTFA(user)

		await puppy.call('')
		await page.click('#settings')
		await page.click('#settings_password', { visible: true })

		const header = await puppy.content('.panel .header')
		await expect(header).toContain('Senha')

		await page.click('#body form a.btn-warning')

		await puppy.waitFor('#body form')
		await expect(page.title()).resolves.toMatch('DfM - Remover Login mais Seguro')
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
		await db.clearSecret(user)

		await puppy.call('')
		await page.click('#settings')
		await page.click('#settings_tfa', { visible: true })

		const header = await puppy.content('.panel .header')
		await expect(header).toContain('Login mais Seguro')

		const qrCodeUrl = await puppy.property('#qrcode', e => e.getAttribute('data-url'))
		expect(qrCodeUrl).toMatch(/otpauth:/)

		await puppy.submit(`/Settings/TFA`)

		const message = await puppy.content('.alert')
		await expect(message).toContain('Código inválido')
	})

	test('TFA - Remove using code', async () => {
		const secret = 'answer to the life universe and everything'
		await db.setSecret(user, secret)
		await db.validateLastTFA(user)

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

		const qrCodeUrl = await puppy.property('#qrcode', e => e.getAttribute('data-url'))
		expect(qrCodeUrl).toMatch(/otpauth:/)
	})

	test('TFA - Use as password', async () => {
		const secret = 'answer to the life universe and everything'
		await db.setSecret(user, secret)
		await db.validateLastTFA(user)

		await puppy.call('Settings/TFA')
		await puppy.waitFor('#body form')
		await expect(page.title()).resolves.toMatch('DfM - Login mais Seguro')

		await page.click('#UseTFAPassword')

		await page.type('#TFA_Password', db.password)
		await page.type('#TFA_Code', tfa.code(secret))
		await page.click('#body form button[type="submit"]')

		const successMessage = await puppy.content('.alert')
		expect(successMessage).toContain(
			`Configuração de Login mais Seguro alterada com sucesso!`
		)

		const useTFA = await puppy.property('#UseTFAPassword', n => n.checked)
		expect(useTFA).toBe(true)
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
