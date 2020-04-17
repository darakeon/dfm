const db = require('./db')

db.createContract()
	.then(() => {
		console.log('contract created!')
	})
	.catch(e => {
		process.exit(1)
	})
