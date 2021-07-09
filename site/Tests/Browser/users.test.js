const db = require('./db')
const puppy = require('./puppy')
const tfa = require('./tfa')

describe('Users', () => {
	beforeEach(async () => {
		await db.cleanupTickets()
	})

	test('SignUp', async () => {
		await puppy.call('Users/SignUp')
		await page.waitForSelector('#body form')

		await page.type('#Email', 'signup@dontflymoney.com')
		await page.type('#Password', db.password.plain)
		await page.type('#RetypePassword', db.password.plain)
		await page.click('#Accept')
		await puppy.submit('/Users/SignUp')

		await expect(page.title()).resolves.toMatch('DfM - Contas')

		await page.click('.activate-warning')
		await page.waitForSelector('#activate-warning.modal', { visible: true })

		await puppy.submit('/Users/Verification')
		const alert = await puppy.content('.alert')
		expect(alert).toContain('Código enviado')
	})

	test('SignUp - contract modal', async () => {
		await puppy.call('Users/SignUp')
		await page.waitForSelector('#body form')

		await page.waitForSelector('#contract-modal', { visible: false })

		const termsLink = '#body form a[role="button"]'
		await page.click(termsLink)

		await page.waitForSelector('#contract-modal', { visible: true })
	})

	test('Logon', async () => {
		const email = 'logon_success@dontflymoney.com'
		await db.createUserIfNotExists(email, {active:true})

		await puppy.call('Users/Logon')
		await page.waitForSelector('#body form')

		await page.type('#Email', email)
		await page.type('#Password', db.password.plain)
		await page.click('#RememberMe')
		await puppy.submit('/Users/LogOn')

		await expect(page.title()).resolves.toMatch('DfM - Contas')
	})

	test('Logon not active', async () => {
		const email = 'logon_not_active@dontflymoney.com'
		await db.createUserIfNotExists(email, { creation: -27 })

		await puppy.call('Users/Logon')
		await page.waitForSelector('#body form')

		await page.type('#Email', email)
		await page.type('#Password', db.password.plain)
		await page.click('#RememberMe')
		await page.click('#body form button[type="submit"]')

		const message = await puppy.content('.alert')
		await expect(message).toContain('Seu acesso ao sistema não foi ativado ainda.')
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

	test('LogOff', async () => {
		const email = 'logoff@dontflymoney.com'
		await db.createUserIfNotExists(email, {active:true})

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
		await db.createUserIfNotExists(email, {active:true, wizard:true})

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

	test('Contract', async () => {
		const email = 'contract@dontflymoney.com'
		await db.createUserIfNotExists(email, {active:true, wizard:true})

		await puppy.call('Users/Contract')
		await page.waitForSelector('#body form')

		const acceptLabel = await puppy.content('.panel-heading')
		await expect(acceptLabel).toContain('Termos de Uso')
	})

	test('TFA', async () => {
		const email = 'tfa@dontflymoney.com'
		const user = await puppy.logon(email)

		const secret = 'answer to the life universe and everything'
		await db.setSecret(email, secret)

		await puppy.call('Accounts')
		await page.waitForSelector('#body form')
		await expect(page.title()).resolves.toMatch('DfM - Login mais seguro')

		await page.click('#body form a.btn-info')
		await page.waitForSelector('#contact-modal', { visible: true })
		await page.click('#contact-modal .close')
		await page.waitForSelector('#contact-modal', { visible: false })

		await page.type('#Code', tfa.code(secret))
		await page.click('#body form button[type="submit"]')

		await page.waitForSelector('#body')
		await expect(page.title()).resolves.toMatch('DfM - Contas')
	})
})
