

$(document).ready(function () {
	$('.delete-me').remove()

	$('body').css('display', 'block')

	$('.theme-changer').click(function () {
		const currentBrightness = window.currentBrightness.toLowerCase()
		const currentColor = window.currentColor.toLowerCase()
		let newBrightness = currentBrightness
		let newColor = currentColor
		const type = $(this).data('type')
		const newValue = $(this).data('choice').toLowerCase()

		if (type === 'brightness')
			newBrightness = newValue

		if (type === 'color')
			newColor = newValue

		const currentTheme = currentBrightness + currentColor
		const newTheme = newBrightness + newColor

		const currentHref = $('#bootstrap-theme').attr('href')
		const newHref = currentHref.replace(currentTheme, newTheme)
		$('#bootstrap-theme').attr('href', newHref)

		const colored = $(this).data('colored')
		if (colored) {
			$('img.image-decolorize').each(function () {
				const src = $(this).attr('src')
					.replace('on', colored)
					.replace('off', colored)
				$(this).attr('src', src)
			})

			$('link.image-decolorize').each(function () {
				const href = $(this).attr('href')
					.replace('on', colored)
					.replace('off', colored)
				$(this).attr('href', href)
			})
		}

		window.currentColor = newColor
		window.currentBrightness = newBrightness

		const parent = $(this).closest('.btn-group, .btn-group-vertical')
		const currentActive = parent.find('.btn.active')

		currentActive.removeClass('active')
		$(this).addClass('active')

		$('.theme-changer-hidden').val(newTheme)
	})
})
