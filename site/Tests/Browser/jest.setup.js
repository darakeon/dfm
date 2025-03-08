jest.setTimeout(120000)

process.on('unhandledRejection', err => {
	throw err
})
