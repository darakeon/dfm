function AjustLinkButtons() {
    $("button").click(function () {
        var href = $(this).attr("href");
        if (href) location = href;
    });
}