$(document).ready(function () {
	const text = $('#qrcode').data('url')
	const width = 128
	const height = 128
	const correctLevel = 0
	$('#qrcode').qrcode({ text, width, height, correctLevel })
})
