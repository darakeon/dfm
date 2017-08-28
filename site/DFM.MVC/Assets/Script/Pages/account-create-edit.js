$(document).ready(function () {
    $("#HasLimit").click(function () {
        toggleLimitFields($(this));
    });

    toggleLimitFields($("#HasLimit"));

    $(".suggest-name").change(suggestUrl);
});

function toggleLimitFields(hasLimitCheckBox) {
    var hasLimit = hasLimitCheckBox[0].checked;

    $('#Account_RedLimit')[0].disabled = !hasLimit;
    $('#Account_YellowLimit')[0].disabled = !hasLimit;
}

function suggestUrl(obj) {
    var name = $(".suggest-name").val();
    var url = $(".suggest-url").val();

    if (name !== "" && url === "") {
        url = name
            .toLowerCase()
            .replace(/[ ]/g, "_")
            .replace(/[^a-z0-9_]/g, "");

        $(".suggest-url").val(url);
    }
}