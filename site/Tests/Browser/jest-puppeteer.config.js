const args = []

let browser = '/usr/bin/google-chrome'

if (process.getuid && process.getuid() == 0) {
	args.push("--no-sandbox")
} else if (process.platform.startsWith('win')) {
	browser = 'C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe'
}

module.exports = {
	launch: {
		args,
		executablePath: browser,
		dumpio: true,
	},
}
