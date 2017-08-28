$(document).ready(function () {
	$(".check-toggle").click(function () {
		toggleFields($(this));
	});

	toggleFields($(".check-toggle"));
});

function toggleFields(checkBox) {
	var isChecked = checkBox.is(":checked");
	var toEnableField = checkBox.data("to-enable");
	var toDisableField = checkBox.data("to-disable");
	$(toEnableField).attr("disabled", !isChecked);
	$(toDisableField).attr("disabled", isChecked);
}