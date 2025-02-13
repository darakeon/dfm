$(document).ready(function () {
	$("#UseTFA").change(() => {
		if (!$("#UseTFA").is(":checked"))
			$("#UseTFAPassword").prop("checked", false)
	})

	$("#UseTFAPassword").change(() => {
		if ($("#UseTFAPassword").is(":checked"))
			$("#UseTFA").prop("checked", true)
	})
})
