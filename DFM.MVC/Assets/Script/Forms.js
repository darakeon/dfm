function valueToBoolean(obj) {
    var value = obj.value;

    if (value == undefined) value = obj.val();
    if (value == undefined) value = obj.attr("value");
    if (value == undefined) alert(obj.id);


    return toBoolean(value);
}

function toBoolean(value) {
    return value.toLowerCase() == "true";
}