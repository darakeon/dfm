describe('Open site', () => {
	beforeAll(async () => {
		await page.goto('http://localhost:2709')
	})

	it('should be titled Don\'t fly Money', async () => {
		await expect(page.title()).resolves.toMatch('Don\'t fly Money')
	})
})
