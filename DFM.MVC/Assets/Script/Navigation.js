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

    });

    $("button#NavGo").click(function () {

        var year = $("input#Year")[0];
        var month = $("input#Month")[0];

        var hasYear = year && !isNaN(year.value);
        var hasMonth = month && !isNaN(month.value);

        if (hasYear)
            if (hasMonth)
                time = year.value * 100 + month.value * 1;
            else
                time = year.value;
        else
            time = "";

        location = reportPage + "/" + time;
    });
}