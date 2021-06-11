jest.setTimeout(5000)

process.on('unhandledRejection', err => {
	throw err
})
