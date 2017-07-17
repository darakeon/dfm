$(document).ready(function () {
    $('#newCategoryCaller').click(function () {
        $('#newCategory').toggle();
    });

    $('#Nature').change(function () {
        ShowAccountList(this.value);
    });

    $('.DetailLevel').click(function () {
        var thisIsTrue = this.value.toLowerCase() == 'true';

        ChangeDetailLevel(thisIsTrue);
    });

    SetDetailLevel();

    var nature = $("#Nature option:selected").val();
    ShowAccountList(nature);
});


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


function ShowAccountList(value) {
    $.post(whetherShowAccountListPage,
            { nature: value },
            function (show) {
                $('#AccountID').toggle(show == 'True');
            }
        );
}



function AddDetail() {
    $("#addDetailButton").hide();

    var positionDetail = $(".moveDetailItem").length;

    $.get(addDetailsPage,
        { position: positionDetail },

        function (data) {
            $("#details").append(data);

            AjustMoney("#details .money");

            $("#addDetailButton").show();
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
        $("#addDetailButton").show();
        $('#oneValue :input').attr('disabled', true);
        $('#details :input').removeAttr('disabled');
    }
    else {
        $("#addDetailButton").hide();
        $('#oneValue :input').removeAttr('disabled');
        $('#details :input').attr('disabled', true);
    }
}


function SetDetailLevel() {
    var first = $('.DetailLevel')[0];
    var firstIsDetailed = first.value == 'True';
    var firstIsChecked = first.checked;

    var detailedIsChecked = firstIsDetailed == firstIsChecked;

    ChangeDetailLevel(detailedIsChecked);
}