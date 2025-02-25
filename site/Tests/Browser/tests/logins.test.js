const db = require('../helpers/db')
const puppy = require('../helpers/puppy')
const { rand } = require('../helpers/math')


describe('Logins', () => {
	let user = {};

	beforeAll(async () => {
		user = await puppy.logon(`logins${rand()}@dontflymoney.com`)
	})

	test('Delete', async () => {
		await puppy.call('Logins')

		const cookies = await page.cookies()
		const cookie = cookies.find(c => c.name == "DFM")
		const id = cookie.value.substring(0, 5)

		await puppy.submit(`/Logins/Delete/${id}`)

		expect(page.url()).toEqual(
			`http://localhost:2703/Users/LogOn`
		)

		const permanent = await db.getTipPermanent(user)

		expect(permanent).toEqual(1)
	})
})
