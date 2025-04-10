const sqlite = require('sqlite3')
const { generateUUID } = require('./uuid')
const { bytesToGuid } = require('./guid')
const { hash } = require('./crypt')
const { delay } = require('./time')

const password = 'pass_word'

const language = 'pt-BR'
const languageSecond = 'en-US'

async function createContract() {
	await execute(
		`insert into contract (beginDate, version)
			values (datetime('now'), 'test')`
	)

	await execute(
		`insert into terms (contract_ID, language, json)
			select id, '${language}', '{ \"Text\": \"contract\" }' from contract`
	)

	await execute(
		`insert into terms (contract_ID, language, json)
			select id, '${languageSecond}', '{ \"Text\": \"contract\" }' from contract`
	)
}

async function createPlan() {
	await execute(
		`insert into plan (
			name, priceCents,
			accountOpened, categoryEnabled,
			scheduleActive,
			accountMonthMove, moveDetail,
			archiveMonthUpload, archiveline, archivesize,
			orderMonth, orderMove
		) values (
			'browser', 0,
			20, 30,
			50,
			70, 30,
			5, 500, 352101,
			2, 500
		)`
	)
}

async function createUserIfNotExists(email, props) {
	const users = await getUser(email)

	const exist = users.length > 0
	let user = {}

	if (!props) props = {}
	active = props.active
	wizard = props.wizard
	creation = props.creation

	if (exist) {
		await changeUserState(email, active, wizard, creation)
		user = { ID: users[0].ID }
	} else {
		user = await createUser(email, active, wizard, creation)
	}

	await acceptLastContract(email)

	return user
}

async function getUser(email) {
	const { username, domain } = splitEmail(email);

	return execute(
		`select id
			from user
			where username='${username}'
				and domain='${domain}'`
	)
}

async function changeUserState(email, active, wizard, creation) {
	const { username, domain } = splitEmail(email);

	const user = await execute(
		`select settings_id, control_ID
			from user
			where username='${username}'
				and domain='${domain}'`
	)

	if (active != undefined) {
		await execute(
			`update control
				set active=${active?1:0}
				where id='${user[0].Control_ID}'`
		)
	}

	if (wizard != undefined) {
		await execute(
			`update settings
				set wizard=${wizard?1:0}
				where id=${user[0].Settings_ID}`
		)
	}

	if (creation != undefined) {
		await execute(
			`update control
				set creation=datetime('now','${creation} day')
				where id=${user[0].Control_ID}`
		)
	}
}

async function createUser(email, active, wizard, creation) {
	await execute(
		`insert into settings (
				language, timezone, sendMoveEmail,
				useCategories, useAccountsSigns, moveCheck, useCurrency, theme, wizard
			) values (
				'pt-BR', 'UTC-03:00', 0,
				1, 1, 1, 1, 1, ${wizard?1:0}
			)`
	)

	const settings = await execute(
		`select id from settings order by id desc limit 1`
	)

	await execute(
		`insert into control (
				creation, active, isAdm, isRobot,
				wrongLogin, wrongTFA,
				removalWarningSent, robotCheck, plan_id
			) values (
				datetime('now','${creation??0} day'), ${active?1:0}, 0, 0,
				0, 0,
				0, datetime('now'), 1
			)`
	)

	const control = await execute(
		`select id from control order by id desc limit 1`
	)

	const { username, domain } = splitEmail(email);

	const result = await execute(
		`insert into user (
				password, username, domain,
				settings_id, tfaPassword, control_id
			) values (
				'${await hash(password)}', '${username}', '${domain}',
				${settings[0].ID}, 0, ${control[0].ID}
			)`
	)

	return { ID: result.lastID }
}

async function acceptLastContract(email) {
	const { username, domain } = splitEmail(email)

	const queryAccepted =
		`select a.contract_id
			from acceptance a
				inner join user u
					on a.user_id = u.id
			where u.username = '${username}'
				and u.domain = '${domain}'`

	const queryContracts =
		`select id
			from contract
			where id not in (${queryAccepted})
			order by id desc limit 1`

	const contracts = await execute(queryContracts)

	if (contracts.length == 0) return;

	await execute(
		`insert into acceptance
				(createDate, accepted, acceptDate, user_ID, contract_ID)
			select datetime('now'), 1, datetime('now'), id, ${contracts[0].ID}
				from user
				where username='${username}'
					and domain='${domain}'`
	)
}

async function cleanupTickets() {
	await execute(
		`update ticket
			set key_ = key_ || strftime('%Y%m%d%H%M%f','now'),
				active = 0
			where active = 1 and id <> 0`
	)
}

async function createToken(email, action) {
	const users = await getUser(email)

	const guid = generateUUID().replace(/\-/g, '').toUpperCase()

	await execute(
		`insert into security
			(token, active, expire, action, sent, user_id)
		values
			('${guid}', 1, datetime('now','+1 hour'), ${action}, 0, ${users[0].ID})`
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
		`insert into account
			(name, url, beginDate, open, user_id)
		values
			('${name}', '${url}', datetime('now'), 1, ${user.ID})`
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
		`insert into category
				(name, user_id, active)
			values
				('${name}', ${user.ID}, ${active})`
	)
}

async function updateCategory(name, user, active) {
	return execute(
		`update category
			set active=${active}
			where name='${name}'
				and user_id=${user.ID}`
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

	const externalId = random()

	const query =
		`insert into schedule (
				externalId, times, boundless, showInstallment, frequency,
				description, year, month, day, nature, valueCents,
				category_id, out_id, in_id, user_id, lastRun, deleted
			) values (
				?, ${times}, ${boundless}, ${showInstallment}, ${frequency},
				'${description}', ${year}, ${month}, ${day}, '${nature}', ${value},
				${categoryId}, ${accountOutId}, ${accountInId}, ${user.ID}, 1, 0
			);`

	const inserted = await execute(query, [externalId])

	const result = await execute(
		`select externalId from schedule where id=${inserted.lastID}`
	)

	return bytesToGuid(result[0].ExternalId)
}

function random() {
	const array = []
	for(let b = 0; b < 16; b++) {
		const rnd = Math.floor(Math.random() * 256) % 256
		array.push(rnd)
	}
	return Buffer.from(array)
}

async function getMoveId(description, year, month, day) {
	const result = await execute(
		`select externalId
			from move
			where description='${description}'
				and year=${year}
				and month=${month}
				and day=${day}
			order by id desc
			limit 1`
	)

	const id = result[0].ExternalId

	return bytesToGuid(id)
}

async function checkMove(description, year, month, day, nature) {
	const result = await execute(
		`update move
			set checked${nature} = 1
			where description = '${description}'
				and year = ${year}
				and month = ${month}
				and day = ${day}`
	)
}

async function checkTicket(email, ticket) {
	const { username, domain } = splitEmail(email)

	const result = await execute(
		`select count(*) as occurrences
			from ticket t
				inner join user u
					on user_id = u.id
			where u.username = '${username}'
				and u.domain = '${domain}'
				and t.key_ = '${ticket}'`
	)

	const occurrences = result[0]["occurrences"]

	return occurrences > 0
}

async function getLastAccess(email) {
	const { username, domain } = splitEmail(email)

	const result = await execute(
		`select lastAccess
			from ticket t
				inner join user u
					on user_id = u.id
			where u.username = '${username}'
				and u.domain = '${domain}'`
	)

	const field = result[0]["LastAccess"]

	return new Date(field)
}

async function getLastUnsubscribeMoveMailToken(user) {
	const result = await execute(
		`select token
			from security
			where user_id=${user.ID}
				and action = 2
			order by id desc`
	)

	return result[0]["Token"]
}

async function setSecret(user, secret) {
	return await execute(
		`update user
			set tfaSecret='${secret}'
			where ID = ${user.ID}`
	)
}

async function clearSecret(user) {
	return await execute(
		`update user
			set tfaSecret=null
			where ID = ${user.ID}`
	)
}

async function getEndDate(url, user) {
	const result = await execute(
		`select endDate
			from account
			where url='${url}'
				and user_id=${user.ID}`
	)
	return result[0]["EndDate"]
}

async function getTipPermanent(user) {
	const result = await execute(
		`select Permanent
			from tips
			where type=1
				and user_id=${user.ID}`
	)
	return result[0]["Permanent"]
}

async function deleteWipe(email) {
	let { username, domain } = splitEmail(email)
	username = username.substring(0, 2)
	domain = domain.substring(0, 3)

	await execute(
		`delete from wipe
			where usernameStart = '${username}'
				and domainStart = '${domain}'`
	)
}

async function createWipe(email) {
	const hashedEmail = await hash(email)

	let { username, domain } = splitEmail(email)
	username = username.substring(0, 2)
	domain = domain.substring(0, 3)

	const why = 2 // Not Signed Contract

	const hashedPassword = await hash(password)

	var hashedEmailBase64 = Buffer.from(hashedEmail).toString('base64');
	const s3 = `${hashedEmailBase64}_19860327012000.csv`;

	await execute(
		`insert into wipe
				(hashedEmail, usernameStart, domainStart, when_, why, password, s3, theme, language)
			values
				('${hashedEmail}', '${username}', '${domain}', datetime('now'), ${why}, '${hashedPassword}', '${s3}', 3, 'pt-BR')`
	)

	return s3;
}

async function validateLastTFA(user) {
	const result = await execute(
		`select ID
			from ticket
			where user_id = '${user.ID}'
			order by ID desc`
	)

	const id = result[0]["ID"]

	await execute(
		`update ticket
			set validTFA = 1
			where id = ${id}`
	)
}


async function execute(query, params) {
	let db = {close:()=>{}};

	try {
		let done = false;
		let result;
		let error;

		if (!params) params = []
	
		const db = new sqlite.Database(
			'server/tests.db',
			(err) => { if (err) throw err }
		)

		if (query.indexOf('select') == 0) {
			db.all(query, params, (err, rows) => {
				done = true
				error = err
				result = rows
			})
		} else {
			db.run(query, params, function(err) {
				done = true
				error = err
				result = this
			})
		}
	
		while (!done)
			await delay(50)
	
		if (error) {
			if (error.code === 'SQLITE_BUSY') {
				console.warn("Database is locked. Trying to unlock...")

				removeLock(db)
				db.close()

				return await execute(query, params)

			} else {
				console.error(query)
				throw error
			}
		}
	
		removeLock(db)
		db.close()

		return result
	} catch (error) {
		console.error(new Date())
		console.error(error)

		db.close()

		throw error
	}
}

async function removeLock(db) {
	db.run("PRAGMA wal_checkpoint(TRUNCATE);")
	await delay(50)
}

function splitEmail(email) {
	if (!email) return;

	const parts = email.split('@');

	return {
		username: parts[0],
		domain: parts[1],
	}
}

module.exports = {
	password,
	language,
	createContract,
	createPlan,
	createUserIfNotExists,
	cleanupTickets,
	createToken,
	createAccountIfNotExists,
	createCategoryIfNotExists,
	createSchedule,
	getMoveId,
	checkMove,
	checkTicket,
	getLastAccess,
	getLastUnsubscribeMoveMailToken,
	setSecret,
	clearSecret,
	getEndDate,
	getTipPermanent,
	deleteWipe,
	createWipe,
	validateLastTFA,
}
