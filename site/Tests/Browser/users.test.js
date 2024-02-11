const fs = require('fs/promises');

const db = require('./db')
const puppy = require('./puppy')
const tfa = require('./tfa')


describe('Users', () => {
	beforeEach(async () => {
		await db.cleanupTickets()
	})

	test('SignUp', async () => {
		await puppy.call('Users/SignUp')
		await puppy.waitFor('#body form')

		await page.type('#Email', 'signup@dontflymoney.com')
		await page.type('#Password', db.password)
		await page.type('#RetypePassword', db.password)
		await page.click('#Accept')
		await puppy.submit('/Users/SignUp')

		await expect(page.title()).resolves.toMatch('DfM - Contas')
	})

	test('SignUp - contract modal', async () => {
		await puppy.call('Users/SignUp')
		await puppy.waitFor('#body form')

		await puppy.waitFor('#contract-modal', { visible: false })

		const termsLink = '#body form a[role="button"]'
		await page.click(termsLink)

		await puppy.waitFor('#contract-modal', { visible: true })
	})

	test('Not active first day', async () => {
		const email = 'user_first_day@dontflymoney.com'
		await puppy.logon(email, { creation: 0 })

		await puppy.call()

		const body = await puppy.content('#body')
		expect(body).not.toContain('activate-warning')
	})

	test('Not active second day', async () => {
		const email = 'user_second_day@dontflymoney.com'
		await puppy.logon(email, { creation: -1 })

		await puppy.call()

		const body = await puppy.content('#body')
		expect(body).not.toContain('activate-warning')
	})

	test('Not active third day', async () => {
		const email = 'user_third_day@dontflymoney.com'
		await puppy.logon(email, { creation: -2 })

		await puppy.call()

		const body = await puppy.content('#body')
		expect(body).not.toContain('activate-warning')
	})

	test('Not active fourth day', async () => {
		const email = 'user_fourth_day@dontflymoney.com'
		await puppy.logon(email, { creation: -3 })

		await puppy.call()

		await page.click('.activate-warning-low')
		await puppy.waitFor('#activate-warning.modal', { visible: true })

		await puppy.submit('/Users/Verification')
		const alert = await puppy.content('.alert')
		expect(alert).toContain('Código enviado')
	})

	test('Not active fifth day', async () => {
		const email = 'user_fifth_day@dontflymoney.com'
		await puppy.logon(email, { creation: -4 })

		await puppy.call()

		await page.click('.activate-warning-low')
		await puppy.waitFor('#activate-warning.modal', { visible: true })

		await puppy.submit('/Users/Verification')
		const alert = await puppy.content('.alert')
		expect(alert).toContain('Código enviado')
	})

	test('Not active sixth day', async () => {
		const email = 'user_sixth_day@dontflymoney.com'
		await puppy.logon(email, { creation: -5 })

		await puppy.call()

		await page.click('.activate-warning-high')
		await puppy.waitFor('#activate-warning.modal', { visible: true })

		await puppy.submit('/Users/Verification')
		const alert = await puppy.content('.alert')
		expect(alert).toContain('Código enviado')
	})

	test('Not active seventh day', async () => {
		const email = 'user_seventh_day@dontflymoney.com'
		await puppy.logon(email, { creation: -6 })

		await puppy.call()

		await page.click('.activate-warning-high')
		await puppy.waitFor('#activate-warning.modal', { visible: true })

		await puppy.submit('/Users/Verification')
		const alert = await puppy.content('.alert')
		expect(alert).toContain('Código enviado')
	})

	test('Not active eighth day', async () => {
		const email = 'user_eighth_day@dontflymoney.com'
		await puppy.logon(email, { creation: -7 })

		const alert = await puppy.content('.alert')
		expect(alert).toContain('Seu acesso ao sistema não foi ativado ainda.')
	})

	test('Logon', async () => {
		const email = 'logon_success@dontflymoney.com'
		await db.createUserIfNotExists(email, {active:true})

		await puppy.call('Users/Logon')
		await puppy.waitFor('#body form')

		await page.type('#Email', email)
		await page.type('#Password', db.password)
		await page.click('#RememberMe')
		await puppy.submit('/Users/LogOn')

		await expect(page.title()).resolves.toMatch('DfM - Contas')
	})

	test('Logon not active', async () => {
		const email = 'logon_not_active@dontflymoney.com'
		await db.createUserIfNotExists(email, { creation: -27 })

		await puppy.call('Users/Logon')
		await puppy.waitFor('#body form')

		await page.type('#Email', email)
		await page.type('#Password', db.password)
		await page.click('#RememberMe')
		await page.click('#body form button[type="submit"]')

		const message = await puppy.content('.alert')
		await expect(message).toContain('Seu acesso ao sistema não foi ativado ainda.')
	})

	test('Forgot Password', async () => {
		const email = 'forgot_password@dontflymoney.com'
		await db.createUserIfNotExists(email)

		await puppy.call('Users/ForgotPassword')
		await puppy.waitFor('#body form')

		await page.type('#Email', email)
		await page.click('#body form button[type="submit"]')
		const message = await puppy.content('.alert')
		await expect(message).toContain('Se existir este e-mail no sistema, você receberá as instruções para prosseguir.')
	})

	test('Forgot Password - not existente', async () => {
		const email = 'not_existente@dontflymoney.com'

		await puppy.call('Users/ForgotPassword')
		await puppy.waitFor('#body form')

		await page.type('#Email', email)
		await page.click('#body form button[type="submit"]')

		const message = await puppy.content('.alert')
		await expect(message).toContain('Se existir este e-mail no sistema, você receberá as instruções para prosseguir.')
	})

	test('LogOff', async () => {
		const email = 'logoff@dontflymoney.com'
		await db.createUserIfNotExists(email, {active:true})

		await puppy.call('Users/Logon')
		await puppy.waitFor('#body form')

		await page.type('#Email', email)
		await page.type('#Password', db.password)
		await page.click('#RememberMe')
		await page.click('#body form button[type="submit"]')

		await puppy.waitFor('.nav')
		await page.click('.nav form button[type="submit"]')

		const nav = await puppy.content('.nav')
		await expect(nav).not.toContain('form')
		await expect(nav).toContain('Sign up')
	})

	test('End Wizard', async () => {
		const email = 'end_wizard@dontflymoney.com'
		await db.createUserIfNotExists(email, {active:true, wizard:true})

		await puppy.call('Users/Logon')
		await puppy.waitFor('#body form')

		await page.type('#Email', email)
		await page.type('#Password', db.password)
		await page.click('#RememberMe')
		await page.click('#body form button[type="submit"]')

		await puppy.waitFor('.panel')
		await page.click('.panel form button[type="submit"]')

		const panelBody = await puppy.content('.panel-body')
		await expect(panelBody).toContain('Agradecemos por ler as mensagens do Tutorial.')
	})

	test('Contract', async () => {
		const email = 'contract@dontflymoney.com'
		await db.createUserIfNotExists(email, {active:true, wizard:true})

		await puppy.call('Users/Contract')
		await puppy.waitFor('#body form')

		const acceptLabel = await puppy.content('.panel-heading')
		await expect(acceptLabel).toContain('Termos de Uso')
	})

	test('TFA', async () => {
		const email = 'tfa@dontflymoney.com'
		const user = await puppy.logon(email)

		const secret = 'answer to the life universe and everything'
		await db.setSecret(email, secret)

		await puppy.call('Accounts')
		await puppy.waitFor('#body form')
		await expect(page.title()).resolves.toMatch('DfM - Login mais seguro')

		await page.click('#body form a.btn-info')
		await puppy.waitFor('#contact-modal', { visible: true })
		await page.click('#contact-modal .close')
		await puppy.waitFor('#contact-modal', { visible: false })

		await page.type('#Code', tfa.code(secret))
		await page.click('#body form button[type="submit"]')

		await puppy.waitFor('#body')
		await expect(page.title()).resolves.toMatch('DfM - Contas')
	})

	test('SendWipedData', async () => {
		email = 'deleted@dontflymoney.com'
		await db.deleteWipe(email)

		await puppy.call()
		await puppy.waitFor('#body')

		await page.click('#dfm-navigation ul li:last-child')
		await puppy.waitFor('#body')

		await expect(page.title()).resolves.toMatch('DfM - Dados removidos')

		await page.type('#Email', email)
		await page.type('#Password', db.password)
		await puppy.submit('/Users/SendWipedData')

		await puppy.waitFor('#body')
		await expect(page.title()).resolves.toMatch('DfM - Dados removidos')

		const warningNotFound = await puppy.content('.alert')
		expect(warningNotFound).toContain(
			`E-mail não encontrado entre dados excluídos de pessoas`
		)

		const s3 = await db.createWipe(email)
		const path = `../../../outputs/s3/${s3}`
		await fs.writeFile(path, 'test')

		await page.type('#Password', db.password)
		await puppy.submit('/Users/SendWipedData')

		await puppy.waitFor('#body')
		await expect(page.title()).resolves.toMatch('DfM - Dados removidos')

		const warningNoFile = await puppy.content('.alert')
		expect(warningNoFile).toContain(
			`Arquivo com movimentações enviado com sucesso`
		)
	})
})
