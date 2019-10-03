const puppeteer = require('puppeteer')
const fs = require('fs')
const db = require('./db')

describe('Users', () => {
	beforeAll(async () => {
		await db.cleanup()
	}, 30000)

	beforeEach(async () => {
		await db.cleanupTickets()
	})

	test('SignUp', async () => {
		await page.goto('http://localhost:2709/Users/SignUp')
		await page.waitForSelector('#body form')

		await page.type('#Email', 'signup@dontflymoney.com')
		await page.type('#Password', db.password.plain)
		await page.type('#RetypePassword', db.password.plain)
		await page.click('#Accept')
		await page.click('#body form button[type="submit"]')

		const alertBox = '.alert'
		await page.waitForSelector(alertBox)

		const message = await page.$eval(alertBox, n => n.innerHTML)
		await expect(message).toContain('Cadastro efetuado com sucesso.')
	})

	test('Logon', async () => {
		const email = 'logon_success@dontflymoney.com'
		await db.createUserIfNotExists(email, 1)

		await page.goto('http://localhost:2709/Users/Logon')
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

		await page.goto('http://localhost:2709/Users/Logon')
		await page.waitForSelector('#body form')

		await page.type('#Email', email)
		await page.type('#Password', db.password.plain)
		await page.click('#RememberMe')
		await page.click('#body form button[type="submit"]')

		const alertBox = '.alert'
		await page.waitForSelector(alertBox)

		const message = await page.$eval(alertBox, n => n.innerHTML)
		await expect(message).toContain('Seu usuário não foi ativado.')
	})

	test('Token User Activate - by url', async () => {
		const email = 'token_activate_user@dontflymoney.com'
		await db.createUserIfNotExists(email)
		const token = await db.createToken(email, 0)

		await page.goto(`http://localhost:2709/Tokens/UserVerification/${token}`)

		const alertBox = '.alert'
		await page.waitForSelector(alertBox)
		const message = await page.$eval(alertBox, n => n.innerHTML)
		await expect(message).toContain('Usuário verificado com sucesso.')
	})

	test('Token User Activate - by form', async () => {
		const email = 'token_activate_user@dontflymoney.com'
		await db.createUserIfNotExists(email)
		const token = await db.createToken(email, 0)

		await page.goto('http://localhost:2709/Tokens')
		await page.waitForSelector('#body form')

		await page.type('#Token', token)
		await page.click('#body form button[type="submit"]')

		const alertBox = '.alert'
		await page.waitForSelector(alertBox)
		const message = await page.$eval(alertBox, n => n.innerHTML)
		await expect(message).toContain('Usuário verificado com sucesso.')
	})

	test('Forgot Password', async () => {
		const email = 'forgot_password@dontflymoney.com'
		await db.createUserIfNotExists(email)

		await page.goto('http://localhost:2709/Users/ForgotPassword')
		await page.waitForSelector('#body form')

		await page.type('#Email', email)
		await page.click('#body form button[type="submit"]')

		const alertBox = '.alert'
		await page.waitForSelector(alertBox)
		const message = await page.$eval(alertBox, n => n.innerHTML)
		await expect(message).toContain('Se existir este e-mail no sistema, você receberá um e-mail.')
	})

	test('Forgot Password - not existente', async () => {
		const email = 'not_existente@dontflymoney.com'

		await page.goto('http://localhost:2709/Users/ForgotPassword')
		await page.waitForSelector('#body form')

		await page.type('#Email', email)
		await page.click('#body form button[type="submit"]')

		const alertBox = '.alert'
		await page.waitForSelector(alertBox)
		const message = await page.$eval(alertBox, n => n.innerHTML)
		await expect(message).toContain('Se existir este e-mail no sistema, você receberá um e-mail.')
	})
})
