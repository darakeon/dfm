const bcrypt = require('bcrypt');

const saltRounds = 10;

const hash = async function(text) {
	return await bcrypt
		.hash(text, saltRounds)
		.then(hash => hash)
}

const check = async function(text, hash) {
	return await bcrypt
		.compare(text, hash)
		.then(isRight => isRight)
}

module.exports = { hash, check }
