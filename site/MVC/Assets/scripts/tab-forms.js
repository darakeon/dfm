$(document).ready(function () {
	$('.nav-tabs li').click(function () {
		const href = $(this).find('a').attr('href')

		const action = $(href).data('form')
		$('form').attr('action', action)

		$('#ActiveForm').val(href.substr(1))

		const submitText = $(this).find('.tab-text').data('button')

		$('.tab-submit').html(submitText)

		location.href = $(this).children().attr('href')
	})

	var chosen = location.href.split('#')[1]
		|| $('#ActiveForm').val().toLowerCase()
	if (chosen) {
		const tabCaller = `.${chosen}-caller`
		$(tabCaller).find('a').click()
	}
})
