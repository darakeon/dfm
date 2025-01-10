const zxcvbnUrl = 'https://cdn.jsdelivr.net/npm/@zxcvbn-ts';

const scoreEmojis = [
	'😨',
	'🙁',
	'😯',
	'🙂',
	'😁'
]

const scoreEmojiOff = '😴'
const scoreEmojiOn = '🤔'
	
const scoreStyles = [
	'',
	'danger',
	'warning',
	'success',
	'info'
]

let zxcvbnConfigured = false


function toggleZxcvbn() {
	if ($('#evaluate-password').is(':checked')) {
		addZxcvbn()
	} else {
		removeZxcvbn();
	}
}

function addZxcvbn() {
	addScript('zxcvbn-1', zxcvbnUrl + '/core@2.0.0/dist/zxcvbn-ts.js')
	addScript('zxcvbn-2', zxcvbnUrl + '/language-common@2.0.0/dist/zxcvbn-ts.js')
	addScript('zxcvbn-3', zxcvbnUrl + '/language-en@2.0.0/dist/zxcvbn-ts.js')

	addEvaluator()

	$('#password-emoji')
		.html(scoreEmojiOn)
		.attr('title', scoreLabelOn)
}

function addScript(id, url) {
	const scriptTag = document.createElement('script')
	scriptTag.setAttribute('id', id)
	scriptTag.setAttribute('src', url)
	document.head.appendChild(scriptTag)
}

function addEvaluator() {
	const passwordToEvaluate = $('.password-to-evaluate')

	if (!passwordToEvaluate)
		return

	if (passwordToEvaluate.hasClass('evaluator-added'))
		return

	$('.password-to-evaluate').on('keyup', evaluatePassword)
	$('.password-to-evaluate').addClass('evaluator-added')
}

function removeZxcvbn() {
	$('#zxcvbn-1').remove()
	$('#zxcvbn-2').remove()
	$('#zxcvbn-3').remove()

	$('#password-emoji')
		.html(scoreEmojiOff)
		.attr('title', scoreLabelOff)
}

function evaluatePassword(event) {
	if (!$('#evaluate-password').is(':checked'))
		return

	configZxcvbn()

	const text = $(event.target).val()
	const result = zxcvbnts.core.zxcvbn(text)

	$('#password-emoji')
		.html(scoreEmojis[result.score])
		.attr('title', scoreLabels[result.score])
}

function configZxcvbn() {
	if (zxcvbnConfigured)
		return

	// all package will be available under zxcvbnts
	const options = {
		translations: zxcvbnts['language-en'].translations,
		graphs: zxcvbnts['language-common'].adjacencyGraphs,
		dictionary: {
		...zxcvbnts['language-common'].dictionary,
		...zxcvbnts['language-en'].dictionary,
		},
	}

	zxcvbnts.core.zxcvbnOptions.setOptions(options)

	zxcvbnConfigured = true
}


$(document).ready(() => {
	$('#password-emoji')
		.html(scoreEmojiOff)
		.attr('title', scoreLabelOff)
})
