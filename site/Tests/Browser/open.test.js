const puppy = require('./puppy.js')
const db = require('./db.js')

describe('Open site', () => {
	it('should be titled Don\'t fly Money', async () => {
		await puppy.call()
		await expect(page.title()).resolves.toMatch('Don\'t fly Money')
	})

	it('should have a modal for contact', async () => {
		await puppy.call()

		await page.click('#open-contact')

		await page.waitForSelector(
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

	it('should have a modal to change language (LOGGED IN)', async () => {
		const email = 'language@dontflymoney.com'
		await puppy.logon(email)

		let button = await puppy.content('#open-language')
		await expect(button).toContain(
			'<img src="/Assets/images/pt-br.svg">'
		)

		await page.click('#open-language')

		await page.waitForSelector(
			'#language-modal',
			{ visible: true }
		)

		await page.click('#language-modal #language-en-us')

		button = await puppy.content('#open-language')
		await expect(button).toContain(
			'<img src="/Assets/images/en-us.svg">'
		)
	})

	it('should have a modal to change language (LOGGED OUT)', async () => {
		await db.cleanupTickets()
		await puppy.call()

		let button = await puppy.content('#open-language')

		await expect(button).toContain(
			'<img src="/Assets/images/pt-br.svg">'
		)

		await page.click('#open-language')

		await page.waitForSelector(
			'#language-modal',
			{ visible: true }
		)

		await page.click('#language-modal #language-en-us')

		button = await puppy.content('#open-language')
		await expect(button).toContain(
			'<img src="/Assets/images/en-us.svg">'
		)
	})
})
