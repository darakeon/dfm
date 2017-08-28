$(document).ready(function () {
	$(".check-toggle").click(function () {
		toggleFields($(this));
	});

	$(".button-toggle").click(function () {

		$(this).closest(".btn-group")
			.find(".active").removeClass("active");

		$(this).toggleClass("active");

		toggleFields($(this));
	});

	toggleFields($(".check-toggle"));
	toggleFields($(".button-toggle.active"));
});

function toggleFields(obj) {
	if (obj.length === 0)
		return;

	var isChecked = obj.is(":checked") || obj.is(".active");
	var toEnableField = obj.data("to-enable");
	var toDisableField = obj.data("to-disable");
	$(toEnableField).attr("disabled", !isChecked);
	$(toDisableField).attr("disabled", isChecked);
}