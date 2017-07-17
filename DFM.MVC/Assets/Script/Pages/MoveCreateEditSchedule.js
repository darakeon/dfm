$(document).ready(function () {
    ActiveDatePicker();

    $('#newCategoryCaller').click(function () {
        $('#newCategory').toggle();
    });

    $(".boundlessRadio").change(function () {
        ToggleBoundless(this);
    });


    $('#Nature').change(function () {
        ShowAccountList(this);
    });


    $('.DetailLevel').click(function () {
        ChangeDetailLevel(valueToBoolean(this));
    });


    SetDetailLevel();

    var nature = $("#Nature option:selected");
    ShowAccountList(nature);

    var boundless = $("#Boundless:checked");
    ToggleBoundless(boundless);
});


function ActiveDatePicker() {
    $("#Move_Date").datepicker({
        dateFormat: "dd/mm/yy",
        dayNamesMin: dayNames
    });

    $("#Move_Date").attr("disabled", false);
}


function InsertCategoryOnDropDown(data) {
    var id = data["id"];
    var name = data["name"];

    if (id == "0") {
        alert("Fill the Category Name.");
    }
    else {
        var op = new Option(name, id, true);
        $("#CategoryID").append(op);

        $('#newCategory form')[0].reset();
        $('#newCategory').hide();
    }
}


function ShowAccountList(obj) {
    $.post(whetherShowAccountListPage,
            { nature: $(obj).val() },
            function (show) {
                $('#AccountID').toggle(toBoolean(show));
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
    var first = $('.DetailLevel')[0];
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
