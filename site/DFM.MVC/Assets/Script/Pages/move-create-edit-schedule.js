$(document).ready(function () {
    ActiveDatePicker();

    SetNewCategoryCaller();

    $(".boundlessRadio").change(function () {
        ToggleBoundless(this);
    });


    $('.natureSelect').change(function () {
        ShowAccountList(this);
    });


    $('.detailLevel').click(function () {
        ChangeDetailLevel(valueToBoolean(this));
    });


    SetDetailLevel();

    var nature = $(".natureSelect option:selected");
    ShowAccountList(nature);

    var boundless = $(".boundlessRadio:checked");
    ToggleBoundless(boundless);
});

var inputErrorClass = "input-validation-error";


function ActiveDatePicker() {
    $("#Date").datepicker({
        dateFormat: "dd/mm/yy",
        dayNamesMin: dayNames
    });

    $("#Date").attr("disabled", false);
}


function SetNewCategoryCaller() {
    $('#newCategoryCaller').click(function () {
        if ($('#top-right-div').html() == "") {

            $("#hiddenNewCategory ." + inputErrorClass)
                .removeClass(inputErrorClass);

            var html = $("#hiddenNewCategory").html();
            $('#top-right-div').html(html);

        } else {
            $('#top-right-div').html("");
        }

        return false;
    });
}


function InsertCategoryOnDropDown(data) {
    var name = data["name"];

    if (name == null) {
        $("#Category_Name").addClass(inputErrorClass);
    }
    else {
        var op = new Option(name, name, true);
        $("#CategoryName").append(op);

        $('#newCategory form')[0].reset();
        $('#top-right-div').html("");
    }
}


function ShowAccountList(obj) {
    $.post(whetherShowAccountListPage,
            { nature: $(obj).val() },
            function (show) {
                $('.accountForTransfer').toggle(toBoolean(show));
            }
        );
}



function AddDetail() {
    $("#addDetailCaller").hide();

    var positionDetail = $(".moveDetailItem").length;

    $.get(addDetailsPage,
        { position: positionDetail },

        function (data) {
            $("#details").append(data);

            AjustMoney("#details .money");

            $("#addDetailCaller").show();
        }
    );
}

function RemoveDetail(position) {
    var prefix = "#Detail_";

    $(prefix + position).remove();

    do {
        position++;

        AjustNumbersLess(prefix + position, "Detail", position)

    } while ($(prefix + position)[0]);

    AjustMoney("#details .money");
}





function ChangeDetailLevel(detailed) {
    if (detailed) {
        $("#addDetailCaller").show();
        $('#oneValue :input').attr('disabled', true);
        $('#details :input').removeAttr('disabled');
    }
    else {
        $("#addDetailCaller").hide();
        $('#oneValue :input').removeAttr('disabled');
        $('#details :input').attr('disabled', true);
    }
}


function SetDetailLevel() {
    var first = $('.detailLevel')[0];
    var firstIsDetailed = valueToBoolean(first);
    var firstIsChecked = first.checked;

    var detailedIsChecked = firstIsDetailed == firstIsChecked;

    ChangeDetailLevel(detailedIsChecked);
}


function ToggleBoundless(obj) {
    var isBoundless = valueToBoolean(obj);

    if (isBoundless != null)
        $('#Times').attr("disabled", isBoundless);
}
