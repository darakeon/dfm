const crypto = require('crypto')

function code(key) {
	const factor = timeFactor()
	return encrypt(factor, key)
}

function timeFactor() {
	let hex = count30secs()
		.toString(16)
		.toUpperCase()

	while(hex.length < 16) {
		hex = '0' + hex
	}

	return hex
		.match(/.{2}/g)
		.map(b => parseInt(b, 16))
}

function count30secs() {
	const milliseconds = Date.now()
	const seconds = milliseconds / 1000
	return Math.floor(seconds / 30)
}

function encrypt(message, key) {
	key = Buffer.from(key)
	message = Buffer.from(message)

	const hash = crypto
		.createHmac('sha1', key)
		.update(message)
		.digest()

	const position = hash.slice(-1)[0] & 0xf

	const digit =
		(hash[position+0] & 0x7f) << 24 |
		(hash[position+1] & 0xff) << 16 |
		(hash[position+2] & 0xff) << 8 |
		(hash[position+3] & 0xff)

	const text = digit.toString()

	return text.substring(text.length - 6)
}

module.exports = {
	code
}
