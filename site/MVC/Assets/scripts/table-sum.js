$(document).ready(function () {
	let total = 0

	$('.sum-factor').click(function (e) {
		let target = $(e.target)
		const id = $(this).attr('id')

		while (target.attr('id') !== id) {
			if ($(target).attr('role') === 'button')
				return;

			target = target.parent()
		}

		const factor = $(this).hasClass('active') ? -1 : 1

		$(this).toggleClass('active')

		const parent = $(this).data('parent')
		const anyActive = $(parent + ' .active').length > 0

		if (!anyActive) {
			total = 0
			$('.top-right-highlight').html('')
			return
		}

		const value =
			$(this).find('.value')
				.html()
				.replace(',', '.')

		total += parseFloat(value) * factor
		total = Math.round(total * 100) / 100

		$('.top-right-highlight').html(
			total.toLocaleString(lang, { minimumFractionDigits: 2 })
		)
	})
})
