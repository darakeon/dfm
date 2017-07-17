$(document).ready(function () {
    AjustMoneyAndDate();
    AjustLinkButtons();
});



function AjustMoneyAndDate() {
    AjustMoney(".money");
    AjustDate(".date");
}

function AjustMoney(discriminator) {
    $(discriminator).unmaskMoney();
    $(discriminator).maskMoney();

    $(discriminator).each(function () {
        var dotPosition = this.value.indexOf('.');
        var numberSize = this.value.length;

        if (this.value == '0')
            this.value = '';
        else if (this.value != '' && dotPosition == -1)
            this.value += '.00';
        else if (dotPosition + 3 > numberSize)
            this.value += '0';
    });
}

function AjustDate(discriminator) {
    $(discriminator).unmask();
    $(discriminator).mask("99/99/9999");

    $(discriminator).each(function () {
        if (this.value == '01/01/0001')
            this.value = '';
    });
}


function AjustLinkButtons() {
    $("button").each(function () {
        $(this).click(function () {
            var href = $(this).attr("href");
            if (href) location = href;
        });
    });
}



var fail;
function AjaxFail(html) {
    var error = html.responseText
                .split(/title/g)[1];

    error = error.substr(1, error.length - 3);

    alert(error);
    EndAjaxPost()

    if (error.match(/session expired/i))
        location.reload();
}

function TellResultAndReload(data) {
    alert(data.message);
    location.reload();
}

function BeginAjaxPost() {
    $("*").css("cursor", "wait");
}

function EndAjaxPost() {
    $("*").css("cursor", "default");
}



function AjustNumbersPlus(obj, preced, currentNumber) {
    var rightPieceNumber = currentNumber + 1;

    var current = new RegExp("(" + preced + "[^0-9 ]*)(" + currentNumber + ")", "gi");
    var right = "$1" + rightPieceNumber;

    $(obj).html(
            $(obj).html().replace(current, right)
        );

    $(obj).attr("id",
            $(obj).attr("id").replace(current, right)
        );
}

function AjustNumbersLess(identifier, preced, currentNumber) {
    var obj = $(identifier);

    GetInputValues(obj);

    var rightPieceNumber = currentNumber - 1;

    var current = new RegExp("(" + preced + "[^0-9 ]*)(" + currentNumber + ")", "gi");
    var right = "$1" + rightPieceNumber;

    obj.html(
            obj.html().replace(current, right)
        );

    obj.attr("id",
            obj.attr("id").replace(current, right)
        );

    SetInputValues(obj);
}

function GetInputValues(obj) {
    var id = obj.attr("id");

    $("#" + id + " input").each(function () {
        var value = $(this).attr("value");
        $(this).attr("editedValue", value);
    });
}

function SetInputValues(obj) {
    var id = obj.attr("id");

    $("#" + id + " input").each(function () {
        var editedValue = $(this).attr("editedValue");
        $(this).attr("value", editedValue);
    });
}