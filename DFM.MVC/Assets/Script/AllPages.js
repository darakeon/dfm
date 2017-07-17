$(document).ready(function () {
    AjustMoneyAndDate();
});


var fail;


function AjaxFail(html) {
    var error = html.responseText
                .split(/title/g)[1];

    error = error.substr(1, error.length - 3);

    alert(error);

    if (error.match(/session expired/i))
        location.reload();
}


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
        else if (dotPosition == -1)
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