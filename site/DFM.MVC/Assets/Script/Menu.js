function AjustMenuButtons() {
    $(".menu-topo li").click(function ()
    {
        var link = $(this).find("a").attr("href");
        location.href = link;
    });
}

