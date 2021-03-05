$(document).ready(function () {
	$('.suggest-field').change(suggestUrl)
})

function suggestUrl(obj) {
	const field = $(obj.target)
	const name = field.val()

	const targetField = $(field.data('target'))
	var url = targetField.val()

	if (name !== '' && url === '') {
		url = name
			.toLowerCase()
			.replace(/[ ]/g, '_')
			.replace(/[^a-z0-9_]/g, '')

		targetField.val(url)
	}
}
