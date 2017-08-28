$(document).ready(function () {
	$('[data-toggle="tooltip"]').tooltip();

	$(".input-validation-error")
		.closest(".form-group")
		.addClass("has-error")
		.addClass("has-feedback");
});