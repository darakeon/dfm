$(document).ready(function () {
	$(".check-toggle").click(function () {
		toggleFields($(this));
	});

	toggleFields($(".check-toggle"));
});

function toggleFields(checkBox) {
	var hasLimit = checkBox.is(":checked");
	var targetField = checkBox.data("target");
	$(targetField).attr("disabled", !hasLimit);
}