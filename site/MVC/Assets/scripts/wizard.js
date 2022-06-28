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
		if (element.text() !== text) {
			addText(element, text, ++chars)
		} else {
			addHighlight()
			disableContinueIfLastMessage()
		}
	}, showLetterTime)
}

function addHighlight() {
	$('.wizard-highlight-' + wizardCount).each(
		function (index, obj) {
			const fixed = checkFixed(obj)

			addHighlightTo(index, obj, fixed)

			if (!fixed) {
				scrollTo(obj)
			}
		}
	)

	$('.wizard-scroll-' + wizardCount).each(
		function (index, obj) {
			scrollTo(obj)
		}
	)
}

function checkFixed(obj) {
	let check = $(obj);
	const maxParent = $(document.documentElement)

	while (check[0] !== maxParent[0]) {
		if (check.css('position') === 'fixed') {
			return true
		}
		check = check.parent()
	}

	return false
}

function addHighlightTo(index, obj, fixed) {
	const hl = $('#wh').clone()
	hl.prop('id', null)

	const rect = obj.getBoundingClientRect()

	hl.width(rect.width)
	hl.height(rect.height)

	const borderRadius = getBorderRadius(rect)

	const borderAnimationDuration = `${index + 1}s`

	let top = rect.top - 20
	let left = rect.left - 20

	if (!fixed) {
		top += window.scrollY
		left += window.scrollX
	}

	const zIndex = getZIndex(obj)

	hl.css({
		top,
		left,
		zIndex,
		borderRadius,
		borderAnimationDuration
	})

	hl.addClass(`wl${wizardCount}`)

	if ($(obj).hasClass('wl-other-color')) {
		hl.addClass('wl-other-color')
	}

	$(document.body).append(hl)

	if ($(obj).hasClass('hide-whl-after-click')) {
		$(obj).click(function() {
			hl.remove()
			return true
		})
	}

	if (fixed) {
		$(hl).css('position', 'fixed')
	}
}

function getBorderRadius(rect) {
	return Math.round(
		Math.min(
			Math.min(
				rect.width,
				rect.height
			)
			/ 2 + 20,
			50
		)
	)
}

function getZIndex(obj) {
	let zOwner = $(obj)
	let zIndex

	do {
		zIndex = zOwner.css('z-index')
		zOwner = zOwner.parent()
	} while (zIndex === 'auto' && zOwner[0] && zOwner[0] !== document)

	if (zIndex === 'auto')
		return 2

	return zIndex
}

function scrollTo(obj) {
	const objTop = Math.round($(obj).offset().top)
	const objPageTop = objTop - 220

	const body = [document.documentElement, document.body]
	$(body).animate(
		{ scrollTop: objPageTop }, 1000
	)
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

$(document).ready(function () {
	nextWizard()
})
