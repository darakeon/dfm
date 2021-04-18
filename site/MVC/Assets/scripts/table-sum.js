$(document).ready(function () {
	window.afterAjaxPost = addSumFactor
	addSumFactor()
})

let total = 0
const ids = []

function addSumFactor() {
	$('.sum-factor').each(function (_, obj) {
		if ($(obj).hasClass('has-sum-factor'))
			return;

		$(obj).addClass('has-sum-factor')

		toggleClass(obj)
		
		$(obj).click(function (event) {
			toggleFromSum(this, event.target)
		})
	})
}

function toggleClass(obj) {
	return $(obj).toggleClass('active', active(obj))
}

function active(obj) {
	return ids.indexOf(obj.id) >= 0
}

function toggleFromSum(obj, target) {
	if (clickedButtonChild(obj, target))
		return {};

	toggleArray(obj)
	toggleClass(obj)

	if (ids.length === 0)
		return clear()

	recalculateTotal(obj)
}

function clickedButtonChild(obj, target) {
	target = $(target)

	while (target.attr('id') !== obj.id) {
		if ($(target).attr('role') === 'button')
			return true

		target = target.parent()
	}

	return false
}

function toggleArray(obj) {
	if (active(obj))
		ids.splice(ids.indexOf(obj.id), 1)
	else
		ids.push(obj.id)
}

function clear() {
	total = 0
	$('.top-right-highlight').html('')
}

function recalculateTotal(obj) {
	const value =
		$(obj).find('.value')
			.html()
			.replace(',', '.') * 1

	const factor = active(obj) ? 1 : -1

	total += value * factor
	total = Math.round(total * 100) / 100

	$('.top-right-highlight').html(
		total.toLocaleString(
			lang,
			{ minimumFractionDigits: 2 }
		)
	)
}
