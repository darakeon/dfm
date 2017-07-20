$(document).ready(function () {
    $("#HasLimit").click(function () {
        toggleLimitFields($(this));
    });

    toggleLimitFields($("#HasLimit"));
});

function toggleLimitFields(hasLimitCheckBox) {
    var hasLimit = hasLimitCheckBox[0].checked;

    $('#Account_RedLimit')[0].disabled = !hasLimit;
    $('#Account_YellowLimit')[0].disabled = !hasLimit;
}