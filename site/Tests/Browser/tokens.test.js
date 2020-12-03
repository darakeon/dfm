const puppeteer = require('puppeteer')
const fs = require('fs')
const db = require('./db')
const puppy = require('./puppy')

describe('Users', () => {
	beforeEach(async () => {
		await db.cleanupTickets()
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
})
