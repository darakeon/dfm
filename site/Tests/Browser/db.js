const sqlite = require('sqlite3')
const util = require('util')
const { v4: uuid } = require('uuid')

const password = {
	plain: 'password',
	encrypted: '$2a$11$B.hVZuq8he7GopqvMeFXWOphCfy.ATSnR7ksneKS.eiCCKFkP8usS',
}

const language = 'pt-BR'

async function createContract() {
	await execute(
		`insert into contract (beginDate, version) 
			values (datetime('now'), 'test')`
	)

	await execute(
		`insert into terms (contract_ID, language, json)
			select id, '${language}', '{ \"Text\": \"contract\" }' from contract`
	)
}

async function createUserIfNotExists(email, active, wizard) {
	const users = await getUser(email)
	active = !!active
	wizard = !!wizard

	const exist = users.length > 0
	let user = {}

	if (exist) {
		await changeUserState(email, active, wizard)
		user = { ID: users[0].ID }
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
	const user = await execute(
		`select config_id from user where email='${email}'`
	)
	await execute(
		`update user
			set active=${active?1:0}
			where email='${email}'`
	)
	await execute(
		`update config as c
			set wizard=${wizard?1:0}
			where id=${user[0].Config_ID}`
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
			+ `('${password.encrypted}', '${email}', ${active}, 0, ${config[0].ID})`
	)

	return { ID: result.lastID }
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
		+ ` select datetime('now'), 1, datetime('now'), id, ${contracts[0].ID}`
			+ ` from user where email='${email}'`
	)
}

async function cleanupTickets() {
	await execute(
		'update ticket set'
			+ ` key_ = key_ || strftime('%Y%m%d%H%M%f','now'),`
			+ '	active = 0'
		+ '	where active = 1 and id <> 0'
	)
}

async function createToken(email, action) {
	const users = await getUser(email)

	const guid = uuid().replace(/\-/g, '').toUpperCase()

	await execute(
		`insert into security`
			+ ` (token, active, expire, action, sent, user_id)`
		+ ` values`
			+ ` ('${guid}', 1, datetime('now','+1 hour'), ${action}, 0, ${users[0].ID})`
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
		`select id from account where url='${url}' and user_id=${user.ID}`
	)
}

async function createAccount(name, url, user) {
	return execute(
		`insert into account `
			+ `(name, url, beginDate, user_id)`
		+ ` values`
			+ ` ('${name}', '${url}', datetime('now'), ${user.ID})`
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
		`select id from category where name='${name}' and user_id=${user.ID}`
	)
}

async function createCategory(name, user, active) {
	return execute(
		`insert into category `
			+ `(name, user_id, active)`
		+ ` values`
			+ ` ('${name}', ${user.ID}, ${active})`
	)
}

async function updateCategory(name, user, active) {
	return execute(
		`update category set`
			+ ` active=${active}`
		+ ` where name='${name}' and user_id=${user.ID}`
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
	
	const categoryId = category.length == 0 ? 'null' : category[0].ID
	const accountOutId = accountOut.length == 0 ? 'null' : accountOut[0].ID
	const accountInId = accountIn.length == 0 ? 'null' : accountIn[0].ID
	
	const dateParts = date.split('-')
	const year = dateParts[0]
	const month = dateParts[1]
	const day = dateParts[2]
	
	return execute(
		`insert into schedule `
			+ `(times, boundless, showInstallment, frequency, `
			+ `description, year, month, day, nature, valueCents, `
			+ `category_id, out_id, in_id, user_id, lastRun, deleted) `
		+ `values `
			+ `(${times}, ${boundless}, ${showInstallment}, ${frequency}, `
			+ `'${description}', ${year}, ${month}, ${day}, '${nature}', ${value}, `
			+ `${categoryId}, ${accountOutId}, ${accountInId}, ${user.ID}, 1, 0);`
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
	
	return result[0].ID
}

async function checkMove(id, nature) {
	await execute(`update move set checked${nature}=1 where id=${id}`)
}

async function execute(query) {	
	let done = false;
	let result;
	let error;

	const db = new sqlite.Database(
		'server/tests.db',
		(err) => { if (err) throw err }
	)

	if (query.indexOf('select') == 0) {
		db.all(query, [], (err, rows) => {
			done = true
			error = err
			result = rows			
		})
	} else {
		db.run(query, [], function(err) {
			done = true
			error = err
			result = this
		})
	}

	while (!done)
		await delay(100)

	db.close()

	if (error) {
		console.log(query)
		console.error(error)
		throw error
	}

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
	language,
	createContract,
	createUserIfNotExists,
	cleanupTickets,
	createToken,
	createAccountIfNotExists,
	createCategoryIfNotExists,
	createSchedule,
	getMoveId,
	checkMove,
}
