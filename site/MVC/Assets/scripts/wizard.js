let wizardCount = -1;

const showLetterTime = 50
const animateHighlightTime = 50

const sides = [
	'Top',
	'Bottom',
	'Left',
	'Right',
]

function printAll() {
	const current = $(`#wizard-text-${wizardCount}`);
	current.html(current.data('text'))
}

function nextWizard() {
	const current = $('#wizard-text-' + wizardCount);

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

	const element = $('#wizard-text-' + wizardCount);
	const text = element.data('text');

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
		function (index, obj) {
			const hl = $('#wh').clone()
			hl.prop('id', null)

			const rect = obj.getBoundingClientRect()

			hl.width(rect.width)
			hl.height(rect.height)

			const borderRadius = Math.min(rect.width, rect.height) / 2 + 20

			const animationDuration = `${index + 1}s`

			hl.css({
				top: rect.top - 20 + window.scrollY,
				left: rect.left - 20 + window.scrollX,
				zIndex: getZIndex(obj),
				borderRadius,
				animationDuration
			})

			hl.addClass(`wl${wizardCount}`)

			$(document.body).append(hl)

			highlights.push(obj)
		}
	)
	highlights.reverse().forEach(
		function (obj) {
			$(obj).focus()
		}
	)
}

function getZIndex(obj) {
	let zOwner = $(obj)
	let zIndex

	do {
		zIndex = zOwner.css('z-index')
		zOwner = zOwner.parent()
	} while (zIndex === 'auto' && zOwner[0] && zOwner[0] !== document)

	return zIndex
}

function disableContinueIfLastMessage() {
	if (wizardCount + 1 === wizardMax) {
		$("#wizard-continue").addClass("disabled")
		$("#wizard-continue")[0].disabled = true
	}
}

function clearHighlight() {
	$(`.wl${wizardCount - 1}`).remove()
}

function restoreObjPostHighlight(obj, property) {
	$(obj).css(property, $(obj).data(property))
	$(obj).data(property, '')
}

$(document).ready(function () {
	nextWizard()
})
