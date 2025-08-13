function toggleConversion() {
	const currencyIn = $('.combo-account-in').val()
	const currencyOut = $('.combo-account-out').val()

	const differenceCurrency =
		accountCurrencies[currencyIn] !== accountCurrencies[currencyOut]

	const isTransfer = $('#Nature').val() == 'Transfer'

	if (isTransfer && differenceCurrency) {
		$('.conversion-element').show()
		$('.detail-footer').attr('colspan', 4)
	} else {
		$('.conversion-element').hide()
		$('.detail-footer').attr('colspan', 3)

		$('.conversion-element,.conversion-element input')
			.each(function (index, obj) {
				$(obj).val('')
			})
	}
}


function addDetail() {
	const addButton = $('.btn-add-detail')

	const detailTable = addButton.closest('table').find('tbody')
	const lastTrVisible = detailTable.find('tr:visible').last()

	const nextTrInvisible = lastTrVisible.length === 1
		? lastTrVisible.next('tr.hidden')
		: detailTable.find('tr.hidden').first()

	nextTrInvisible.removeClass('hidden')
	nextTrInvisible.find('.hidden-send').val('True')

	if (detailTable.find('tr:visible').length >= addButton.data('limit')) {
		addButton.prop('disabled', true)
	}
}


const regexDetail = /^([^\t;]+)(?:[\t;](\-?\d+))?(?:[\t;](\-?[\d,.]+))?$/
function splitDetail(value) {
	if (!value)
		return null

	const parts = regexDetail.exec(value)

	if (!parts)
		return null

	return {
		description: parts[1],
		amount: parts[2] || null,
		value: parts[3] || null,
	}
}


function populateDetails(details) {
	const addButton = $('.btn-add-detail')

	const detailTable = addButton.closest('table').find('tbody')
	const trs = detailTable.find('tr')
	let lastAvailable = 0

	for (let r = trs.length - 1; r > 0; r--) {
		const tr = $(trs[r])

		const description = tr.find('.detail-description').val()

		const amount = parseInt(
			tr.find('.detail-amount').val()
		)

		const value = parseInt(
			tr.find('.detail-value').val()
				.replace(/[.,]/g, '')
		)

		if (description != '' || amount != 1 || value != 0) {
			lastAvailable = r + 1
			break
		}
	}

	if (lastAvailable >= trs.length)
		return null

	for (let d = 0; d < details.length && lastAvailable < trs.length; d++, lastAvailable++) {
		const tr = $(trs[lastAvailable])

		tr.find('.detail-description').val(details[d].description)

		tr.find('.detail-amount').val(details[d].amount)

		const value = parseInt(
			details[d].value?.replace(/[,.\-]/g, '')
		)
		const cents = value % 100
		const units = (value - cents) / 100
		tr.find('.detail-value').val(
			units + decimal + cents.toString().padStart(2, '0')
		)

		tr.removeClass('hidden')
		tr.find('.hidden-send').val('True')
	}
}


$(document).ready(function () {
	$('.btn-add-detail').click(function () {
		addDetail()
	})

	$('.btn-remove-detail').click(function () {
		const tr = $(this).closest('tr')
		tr.addClass('hidden')
		tr.find('.hidden-send').val('False')

		tr.find('.detail-description').val('')
		tr.find('.detail-amount').val('1')
		tr.find('.detail-value').val('0')

		if ($('.btn-add-detail').prop('disabled')) {
			$('.btn-add-detail').prop('disabled', false)
		}
	})

	$('.combo-account').change(function () {
		toggleConversion()
	})

	toggleConversion()

	$('#details-splitter-button').click(function (e) {
		let values = $('#details-splitter-content').val()

		if (!values)
			return

		const details = values
			.replace(/\r/g, '')
			.split('\n')
			.map(splitDetail)
			.filter(d => d)

		populateDetails(details)
	})
});
