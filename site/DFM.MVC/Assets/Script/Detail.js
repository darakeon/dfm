function AdjustNumbersPlus(obj, preced, currentNumber) {
	var rightPieceNumber = currentNumber + 1;

	var current = new RegExp("(" + preced + "[^0-9 ]*)(" + currentNumber + ")", "gi");
	var right = "$1" + rightPieceNumber;

	$(obj).html(
			$(obj).html().replace(current, right)
		);

	$(obj).attr("id",
			$(obj).attr("id").replace(current, right)
		);
}

function AdjustNumbersLess(identifier, preced, currentNumber) {
	var obj = $(identifier);

	GetInputValues(obj);

	var rightPieceNumber = currentNumber - 1;

	var current = new RegExp("(" + preced + "[^0-9 ]*)(" + currentNumber + ")", "gi");
	var right = "$1" + rightPieceNumber;

	obj.html(
			obj.html().replace(current, right)
		);

	obj.attr("id",
			obj.attr("id").replace(current, right)
		);

	SetInputValues(obj);
}

function GetInputValues(obj) {
	var id = obj.attr("id");

	$("#" + id + " input").each(function () {
		var value = $(this).attr("value");
		$(this).attr("editedValue", value);
	});
}

function SetInputValues(obj) {
	var id = obj.attr("id");

	$("#" + id + " input").each(function () {
		var editedValue = $(this).attr("editedValue");
		$(this).attr("value", editedValue);
	});
}