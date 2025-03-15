async function delay(time, val) {
	return await new Promise(function(resolve) {
		setTimeout(function() {
			resolve(val);
		}, time);
	});
}

module.exports = {
	delay,
}
