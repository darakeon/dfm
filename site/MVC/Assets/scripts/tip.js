$(document).ready(function () {
	$('.tip-close').click(toggleTip)
	$('.tip img').click(toggleTip)
})

function toggleTip() {
	$('.tip').toggleClass('hidden-tip')
}