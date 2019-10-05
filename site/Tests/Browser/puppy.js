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

module.exports = {
	call,
	content,
	clear,
}
