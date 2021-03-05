$(document).ready(function () {
	$('.nav-tabs li').click(function () {
		const href = $(this).find('a').attr('href')

		const action = $(href).data('form')
		$('form').attr('action', action)

		$('#ActiveForm').val(href.substr(1))

		const submitText = $(this).find('.tab-text').data('button')

		$('.tab-submit').html(submitText)
	})

	const tabCaller = `.${$('#ActiveForm').val().toLowerCase()}-caller`
	$(tabCaller).find('a').click()
})
