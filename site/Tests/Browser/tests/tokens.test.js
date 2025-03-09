const db = require('../helpers/db')
const puppy = require('../helpers/puppy')


describe('Users', () => {
	let email = 'tokens@dontflymoney.com'
	let user = {};
	let account = {};
	let category = {};

	beforeEach(async () => {
		await db.cleanupTickets()
		user = await puppy.logon(email)
		account = await db.createAccountIfNotExists('Account Token', user)
		category = await db.createCategoryIfNotExists('Category Token', user)
	})

	test('Token User Activate - by url', async () => {
		email = 'token_activate_user@dontflymoney.com'
		await db.createUserIfNotExists(email)
		const token = await db.createToken(email, 0)

		await puppy.call(`Tokens/UserVerification/${token}`)

		const message = await puppy.content('.alert')
		await expect(message).toContain('Verificação feita com sucesso.')
	})

	test('Token User Activate - by form', async () => {
		email = 'token_activate_user@dontflymoney.com'
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
		email = 'token_reset_password@dontflymoney.com'
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
		email = 'token_reset_password@dontflymoney.com'
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

	test('Token TFA Remove - by url', async () => {
		const secret = 'answer to the life universe and everything'
		await db.setSecret(user, secret)

		const token = await db.createToken(email, 4)

		await puppy.call(`Tokens/RemoveTFA/${token}`)

		const message = await puppy.content('.alert')
		await expect(message).toContain('Login mais Seguro desabilitado. O código não será mais exigido.')

		await puppy.call('')

		const warning = await puppy.content('#tfa-forgotten-warning')
		await expect(warning).toContain('Login mais Seguro desabilitado via token.')
		await expect(warning).toContain('Se não tiver sido uma ação sua,')
		await expect(warning).toContain('recomendamos reabilitar.')
	})

	test('Token TFA Remove - by form', async () => {
		const secret = 'answer to the life universe and everything'
		await db.setSecret(user, secret)

		const token = await db.createToken(email, 4)

		await puppy.call('Tokens')
		await puppy.waitFor('#body form')

		await page.type('#Token', token)
		await page.select('#SecurityAction', "RemoveTFA")
		await page.click('#body form button[type="submit"]')

		const message = await puppy.content('.alert')
		await expect(message).toContain('Login mais Seguro desabilitado. O código não será mais exigido.')

		await puppy.call('')

		const warning = await puppy.content('#tfa-forgotten-warning')
		await expect(warning).toContain('Login mais Seguro desabilitado via token.')
		await expect(warning).toContain('Se não tiver sido uma ação sua,')
		await expect(warning).toContain('recomendamos reabilitar.')
	})

	test('Token Disable', async () => {
		const token = await db.createToken(email, 1)

		await puppy.call(`Tokens/Disable/${token}`)

		const message = await puppy.content('.alert')
		await expect(message).toContain('Token desativado com sucesso.')
	})
})
