var wizardCount = -1;

var showLetterTime = 50
var animateHighlightTime = 50

function printAll() {
	var current = $('#wizard-text-' + wizardCount)
	current.html(current.data('text'))
}

function nextWizard() {
	var current = $('#wizard-text-' + wizardCount)

	if (current.length === 0 || current.html() === current.data('text')) {

		if (wizardCount + 1 < wizardMax) {
			wizardCount++
			changeMessage(current)
		}

		clearHighlight()

	} else {
		printAll()
	}
}

function changeMessage(current) {
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
			disableContinueIfLastMessage()
		}
	}, showLetterTime)
}

function addHighlight() {
	var highlights = []

	$('.wizard-highlight-' + wizardCount).each(
		function (_, obj) {
			$(obj).data('borderColor', $(obj).css('borderColor'))
			$(obj).css('borderColor', 'var(--highlight-0)')

			$(obj).data('borderStyle', $(obj).css('borderStyle'))
			$(obj).css('borderStyle', 'solid')

			$(obj).data('borderWidth', $(obj).css('borderWidth'))
			$(obj).css('borderWidth', '0')

			var sizeChange = 7

			if ($(obj).hasClass('adjust-padding')) {
				resizeCssDimension(obj, 'paddingTop', false, sizeChange)
				resizeCssDimension(obj, 'paddingBottom', false, sizeChange)
				resizeCssDimension(obj, 'paddingLeft', false, sizeChange)
				resizeCssDimension(obj, 'paddingRight', false, sizeChange)
			}

			resizeCssDimension(obj, 'borderTopWidth', true, sizeChange)
			resizeCssDimension(obj, 'borderBottomWidth', true, sizeChange)
			resizeCssDimension(obj, 'borderLeftWidth', true, sizeChange)
			resizeCssDimension(obj, 'borderRightWidth', true, sizeChange)

			highlights.push(obj)
		}
	)
	highlights.reverse().forEach(
		function (obj) {
			$(obj).focus()
		}
	)
}

function resizeCssDimension(obj, property, increase, maxDiff) {
	var current = $(obj).css(property)
	var original = $(obj).data(property)

	if (!original) {
		$(obj).data(property, current)
		original = current
	}

	var currentValue = current.replace('px', '') * 1
	var originalValue = original.replace('px', '') * 1
	var newValue = currentValue + (increase ? +1 : -1)

	var changeValue = increase
		? newValue < originalValue + maxDiff
		: newValue > originalValue - maxDiff

	if (changeValue) {
		$(obj).css(property, `${newValue}px`)

		setTimeout(
			() => {
				resizeCssDimension(obj, property, increase, maxDiff)
			},
			animateHighlightTime
		)
	}
}

function isOffScreen(obj) {
	var rect = obj.getBoundingClientRect()
	var isBefore = rect.y < rect.height
	var isAfter = rect.y > window.innerHeight

	return isBefore || isAfter
		? Math.floor(rect.y + rect.width - window.innerHeight)
		: 0;
}

function disableContinueIfLastMessage() {
	if (wizardCount + 1 === wizardMax) {
		$("#wizard-continue").addClass("disabled")
	}
}

function clearHighlight() {
	$('.wizard-highlight-' + (wizardCount - 1)).each(
		function (_, obj) {
			$(obj).css('borderColor', $(obj).data('borderColor'))
			$(obj).css('borderStyle', $(obj).data('borderStyle'))
			$(obj).css('borderWidth', $(obj).data('borderWidth'))

			if ($(obj).hasClass('adjust-padding')) {
				$(obj).css('paddingTop', $(obj).data('paddingTop'))
				$(obj).css('paddingBottom', $(obj).data('paddingBottom'))
				$(obj).css('paddingLeft', $(obj).data('paddingLeft'))
				$(obj).css('paddingRight', $(obj).data('paddingRight'))
			}
		}
	)
}

$(document).ready(function () {
	nextWizard()
})
