function TellResultAndReload(data) {
    alert(data.message);
    EndAjaxPost();
    location.reload();
}