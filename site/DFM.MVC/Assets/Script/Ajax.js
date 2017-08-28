var fail;
function AjaxFail(html) {
	var error = html.responseText
				.split(/title/g)[1];

	error = error.substr(1, error.length - 3);

	error = decodeHtml(error);

	alert(error);

	EndAjaxPost();

	if (error.match(/session expired/i))
		SafeReload();
}
function TellResultAndReload(data) {
	alert(decodeHtml(data.message));
	SafeReload();
}

function BeginAjaxPost() {
	$("*").css("cursor", "wait");
}

function EndAjaxPost() {
	$("*").css("cursor", "");
}

function SafeReload() {
	$("a").removeAttr("href");
	$("button").click(function (e) { e.preventDefault(); });
	$("form").submit(function (e) { e.preventDefault(); });
	
	location.reload();
}



function decodeHtml(str) {
	return $("<div/>").html(str).text();
}