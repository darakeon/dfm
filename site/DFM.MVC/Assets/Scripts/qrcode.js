$(document).ready(function () {
	const url = $('#qrcode').data('url');
	$('#qrcode').qrcode(url);
})

