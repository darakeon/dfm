$(document).ready(function () {
	$(".suggest-field").change(suggestUrl);
});

function suggestUrl(obj) {
	var field = $(obj.target);
	var name = field.val();

	var targetField = $(field.data("target"));
	var url = targetField.val();

	if (name !== "" && url === "") {
		url = name	
			.toLowerCase()
			.replace(/[ ]/g, "_")
			.replace(/[^a-z0-9_]/g, "");

		targetField.val(url);
	}
}