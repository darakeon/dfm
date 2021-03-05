$(document).ready(function () {
	$('.delete-me').remove()

	$('body').css('display', 'block')

	$('.theme-changer').click(function () {
		const currentTheme = window.currentTheme.toLowerCase()
		const newTheme = $(this).data('theme').toLowerCase()

		const currentHref = $('#bootstrap-theme').attr('href')
		const newHref = currentHref.replace(currentTheme, newTheme)

		$('#bootstrap-theme').attr('href', newHref)
		window.currentTheme = newTheme

		const parent = $(this).closest('.btn-group, .btn-group-vertical')
		const currentActive = parent.find('.btn.active')

		currentActive.removeClass('active')
		$(this).addClass('active')

		$('.theme-changer-hidden').val(newTheme)
	})
})
