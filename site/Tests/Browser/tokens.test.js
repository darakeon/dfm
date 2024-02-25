const db = require('./db')
const puppy = require('./puppy')


describe('Users', () => {
	let user = {};
	let account = {};
	let category = {};

	beforeEach(async () => {
		await db.cleanupTickets()
		user = await puppy.logon('tokens@dontflymoney.com')
		account = await db.createAccountIfNotExists('Account Token', user)
		category = await db.createCategoryIfNotExists('Category Token', user)
	})

	test('Token User Activate - by url', async () => {
		const email = 'token_activate_user@dontflymoney.com'
		await db.createUserIfNotExists(email)
		const token = await db.createToken(email, 0)

		await puppy.call(`Tokens/UserVerification/${token}`)

		const message = await puppy.content('.alert')
		await expect(message).toContain('Verificação feita com sucesso.')
	})

	test('Token User Activate - by form', async () => {
		const email = 'token_activate_user@dontflymoney.com'
		await db.createUserIfNotExists(email)
		const token = await db.createToken(email, 0)

		await puppy.call('Tokens')
		await puppy.waitFor('#body form')

		await page.type('#Token', token)
		await page.click('#body form button[type="submit"]')

		const message = await puppy.content('.alert')
		await expect(message).toContain('Verificação feita com sucesso.')
	})

	test('Token Password Reset - by url', async () => {
		const email = 'token_reset_password@dontflymoney.com'
		await db.createUserIfNotExists(email, {active:true})
		const token = await db.createToken(email, 1)

		await puppy.call(`Tokens/PasswordReset/${token}`)
		await puppy.waitFor('#body form')

		await page.type('#Password', db.password)
		await page.type('#RetypePassword', db.password)
		await page.click('#body form button[type="submit"]')

		const message = await puppy.content('.alert')
		await expect(message).toContain('Senha redefinida com sucesso.')
	})

	test('Token Password Reset - by form', async () => {
		const email = 'token_reset_password@dontflymoney.com'
		await db.createUserIfNotExists(email, {active:true})
		const token = await db.createToken(email, 1)

		await puppy.call('Tokens')
		await puppy.waitFor('#body form')

		await page.type('#Token', token)
		await page.select('#SecurityAction', "PasswordReset")
		await page.click('#body form button[type="submit"]')
		await puppy.waitFor('#body form')

		await page.type('#Password', db.password)
		await page.type('#RetypePassword', db.password)
		await page.click('#body form button[type="submit"]')

		const message = await puppy.content('.alert')
		await expect(message).toContain('Senha redefinida com sucesso.')
	})
	
	test('Token Unsubscribe Move Mail - by url', async () => {
		await puppy.createMove(
			db,
			'Move Token', '2020-12-03', '1,00',
			category, account, null
		)

		const token = await db.getLastUnsubscribeMoveMailToken(user)

		await puppy.call(`Tokens/UnsubscribeMoveMail/${token}`)

		const message = await puppy.content('.alert')
		await expect(message).toContain('E-mails de Notificação de Movimentação cancelados com sucesso.')
		await expect(message).toContain('Caso queira reativar, vá até as Configurações.')
	})

	test('Token Unsubscribe Move Mail - by form', async () => {
		const id = await puppy.createMove(
			db,
			'Move Token', '2020-12-03', '1,00',
			category, account, null
		)
		
		const token = await db.getLastUnsubscribeMoveMailToken(user)

		await puppy.call('Tokens')
		await puppy.waitFor('#body form')

		await page.type('#Token', token)
		await page.select('#SecurityAction', "UnsubscribeMoveMail")
		await page.click('#body form button[type="submit"]')

		const message = await puppy.content('.alert')
		await expect(message).toContain('E-mails de Notificação de Movimentação cancelados com sucesso.')
		await expect(message).toContain('Caso queira reativar, vá até as Configurações.')
	})

	test('Token Disable', async () => {
		const email = 'token_disable@dontflymoney.com'
		await db.createUserIfNotExists(email, {active:true})
		const token = await db.createToken(email, 1)

		await puppy.call(`Tokens/Disable/${token}`)

		const message = await puppy.content('.alert')
		await expect(message).toContain('Token desativado com sucesso.')
	})
})
