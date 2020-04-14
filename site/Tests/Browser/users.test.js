const puppeteer = require('puppeteer')
const fs = require('fs')
const db = require('./db')
const puppy = require('./puppy')

describe('Users', () => {
	beforeEach(async () => {
		await db.cleanupTickets()
	})

	test.only('SignUp', async () => {
		await puppy.call('Users/SignUp')
		await page.waitForSelector('#body form')

		await page.type('#Email', 'signup@dontflymoney.com')
		await page.type('#Password', db.password.plain)
		await page.type('#RetypePassword', db.password.plain)
		await page.click('#Accept')
		await page.click('#body form button[type="submit"]')

		const message = await puppy.content('.alert')
		await expect(message).toContain('Cadastro efetuado com sucesso.')
	})

	test('Logon', async () => {
		const email = 'logon_success@dontflymoney.com'
		await db.createUserIfNotExists(email, 1)

		await puppy.call('Users/Logon')
		await page.waitForSelector('#body form')

		await page.type('#Email', email)
		await page.type('#Password', db.password.plain)
		await page.click('#RememberMe')
		await page.click('#body form button[type="submit"]')

		await expect(page.title()).resolves.toMatch('DfM - Contas')
	})

	test('Logon not active', async () => {
		const email = 'logon_not_active@dontflymoney.com'
		await db.createUserIfNotExists(email)

		await puppy.call('Users/Logon')
		await page.waitForSelector('#body form')

		await page.type('#Email', email)
		await page.type('#Password', db.password.plain)
		await page.click('#RememberMe')
		await page.click('#body form button[type="submit"]')

		const message = await puppy.content('.alert')
		await expect(message).toContain('Seu acesso ao sistema não foi ativado ainda.')
	})

	test('Token User Activate - by url', async () => {
		const email = 'token_activate_user@dontflymoney.com'
		await db.createUserIfNotExists(email)
		const token = await db.createToken(email, 0)

		await puppy.call(`Tokens/UserVerification/${token}`)

		const message = await puppy.content('.alert')
		await expect(message).toContain('Verifição feita com sucesso.')
	})

	test('Token User Activate - by form', async () => {
		const email = 'token_activate_user@dontflymoney.com'
		await db.createUserIfNotExists(email)
		const token = await db.createToken(email, 0)

		await puppy.call('Tokens')
		await page.waitForSelector('#body form')

		await page.type('#Token', token)
		await page.click('#body form button[type="submit"]')

		const message = await puppy.content('.alert')
		await expect(message).toContain('Verifição feita com sucesso.')
	})

	test('Forgot Password', async () => {
		const email = 'forgot_password@dontflymoney.com'
		await db.createUserIfNotExists(email)

		await puppy.call('Users/ForgotPassword')
		await page.waitForSelector('#body form')

		await page.type('#Email', email)
		await page.click('#body form button[type="submit"]')
		const message = await puppy.content('.alert')
		await expect(message).toContain('Se existir este e-mail no sistema, você receberá as instruções para prosseguir.')
	})

	test('Forgot Password - not existente', async () => {
		const email = 'not_existente@dontflymoney.com'

		await puppy.call('Users/ForgotPassword')
		await page.waitForSelector('#body form')

		await page.type('#Email', email)
		await page.click('#body form button[type="submit"]')

		const message = await puppy.content('.alert')
		await expect(message).toContain('Se existir este e-mail no sistema, você receberá as instruções para prosseguir.')
	})

	test('Token Password Reset - by url', async () => {
		const email = 'token_reset_password@dontflymoney.com'
		await db.createUserIfNotExists(email, 1)
		const token = await db.createToken(email, 1)

		await puppy.call(`Tokens/PasswordReset/${token}`)
		await page.waitForSelector('#body form')

		await page.type('#Password', db.password.plain)
		await page.type('#RetypePassword', db.password.plain)
		await page.click('#body form button[type="submit"]')

		const message = await puppy.content('.alert')
		await expect(message).toContain('Senha redefinida com sucesso.')
	})

	test('Token Password Reset - by form', async () => {
		const email = 'token_reset_password@dontflymoney.com'
		await db.createUserIfNotExists(email, 1)
		const token = await db.createToken(email, 1)

		await puppy.call('Tokens')
		await page.waitForSelector('#body form')

		await page.type('#Token', token)
		await page.select('#SecurityAction', "PasswordReset")
		await page.click('#body form button[type="submit"]')
		await page.waitForSelector('#body form')

		await page.type('#Password', db.password.plain)
		await page.type('#RetypePassword', db.password.plain)
		await page.click('#body form button[type="submit"]')

		const message = await puppy.content('.alert')
		await expect(message).toContain('Senha redefinida com sucesso.')
	})

	test('Token Disable', async () => {
		const email = 'token_disable@dontflymoney.com'
		await db.createUserIfNotExists(email, 1)
		const token = await db.createToken(email, 1)

		await puppy.call(`Tokens/Disable/${token}`)

		const message = await puppy.content('.alert')
		await expect(message).toContain('Token desativado com sucesso.')
	})

	test('LogOff', async () => {
		const email = 'logoff@dontflymoney.com'
		await db.createUserIfNotExists(email, 1)

		await puppy.call('Users/Logon')
		await page.waitForSelector('#body form')

		await page.type('#Email', email)
		await page.type('#Password', db.password.plain)
		await page.click('#RememberMe')
		await page.click('#body form button[type="submit"]')

		await page.waitForSelector('.nav')
		await page.click('.nav form button[type="submit"]')

		const nav = await puppy.content('.nav')
		await expect(nav).not.toContain('form')
		await expect(nav).toContain('Cadastrar-se')
	})

	test('End Wizard', async () => {
		const email = 'end_wizard@dontflymoney.com'
		await db.createUserIfNotExists(email, 1, 1)

		await puppy.call('Users/Logon')
		await page.waitForSelector('#body form')

		await page.type('#Email', email)
		await page.type('#Password', db.password.plain)
		await page.click('#RememberMe')
		await page.click('#body form button[type="submit"]')

		await page.waitForSelector('.panel')
		await page.click('.panel form button[type="submit"]')

		const panelBody = await puppy.content('.panel-body')
		await expect(panelBody).toContain('Agradecemos por ler as mensagens do Tutorial.')
	})
})
