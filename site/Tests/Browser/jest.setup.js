jest.setTimeout(60000)

process.on('unhandledRejection', err => {
	throw err
})
