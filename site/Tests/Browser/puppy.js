const db = require('./db')

let initialized = false;

async function init() {
	if (initialized) return

	await page.setExtraHTTPHeaders({
		'Accept-Language': db.language
	})

	await page.on('response', (r) => {
		if (r.status() >= 400)
			console.error(r.url(), r.status())
	})

	initialized = true
}

async function call(path) {
	await init()

	const result = await page.goto(url(path))
	await waitFor('body')

	const status = result.status()
	if (status == 200)
		return result

	const logPath = path.replace(/\//g, '_')
	await imageLog(`error_${logPath}`)
	throw status
}

async function callPlain(path) {
	const result = await page.goto(url(path))
	return await result.text()
}

function url(path) {
	if (!path) path = ''
	return `http://localhost:2709/${path}`
}

async function content(selector, options) {
	await waitFor(selector, options)
	return await page.$eval(selector, n => n.innerHTML)
}

async function waitFor(selector, options) {
	try {
		return await page.waitForSelector(selector, options)
	} catch (error) {
		const now = new Date()
		const logName = selector.replace(/[^\w]/g, '') +
			now.toISOString().replace(/[^\d]/g, '')

		await imageLog(logName)
		
		throw error
	}
}

async function clear(selector) {
	await setValue(selector, "")
}

async function setValue(selector, value) {
	await page.$eval(selector, (e, v) => e.value = v, value)
}

async function logon(email, props) {
	if (!props)
		props = {active:true}

	user = await db.createUserIfNotExists(email, props)

	const cookie = await dfmCookie()

	if (!cookie || !cookie.value) {
		await callLogonPage(email)
	} else {
		const rightLogin =
			await db.checkTicket(email, cookie.value)

		if (!rightLogin) {
			await db.cleanupTickets()
			await callLogonPage(email)
		}
	}

	return user
}

async function dfmCookie() {
	const cookies = await page.cookies()
	return cookies.filter(c => c.name == 'DFM')[0]
}

async function callLogonPage(email) {
	await db.cleanupTickets()

	await call('Users/Logon')
	await waitFor('#body form')

	await page.type('#Email', email)
	await page.type('#Password', db.password)
	await page.click('#RememberMe')

	await submit('/Users/LogOn')
}

async function submit(action) {
	const selector = `form[action="${action}"] button[type="submit"]`

	try {
		await waitFor(selector, { visible: true })
		await page.click(selector)
		await waitFor('footer')
	} catch (e) {
		const logPath = action.replace(/\//g, '_')
		await imageLog('submit_' + logPath)
		throw e
	}
}

async function createMove(
	db, description, date, value,
	categoryName, accountOutUrl, accountInUrl
) {
	const accountUrl = accountOutUrl || accountInUrl

	await call(`Account/${accountUrl}/Moves/Create`)
	await waitFor('#body form')

	await page.type('#Description', description)

	await setValue('#Date', date)

	await page.select('#CategoryName', categoryName)

	await page.click('#Nature_Transfer')

	if (accountOutUrl) {
		await page.select('#AccountOutUrl', accountOutUrl)
	} else {
		await page.click('#Nature_In')
	}

	if (accountInUrl) {
		await page.select('#AccountInUrl', accountInUrl)
	} else {
		await page.click('#Nature_Out')
	}

	await clear('#Value')
	await page.type('#Value', value)

	await page.click('#body form button[type="submit"]')
	await waitFor('#body')

	const dateParts = date.split('-')
	const year = dateParts[0]
	const month = dateParts[1]
	const day = dateParts[2]

	return db.getMoveId(description, year, month, day)
}

async function imageLog(name, silent) {
	if (!silent)
		console.log(`Recording ${name}`)

	await page.setViewport({
		width: 1024,
		height: 768,
		deviceScaleFactor: 1,
	})

	await page.screenshot({
		path: 'log/' + name + '.png'
	})

	if (!silent)
		console.log(`${name} recorded`)
}

module.exports = {
	call,
	callPlain,
	content,
	clear,
	setValue,
	logon,
	submit,
	createMove,
	imageLog,
	waitFor,
}
