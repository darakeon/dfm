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
			backupPreHighlight()
			addHighlight()
			disableContinueIfLastMessage()
		}
	}, showLetterTime)
}

function backupPreHighlight() {
	$(`.wizard-highlight-${wizardCount}`).each(
		function (_, obj) {
			backupObjPreHighlight(obj, 'borderColor')
			backupObjPreHighlight(obj, 'borderStyle')

			let maxChange = 7;

			if ($(obj).hasClass('adjust-padding')) {
				sides.forEach((side) => {
					const originalValue =
						backupObjPreHighlight(obj, `padding${side}`)

					const maxAllowed = originalValue.replace('px', '') * 1

					if (maxChange > maxAllowed)
						maxChange = maxAllowed
				});
			}

			$(obj).data('maxChange', maxChange)

			sides.forEach((side) => {
				backupObjPreHighlight(obj, `border${side}Width`)
			});
		}
	)
}

function backupObjPreHighlight(obj, key) {
	const value = $(obj).css(key)
	$(obj).data(key, value)
	return value
}

function addHighlight() {
	var highlights = []

	$('.wizard-highlight-' + wizardCount).each(
		function (_, obj) {
			$(obj).css('borderColor', 'var(--highlight-0)')
			$(obj).css('borderStyle', 'solid')

			var sizeChange = $(obj).data('maxChange') * 1

			if ($(obj).hasClass('adjust-padding')) {
				sides.forEach((side) => {
					resizeCssDimension(obj, `padding${side}`, false, sizeChange)
				})
			}

			sides.forEach((side) => {
				resizeCssDimension(obj, `border${side}Width`, true, sizeChange)
			})

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
	const current = $(obj).css(property);
	const original = $(obj).data(property);

	const currentValue = current.replace('px', '') * 1;
	const originalValue = original.replace('px', '') * 1;
	const newValue = currentValue + (increase ? +1 : -1);

	const changeValue = increase
		? newValue < originalValue + maxDiff
		: newValue > originalValue - maxDiff;

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
	const rect = obj.getBoundingClientRect();
	const isBefore = rect.y < rect.height;
	const isAfter = rect.y > window.innerHeight;

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
			restoreObjPostHighlight(obj, 'borderColor')
			restoreObjPostHighlight(obj, 'borderStyle')

			if ($(obj).hasClass('adjust-padding')) {
				sides.forEach((side) => {
					restoreObjPostHighlight(obj, `padding${side}`)
				});
			}

			sides.forEach((side) => {
				restoreObjPostHighlight(obj, `border${side}Width`)
			});
		}
	)
}

function restoreObjPostHighlight(obj, property) {
	$(obj).css(property, $(obj).data(property))
	$(obj).data(property, '')
}

$(document).ready(function () {
	nextWizard()
})
