const puppy = require('./puppy.js')

describe('Logins', () => {
	let user = {};

	beforeAll(async () => {
		user = await puppy.logon('logins@dontflymoney.com')
	})

	test('Delete', async () => {
		await puppy.call('Logins')

		const cookies = await page.cookies()
		const cookie = cookies.find(c => c.name == "DFM")
		const id = cookie.value.substring(0, 5)

		await puppy.submit(`/Logins/Delete/${id}`)

		expect(page.url()).toEqual(
			'http://localhost:2709/Users/LogOn'
		)
	})
})
