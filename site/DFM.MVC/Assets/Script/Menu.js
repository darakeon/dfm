function AdjustMenuButtons() {
	$(".menu li").click(function ()
	{
		var link = $(this).find("a").attr("href");
		location.href = link;
	});
}

