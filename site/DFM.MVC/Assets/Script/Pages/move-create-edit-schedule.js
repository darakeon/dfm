$(document).ready(function () {
    ActiveDatePicker();

    makeCategoryModal();

    $(".boundlessRadio").change(function () {
        ToggleBoundless(this);
    });


    $('.nature-select').change(function () {
        ShowAccountList(this);
    });


    $('.detailLevel').click(function () {
        ChangeDetailLevel(valueToBoolean(this));
    });


    SetDetailLevel();

    var nature = $(".nature-select option:selected");
    ShowAccountList(nature);

    var boundless = $(".boundlessRadio:checked");
    ToggleBoundless(boundless);
});



function ActiveDatePicker() {
    $("#Date").datepicker({
        dateFormat: "dd/mm/yy",
        dayNamesMin: dayNames
    });

    $("#Date").attr("disabled", false);
}


function makeCategoryModal() {
    $("#hidden-new-category").dialog({
        autoOpen: false,
        resizable: false,
        draggable: false,
        modal: true,
        width: 490,
        title: "x",
        open: function () {
            $('.ui-widget-overlay').bind('click', function() {
                $('#hidden-new-category').dialog('close');
            });
        }
    });

    $('#new-category-caller').click(function () {
        $("#hidden-new-category").dialog("open");
    });

}


function InsertCategoryOnDropDown(data) {
    var name = data["name"];
    var error = data["error"];

    if (!name && !error) {
        location.reload();
    }
    else if (error) {
        $(".CRUDCategory #Name").addClass("input-validation-error");
        $(".CRUDCategory .field-validation-valid").html(error);
        $(".CRUDCategory .field-validation-valid").addClass("field-validation-error");
        $(".CRUDCategory .field-validation-valid").removeClass("field-validation-valid");
    }
    else if (!name) {
        $(".CRUDCategory #Name").addClass("input-validation-error");
    }
    else {

        var alreadyInDropDown = $("#CategoryName option[value='" + name + "']");

        if (alreadyInDropDown.length) {
            $("#CategoryName").val(name);
        } else {
            var op = new Option(name, name, true);
            $("#CategoryName").append(op);
        }

        $('#hidden-new-category form')[0].reset();
        $("#hidden-new-category").dialog("close");

        $(".CRUDCategory #Name").removeClass("input-validation-error");
    }
}


function ShowAccountList(obj) {
	var nature = $(obj).val();

	$(".account-combo").each(function() {
		var show = $(this).data(nature) === "1";
		$(this).toggle(show);
	});
}



function AddDetail(page) {
    $("#addDetailCaller").hide();

    var positionDetail = $(".moveDetailItem").length;

    $.get(page,
        { position: positionDetail },

        function (data) {
            $("#details").append(data);

            AdjustMoney("#details .money");

            $("#addDetailCaller").show();
        }
    );
}

function RemoveDetail(position) {
    var prefix = "#Detail_";

    $(prefix + position).remove();

    do {
        position++;

	    AdjustNumbersLess(prefix + position, "Detail", position);

    } while ($(prefix + position)[0]);

    AdjustMoney("#details .money");
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

    var detailedIsChecked = firstIsDetailed === firstIsChecked;

    ChangeDetailLevel(detailedIsChecked);
}


function ToggleBoundless(obj) {
    var isBoundless = valueToBoolean(obj);

    if (isBoundless != null)
        $('#Times').attr("disabled", isBoundless);
}

