const { get } = require('axios')
const db = require('./db')

const { setDefaultOptions } = require('expect-puppeteer')

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
	await page.waitForSelector('body')

	const status = result.status()
	if (status == 200)
		return result

	await imageLog('error')
	throw status
}

async function callPlain(path) {
	const response = await get(url(path))
	return response.data
}

function url(path) {
	if (!path) path = ''
	return `http://localhost:2709/${path}`
}

async function content(selector) {
	await page.waitForSelector(selector)
	return await page.$eval(selector, n => n.innerHTML)
}

async function clear(selector) {
	await setValue(selector, "")
}

async function setValue(selector, value) {
	await page.$eval(selector, (e, v) => e.value = v, value)
}

async function logon(email) {
	user = await db.createUserIfNotExists(email, 1)

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
	await page.waitForSelector('#body form')

	await page.type('#Email', email)
	await page.type('#Password', db.password.plain)
	await page.click('#RememberMe')

	await submit('/Users/LogOn')
}

async function submit(action) {
	const selector = `form[action="${action}"] button[type="submit"]`
	const logPath = action.replace(/\//g, '_')

	var button = await page.waitForSelector(selector, { visible: true })
	await page.click(selector)
	await page.waitForSelector('footer')
}

async function createMove(
	db, description, date, value,
	categoryName, accountOutUrl, accountInUrl
) {
	const accountUrl = accountOutUrl || accountInUrl

	await call(`Account/${accountUrl}/Moves/Create`)
	await page.waitForSelector('#body form')

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

	await page.type('#Value', value)

	await page.click('#body form button[type="submit"]')
	await page.waitForSelector('#body')

	const dateParts = date.split('-')
	const year = dateParts[0]
	const month = dateParts[1]
	const day = dateParts[2]

	return db.getMoveId(description, year, month, day)
}

async function imageLog(name) {
	console.log(`Recording ${name}`)

	await page.setViewport({
		width: 1024,
		height: 768,
		deviceScaleFactor: 1,
	})

	await page.screenshot({
		path: 'log/' + name + '.png'
	})

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
}
