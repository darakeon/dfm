$(document).ready(function () {
	$(".nav-tabs li").click(function () {
		var href = $(this).find("a").attr("href");

		var action = $(href).data("form");
		$("form").attr("action", action);

		$("#ActiveForm").val(href.substr(1));

		var submitText = $(this).find(".tab-text").data("button");

		$(".tab-submit").html(submitText);
	});

	var tabCaller = "." + $("#ActiveForm").val().toLowerCase() + "-caller";
	$(tabCaller).find("a").click();
});
