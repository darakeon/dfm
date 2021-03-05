$(document).ready(function() {
	$('.btn-add-detail').click(function () {
		const detailTable = $(this).closest('table').find('tbody')
		const lastTrVisible = detailTable.find('tr:visible').last()

		const nextTrInvisible = lastTrVisible.length === 1
			? lastTrVisible.next('tr.hidden')
			: detailTable.find('tr.hidden').first()

		nextTrInvisible.removeClass('hidden')
		nextTrInvisible.find('.hidden-send').val('True')
	})

	$('.btn-remove-detail').click(function () {
		const tr = $(this).closest('tr')
		tr.addClass('hidden')
		tr.find('.hidden-send').val('False')

		tr.find('.detail-description').val('')
		tr.find('.detail-amount').val('1')
		tr.find('.detail-value').val('0')
	})
});
