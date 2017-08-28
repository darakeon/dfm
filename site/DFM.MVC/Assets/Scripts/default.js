$(document).ready(function () {
	$('[data-toggle="tooltip"]').tooltip();

	$(".input-validation-error")
		.closest(".form-group")
		.addClass("has-error")
		.addClass("has-feedback");

	$(".value-setter").click(function () {
		var fieldId = $(this).data("target-id");
		var fieldValue = $(this).data("target-value");

		$("#" + fieldId).val(fieldValue);
	});

	$(".input-group.date").datetimepicker({
		format: 'DD/MM/YYYY',
		locale: window.language
	});
});