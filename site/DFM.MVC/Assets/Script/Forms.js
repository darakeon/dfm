$(document).ready(function () {

    $(".validation-summary-errors:visible").each(function () {
        if ($(this).find("li:visible").length == 0) {
            $(this).remove();
        }
    });
});

function valueToBoolean(obj) {
    var value = obj.value;

    if (value == undefined) value = obj.val();
    if (value == undefined) value = obj.attr("value");
    if (value == undefined) return null;

    return toBoolean(value);
}

function toBoolean(value) {
    return value.toLowerCase() == "true";
}