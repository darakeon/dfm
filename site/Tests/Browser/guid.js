function bytesToGuid(bytes) {
	return sliceToHex(bytes, 0, 4, true)
		+ '-' + sliceToHex(bytes, 4, 6, true)
		+ '-' + sliceToHex(bytes, 6, 8, true)
		+ '-' + sliceToHex(bytes, 8, 10, false)
		+ '-' + sliceToHex(bytes, 10, 16, false)
}

function sliceToHex(bytes, start, end, reverse) {
	let array = [...bytes.slice(start, end)]
	if (reverse) array = array.reverse()
	return array.map(b => decToHex(b)).join('')
}

function decToHex(dec) {
	const pad = dec < 16 ? '0' : ''
	return pad + dec.toString(16)
}

module.exports = { bytesToGuid }
