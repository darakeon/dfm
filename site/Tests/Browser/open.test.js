const puppy = require('./puppy.js')

describe('Open site', () => {
	beforeAll(async () => {
		await puppy.call()
	})

	it('should be titled Don\'t fly Money', async () => {
		await expect(page.title()).resolves.toMatch('Don\'t fly Money')
	})
})
