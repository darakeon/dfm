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

async function createUserIfNotExists(email, active, wizard) {
	const users = await getUser(email)
	active = !!active
	wizard = !!wizard

	const exist = users.length > 0
	let user = {}

	if (exist) {
		await changeUserState(email, active, wizard)
		user = { id: users[0].id }
	} else {
		user = await createUser(email, active, wizard)
	}

	await acceptLastContract(email)
	
	return user
}

async function getUser(email) {
	return execute(
		`select id from user where email='${email}'`
	)
}

async function changeUserState(email, active, wizard) {
	await execute(
		`update user u` +
			` inner join config c` +
				` on u.config_id = c.id` +
			` set u.active=${active},` +
				` c.wizard=${wizard}` +
		` where u.email='${email}'`
	)
}

async function createUser(email, active, wizard) {
	await execute(
		`insert into config`
			+ ` (language, timezone, sendMoveEmail, useCategories, moveCheck, theme, wizard)`
		+ ` values`
			+ ` ('pt-BR', 'E. South America Standard Time', 0, 1, 1, 1, ${wizard})`
	)

	const config = await execute(
		`select id from config order by id desc limit 1`
	)

	const result = await execute(
		`insert into user`
			+ ` (password, email, active, wrongLogin, config_id)`
		+ ` values`
			+ `('${password.encrypted}', '${email}', ${active}, 0, ${config[0].id})`
	)

	return { id: result.insertId }
}

async function acceptLastContract(email) {
	const queryAccepted =
		'select a.contract_id'
		+ ' from acceptance a'
			+ ' inner join user u'
				+ ' on a.user_id = u.id'
		+ ` where u.email = '${email}'`;

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

async function createAccountIfNotExists(name, user) {
	const url = name.toLowerCase().replace(' ', '_')
	
	const accounts = await getAccount(url, user)

	const exist = accounts.length > 0

	if (!exist) {
		await createAccount(name, url, user)
	}
	
	return url
}

async function getAccount(url, user) {
	return execute(
		`select id from account where url='${url}' and user_id=${user.id}`
	)
}

async function createAccount(name, url, user) {
	return execute(
		`insert into account `
			+ `(name, url, beginDate, user_id)`
		+ ` values`
			+ ` ('${name}', '${url}', now(), ${user.id})`
	)
}

async function createCategoryIfNotExists(name, user, disabled) {
	active = disabled?0:1
	
	const categories = await getCategory(name, user)

	const exist = categories.length > 0

	if (!exist) {
		await createCategory(name, user, active)
	} else {
		await updateCategory(name, user, active)
	}
	
	return name
}

async function getCategory(name, user) {
	return execute(
		`select id from category where name='${name}' and user_id=${user.id}`
	)
}

async function createCategory(name, user, active) {
	return execute(
		`insert into category `
			+ `(name, user_id, active)`
		+ ` values`
			+ ` ('${name}', ${user.id}, ${active})`
	)
}

async function updateCategory(name, user, active) {
	return execute(
		`update category set`
			+ ` active=${active}`
		+ ` where name='${name}' and user_id=${user.id}`
	)
}

async function createSchedule(
	times, boundless, showInstallment, frequency,
	description, date, nature, value,
	categoryName, accountOutUrl, accountInUrl,
	user
) {
	const category = await getCategory(categoryName, user)
	const accountOut = await getAccount(accountOutUrl, user)
	const accountIn = await getAccount(accountInUrl, user)
	
	const categoryId = category.length == 0 ? 'null' : category[0].id
	const accountOutId = accountOut.length == 0 ? 'null' : accountOut[0].id
	const accountInId = accountIn.length == 0 ? 'null' : accountIn[0].id
	
	const dateParts = date.split('/')
	const year = dateParts[2]
	const month = dateParts[1]
	const day = dateParts[0]
	
	return execute(
		`insert into schedule `
			+ `(times, boundless, showInstallment, frequency, `
			+ `description, year, month, day, nature, valueCents, `
			+ `category_id, out_id, in_id, user_id, lastRun, deleted) `
		+ `values `
			+ `(${times}, ${boundless}, ${showInstallment}, ${frequency}, `
			+ `'${description}', ${year}, ${month}, ${day}, '${nature}', ${value}, `
			+ `${categoryId}, ${accountOutId}, ${accountInId}, ${user.id}, 1, 0);`
	)
}

async function getMoveId(description, year, month, day) {
	const result = await execute(
		`select id from move `
			+ `where description='${description}' `
				+ `and year=${year} `
				+ `and month=${month} `
				+ `and day=${day} `
		+ `order by id desc `
		+ `limit 1`
	)
	
	return result[0].id
}

async function checkMove(id) {
	await execute(`update move set checked=1 where id=${id}`)
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
	createAccountIfNotExists,
	createCategoryIfNotExists,
	createSchedule,
	getMoveId,
	checkMove,
}
