const puppy = require('./puppy.js')
const db = require('./db.js')

describe('Open site', () => {
	beforeAll(async () => {
		await puppy.call()
	})

	it('should be titled Don\'t fly Money', async () => {
		await expect(page.title()).resolves.toMatch('Don\'t fly Money')
	})

	it('should have a modal for contact', async () => {
		await page.click('#open-contact')

		page.waitForSelector(
			'#contact-modal',
			{ visible: true }
		)
	})

	it('should record access when logged in', async () => {
		const start = new Date()

		const email = 'access@dontflymoney.com'
		await puppy.logon(email)

		const lastAccess = await db.getLastAccess(email)

		await expect(lastAccess > start).toBeTruthy()
	})
})
