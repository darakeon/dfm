$(document).ready(function() {
	$(".btn-add-detail").click(function () {
		var detailTable = $(this).closest("table").find("tbody");
		var lastTrVisible = detailTable.find("tr:visible").last();

		var nextTrInvisible =
			lastTrVisible.length === 1
				? lastTrVisible.next("tr.hidden")
				: detailTable.find("tr.hidden").first();

		nextTrInvisible.removeClass("hidden");
		nextTrInvisible.find(".hidden-send").val("True");
	});

	$(".btn-remove-detail").click(function () {
		var tr = $(this).closest("tr");
		tr.addClass("hidden");
		tr.find(".hidden-send").val("False");

		tr.find("input").each(function () {
			if (!$(this).is("[type=hidden]")) {
				$(this).val(null);
			}
		});
	});
});