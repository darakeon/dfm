$(document).ready(function () {
	$('.tip-close').click(toggleTip)
	$('.tip img').click(toggleTip)
})

function toggleTip(obj) {
	$('.tip').toggleClass('hidden-tip')

	const url = $(obj.target).data('url')

	if (url) { $.post(url) }
}
