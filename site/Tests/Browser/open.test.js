const db = require('./db')
const puppy = require('./puppy')
const { rand } = require('./utils')


describe('Open site', () => {
	it('should be titled Don\'t fly Money', async () => {
		await puppy.call()
		await expect(page.title()).resolves.toMatch('Don\'t fly Money')
	})

	it('should have a modal for contact', async () => {
		await puppy.call()

		await page.click('ul.nav li:nth-child(2) a')

		await puppy.waitFor(
			'#contact-modal',
			{ visible: true }
		)
	})

	it('should record access when logged in', async () => {
		const start = new Date()

		const email = `access${rand()}@dontflymoney.com`
		await puppy.logon(email)

		const lastAccess = await db.getLastAccess(email)

		await expect(lastAccess > start).toBeTruthy()
	})

	it('should have a modal to change language (LOGGED IN)', async () => {
		const email = `language${rand()}@dontflymoney.com`
		await puppy.logon(email)

		await puppy.waitFor('ul.nav a.pt-br')
		await page.click('ul.nav a.pt-br')

		await puppy.waitFor(
			'#language-modal',
			{ visible: true }
		)

		await page.click('#language-modal #language-en-us')

		await puppy.waitFor('ul.nav a.en-us')
	})

	it('should have a modal to change language (LOGGED OUT)', async () => {
		await db.cleanupTickets()
		await puppy.call()

		await puppy.waitFor('ul.nav a.pt-br')
		await page.click('ul.nav a.pt-br')

		await puppy.waitFor(
			'#language-modal',
			{ visible: true }
		)

		await page.click('#language-modal #language-en-us')

		await puppy.waitFor('ul.nav a.en-us')
	})

	it('should have a robots', async () => {
		const content = await puppy.callPlain('robots.txt')

		await expect(content).toBe(
			'Sitemap: http://localhost:2703/sitemap.txt'
		)
	})

	it('should have a sitemap', async () => {
		const content = await puppy.callPlain('sitemap.txt')

		const urls = content.split('\n')

		await expect(urls.length).not.toBe(0)

		for (let u = 0; u < urls.length; u++) {
			const url = urls[u]

			await expect(url).toMatch(/http:\/\/localhost:2703\/.+/)

			const response = await page.goto(url)
			await expect(response.status()).toBe(200)
		}
	})
})
