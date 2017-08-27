function AdjustMoneyAndDate() {
    
    AdjustMoney(".money");
    AdjustDate(".date_notHour");
}

function AdjustMoney(discriminator) {
    $(discriminator).unmaskMoney();
    $(discriminator).maskMoney();

    $(discriminator).each(function () {
        var dotPosition = this.value.indexOf('.');
        var numberSize = this.value.length;

        var letZero = $(this).hasClass('letZero');

        if (this.value == '')
            return;

        if (this.value == '0' && !letZero)
            this.value = '';

        else if (dotPosition == -1)
            this.value += '.00';

        else if (dotPosition + 3 > numberSize)
            this.value += '0';
    });
}

function AdjustDate(discriminator) {

    $(discriminator).unmask();
    $(discriminator).mask("99/99/9999");

    $(discriminator).each(function () {
        if (this.value == '01/01/0001')
            this.value = '';
    });
}