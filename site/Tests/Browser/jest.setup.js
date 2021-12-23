jest.setTimeout(process.env.JEST_TIMEOUT || 6000)

process.on('unhandledRejection', err => {
	throw err
})
