const db = require('./db')

const { setDefaultOptions } = require('expect-puppeteer')

setDefaultOptions({ timeout: 10000 })

async function call(path) {
	const result = await page.goto(url(path))
	
	const status = result.status()
	if (status == 200)
		return result

	await page.screenshot({ path:'log/error.png' })
	throw status
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
	await page.evaluate(selector => {
		document.querySelector(selector).value = "";
	}, selector);
}

async function logon(email) {
	user = await db.createUserIfNotExists(email, 1)

	const cookies = await page.cookies()

	if (cookies.length == 0) await callLogonPage(email)

	const dfmCookie = cookies
		.filter(c => c.name == 'DFM')[0]

	if (!dfmCookie || !dfmCookie.value) await callLogonPage(email)

	return user
}

async function callLogonPage(email) {
	await call('Users/Logon')
	await page.waitForSelector('#body form')

	await page.type('#Email', email)
	await page.type('#Password', db.password.plain)
	await page.click('#RememberMe')
	await page.click('#body form button[type="submit"]')
}

async function submit(action) {
	await page.click(`form[action="${action}"] button[type="submit"]`)
}

async function createMove(
	db, description, date, value,
	categoryName, accountOutUrl, accountInUrl
) {
	const accountUrl = accountOutUrl || accountInUrl

	await call(`Account/${accountUrl}/Moves/Create`)
	await page.waitForSelector('#body form')

	await page.type('#Description', description)

	await clear('#Date')
	await page.type('#Date', date)

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

	const dateParts = date.split('/')
	const year = dateParts[2]
	const month = dateParts[1]
	const day = dateParts[0]

	return db.getMoveId(description, year, month, day)
}

module.exports = {
	call,
	content,
	clear,
	logon,
	submit,
	createMove,
}
