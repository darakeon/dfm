function AdjustLinkButtons() {
    $("button").click(function () {
        var href = $(this).attr("href");
        if (href) window.location = href;
    });
}