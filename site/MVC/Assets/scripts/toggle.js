$(document).ready(function () {
	$('.check-toggle').click(function () {
		toggleFields($(this))
	})

	$('.button-toggle').click(function () {

		$(this).closest('.btn-group, .btn-group-vertical')
			.find('.active').removeClass('active')

		$(this).toggleClass('active')

		toggleFields($(this))
	})

	toggleFields($('.check-toggle'))
	toggleFields($('.button-toggle.active'))
})

function toggleFields(list) {
	if (list.length === 0)
		return;

	$(list).each(function () {
		const obj = $(this)

		const isChecked = obj.is(':checked') || obj.is('.active')
		const toEnableField = obj.data('to-enable')
		const toDisableField = obj.data('to-disable')
		$(toEnableField).attr('disabled', !isChecked)
		$(toDisableField).attr('disabled', isChecked)
	})
}
