jest.setTimeout(process.env.JEST_TIMEOUT || 12000)

process.on('unhandledRejection', err => {
	throw err
})
