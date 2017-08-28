function SetNavigation() {
    $("button.navigation").click(function () {

        var rel = $(this).attr("rel");
        var add = $(this).attr("add");

        var navigatorID = "#" + rel;
            
        var current = $(navigatorID)[0].value;
        var max = $(navigatorID).attr("max");
        var min = $(navigatorID).attr("min");

        add = parseInt(add);
        current = parseInt(current);
        max = parseInt(max);
        min = parseInt(min);

        var someNaN = isNaN(current) || isNaN(add) || isNaN(max) || isNaN(min);
        var comeToEnd = current >= max && add > 0;
        var comeToBegin = current <= min && add < 0;

        $(navigatorID)[0].value = 
            someNaN ? 0 :
            comeToEnd ? min :
            comeToBegin ? max :
            current + add;

        var parentInput = $(navigatorID).attr("parent");

        if ($(parentInput).length) {
            var old = $(parentInput).val() * 1;

            if (comeToEnd) {
                $(parentInput).val(old + 1);
            }

            if (comeToBegin) {
                $(parentInput).val(old - 1);
            }
        }

    });

    $("button#NavGo").click(function () {
        $("input#Year_Time").attr("id", "Year");

        var year = $("input#Year");
        var month = $("input#Month");

        var yearTime = year.val();
        var monthTime = month.val();

        var hasYear = year.length && !isNaN(yearTime);
        var hasMonth = month.length && !isNaN(monthTime);

        var time;

        if (hasYear)
            if (hasMonth)
                time = yearTime * 100 + monthTime * 1;
            else
                time = yearTime;
        else
            time = "";

        window.location = reportPage + "/" + time;
    });
}