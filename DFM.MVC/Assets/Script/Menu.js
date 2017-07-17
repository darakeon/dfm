function AjustMenuButtons() {
    $(".higherMenu").hover(ShowSub, HideSub);
    $("#weirdMenu").click(WeirdMenu);
}

function ShowSub() {
    var id = $(this).attr("id");
    $("#Sub" + id).show();
}

function HideSub() {
    var id = $(this).attr("id");
    $("#Sub" + id).hide();
}

function WeirdMenu() {
    alert(weirdMenuExplanation);
}