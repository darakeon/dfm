const mysql = require('mysql')
const util = require('util')

const password = {
	plain: 'password',
	encrypted: '$2a$11$B.hVZuq8he7GopqvMeFXWOphCfy.ATSnR7ksneKS.eiCCKFkP8usS',
}

async function cleanup() {
	await execute('call cleanup')
}

async function execute(query) {
	const connection = mysql.createConnection({
		host     : 'localhost',
		database : 'dfm_test',
		user     : 'dfm_user',
	})

	connection.connect()

	let done = false;
	let result;
	let error;

	connection.query(
		query,
		function (errors, results, fields) {
			done = true
			error = errors
			result = results
		}
	)

	while (!done)
		await delay(100)

	connection.end()

	if (error)
		throw error

	return result
}

function delay(time, val) {
   return new Promise(function(resolve) {
       setTimeout(function() {
           resolve(val);
       }, time);
   });
}

module.exports = {
	password,
	cleanup,
}
