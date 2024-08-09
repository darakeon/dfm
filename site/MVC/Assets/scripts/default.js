$(document).ready(function () {
	$('.input-validation-error')
		.closest('.form-group')
		.addClass('has-error')

	$('.value-setter').click(function () {
		const fieldId = $(this).data('target-id')
		const fieldValue = $(this).data('target-value')

		$(`#${fieldId}`).val(fieldValue)
	})

	$('input.number').each(function() {
		$(this).maskMoney({
			symbol: '',
			allowZero: $(this).data('allow-zero'),
			allowNegative: $(this).data('allow-negative'),
			precision: $(this).data('precision'),
			thousands,
			decimal
		})

		$(this).keyup(function (event) {
			const key = event.key

			const isNumber = key >= '0' && key <= '9'
			const punctuation = key === thousands || key === decimal
			const controlKeys = event.key.length > 1

			const showWarning = !isNumber && !punctuation && !controlKeys;

			$(this).siblings('.number-error').remove()

			if (showWarning) {
				const error = $('#body .number-error').clone()
				$(this).parent().append(error)
			}
		})
	})

	$('.nav-tabs li').click(function () {
		$(this).parent().children('li').each(function () {
			$(this).removeClass('active')
		})
		$(this).addClass('active')
	})

	$('.modal').on('shown.bs.modal', function () {
		$(this).find('input').first().trigger('focus')
	})

	$('input[type="text"]').first().trigger('focus')
})

function microFormSuccess(response, result, promise, a) {
	checkIfReload(response)

	const resetId = $(this).data('ajax-reset-id')
	const resetText = $(this).data('ajax-reset-text')

	if (resetId != '#' && resetText) {
		$(resetId).html(resetText)
	}
}

function checkIfReload(response) {
	if (response === ' ')
		history.go(0)
	else if (typeof afterAjaxPost !== typeof undefined)
		afterAjaxPost()
}
