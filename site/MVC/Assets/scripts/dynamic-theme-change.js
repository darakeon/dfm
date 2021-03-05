$(document).ready(function () {
	$(".delete-me").remove();

	$("body").css("display", "block");

	$(".theme-changer").click(function () {
		var currentTheme = window.currentTheme.toLowerCase();
		var newTheme = $(this).data("theme").toLowerCase();

		var currentHref = $("#bootstrap-theme").attr("href");
		var newHref = currentHref.replace(currentTheme, newTheme);

		$("#bootstrap-theme").attr("href", newHref);
		window.currentTheme = newTheme;

		var parent = $(this).closest(".btn-group, .btn-group-vertical");
		var currentActive = parent.find(".btn.active");

		currentActive.removeClass("active");
		$(this).addClass("active");

		$(".theme-changer-hidden").val(newTheme);
	});
});
