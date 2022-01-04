var wizardCount = -1;

function printAll() {
	var current = $('#wizard-text-' + wizardCount)
	current.html(current.data('text'))
}

function nextWizard() {
	var current = $('#wizard-text-' + wizardCount)

	if (current.length === 0 || current.html() === current.data('text')) {

		if (wizardCount + 1 < wizardMax) {
			changeMessage(current)
		}

		clearHighlight()

	} else {
		printAll()
	}
}

function changeMessage(current) {
	wizardCount++

	current.html('')

	var element = $('#wizard-text-' + wizardCount)
	var text = element.data('text')

	addText(element, text, 1)
}

function addText(element, text, chars) {
	element.html(text.substring(0, chars))

	setTimeout(() => {
		if (element.html() !== text) {
			addText(element, text, ++chars)
		} else {
			addHighlight()
		}
	}, 50)
}

function clearHighlight() {
	$('.wizard-highlight-' + (wizardCount - 1)).each(
		function (_, obj) {
			$(obj).css('borderColor', $(obj).data('borderColor'))
			$(obj).css('borderWidth', $(obj).data('borderWidth'))
		}
	)
}

function addHighlight() {
	$('.wizard-highlight-' + wizardCount).each(
		function (_, obj) {
			$(obj).data('borderColor', $(obj).css('borderColor'))
			$(obj).css('borderColor', 'var(--highlight-0)')

			var borderWidth = '7px'

			$(obj).animate({
				borderWidth: borderWidth,
			}, 500, 'swing', function () {
				$(obj).data('borderWidth', $(obj).css('borderWidth'))
				$(obj).css('borderWidth', borderWidth)
			})
		}
	)
}

$(document).ready(function () {
	nextWizard()
})
