$(document).ready(function () {
	$('[data-toggle="tooltip"]').tooltip();

	$(".input-validation-error")
		.closest(".form-group")
		.addClass("has-error");

	$(".value-setter").click(function () {
		var fieldId = $(this).data("target-id");
		var fieldValue = $(this).data("target-value");

		$("#" + fieldId).val(fieldValue);
	});

	$("input.number").each(function() {
		$(this).maskMoney({
			symbol: "",
			allowZero: $(this).data("allow-zero"),
			allowNegative: $(this).data("allow-negative"),
			precision: $(this).data("precision"),
			thousands: "",
			decimal: "."
		});
	});
});

function checkIfReload(response) {
	if (response === " ") history.go(0);
}
