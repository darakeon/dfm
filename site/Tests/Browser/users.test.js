const puppeteer = require('puppeteer')
const fs = require('fs')
const db = require('./db')

describe('Users', () => {
	beforeAll(async () => {
		await db.cleanup()
	}, 30000)

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
})
