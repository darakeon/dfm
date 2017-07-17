$(document).ready(function () {
    SetConfirms();
});


function SetConfirms() {
    $('.withConfirm').click(function (e) {
        var confirmMessage = $(this).attr("confirm");

        if (!confirm(confirmMessage)) {
            e.preventDefault();
        }
    });
}

