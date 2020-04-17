const args = []

if (process.getuid && process.getuid() == 0)
	args.push("--no-sandbox")

module.exports = {
	launch: {
		args,
		executablePath: 'google-chrome',
		dumpio: true,
	},
}
