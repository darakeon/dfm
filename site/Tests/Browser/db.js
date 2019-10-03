const mysql = require('mysql')
const util = require('util')
const uuid = require('uuid/v4')

const password = {
	plain: 'password',
	encrypted: '$2a$11$B.hVZuq8he7GopqvMeFXWOphCfy.ATSnR7ksneKS.eiCCKFkP8usS',
}

async function cleanup() {
	await execute('call cleanup')
}

async function createUserIfNotExists(email, active) {
	const users = await getUser(email)
	active = !!active

	const exist = users.length > 0

	if (exist) {
		await changeUserState(email, active)
	} else {
		await createUser(email, active)
	}

	await acceptLastContract(email)
}

async function getUser(email) {
	return execute(
		`select id from user where email='${email}'`
	)
}

async function changeUserState(email, active) {
	await execute(
		`update user set active=${active} where email='${email}'`
	)
}

async function createUser(email, active) {
	await execute(
		`insert into config`
			+ ` (language, timezone, sendMoveEmail, useCategories, moveCheck, theme, wizard)`
		+ ` values`
			+ ` ('pt-BR', 'E. South America Standard Time', 0, 0, 0, 1, 0)`
	)

	const config = await execute(
		`select id from config order by id desc limit 1`
	)

	await execute(
		`insert into user`
			+ ` (password, email, active, wrongLogin, config_id)`
		+ ` values`
			+ `('${password.encrypted}', '${email}', ${active}, 0, ${config[0].id})`
	)
}

async function acceptLastContract(email) {
	const queryAccepted =
		'select a.contract_id'
		+ ' from acceptance a'
			+ ' inner join user u'
				+ ' on a.user_id = u.id'
		+ ' where a.acceptDate = 1'
			+ ` and u.email = '${email}'`;

	const queryContracts =
		'select id from contract'
		+ ' where id not in ('
			+ queryAccepted
		+ ')'
		+ ' order by id desc limit 1'

	const contracts = await execute(queryContracts)

	if (contracts.length == 0) return;

	await execute(
		'insert into acceptance'
		+ ' (createDate, accepted, acceptDate, user_ID, contract_ID)'
		+ ` select now(), 1, now(), id, ${contracts[0].id}`
			+ ` from user where email='${email}'`
	)
}

async function cleanupTickets() {
	await execute(
		'update ticket set'
			+ ' key_ = regexp_replace(concat(key_, "_", now()), "[ :-]", ""),'
			+ '	active = 0'
		+ '	where active =1 and id <> 0'
	)
}

async function createToken(email, action) {
	const users = await getUser(email)

	const guid = uuid().replace(/\-/g, '').toUpperCase()

	await execute(
		`insert into security`
			+ ` (token, active, expire, action, sent, user_id)`
		+ ` values`
			+ ` ('${guid}', 1, date_add(now(), interval 1 hour), ${action}, 0, ${users[0].id})`
	)

	return guid
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
	createUserIfNotExists,
	cleanupTickets,
	createToken,
}
