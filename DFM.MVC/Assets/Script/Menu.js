function AjustMenuButtons() {
    $(".accountMenu").hover(ShowSub, HideSub);
}

function ShowSub() {
    var id = $(this).attr("id");
    $("#Sub" + id).show();
}

function HideSub() {
    var id = $(this).attr("id");
    $("#Sub" + id).hide();
}