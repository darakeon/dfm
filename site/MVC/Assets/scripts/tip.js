$(document).ready(function () {
	$('.tip-close').click(toggleTip)
	$('.tip img').click(toggleTip)
})

function toggleTip(obj) {
	const url = $(obj.target).data('url')

	if (url) {
		$('.tip').addClass('hiding-tip')
		$.post(url, function () {
			$('.tip').addClass('hidden-tip')
		}).always(function () {
			$('.tip').removeClass('hiding-tip')
		})
	}
}
