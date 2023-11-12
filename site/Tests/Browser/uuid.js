function generateUUID() {
	const frag1 = random(16, 8);
	const frag2 = random(16, 4);
	const frag3 = random(16, 4);
	const frag4 = random(16, 4);
	const frag5 = random(16, 12);
	return `${frag1}-${frag2}-${frag3}-${frag4}-${frag5}`;
}

function random(base, pow) {
	return Math.ceil(
		Math.random() * Math.pow(base, pow)
	).toString(base)
}
module.exports = {
    generateUUID
}
