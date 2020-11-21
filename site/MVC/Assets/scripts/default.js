$(document).ready(function () {
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
			thousands,
			decimal
		});
	});

	$(".nav-tabs li").click(function () {
		$(this).parent().children("li").each(function () {
			$(this).removeClass("active");
		});
		$(this).addClass("active");
	});
});

function checkIfReload(response) {
	if (response === " ") history.go(0);
}
