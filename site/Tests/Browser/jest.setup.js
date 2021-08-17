jest.setTimeout(6000)

process.on('unhandledRejection', err => {
	throw err
})
