const puppy = require('./puppy.js')

describe('Configs', () => {
	let user = {};

	beforeAll(async () => {
		user = await puppy.logon('configs@dontflymoney.com')
	})

	test('Visit', async () => {
		await puppy.call('Configs/Config')

		const message = await puppy.content('.panel .header')
		await expect(message).toContain('Configurações')
	})
})
