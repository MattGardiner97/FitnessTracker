function getData(ParentSelector, DataName) {
    return $(ParentSelector + " div").map(function (index, element) {
        return $(element).data(DataName);
    });
}

function setupWeekGraph() {
    var dates = getData("#WeekGraphData", "date");
    var weights = getData("#WeekGraphData", "weight");

    var context = $("#WeekGraph")[0].getContext("2d");
    var chart = new Chart(context, {
        type: "line",
        data: {
            labels: $.makeArray(dates).reverse(),
            datasets: [{
                label: "Weight (kg)",
                data: $.makeArray(weights).reverse(),
                backgroundColor: 'rgba(0,0,0,0)',
                borderColor: 'rgba(0,0,255,1)',
                borderWidth: 2,
                lineTension: 0
            }]
        }
    });
}

function setupMonthGraph() {
    var dates = getData("#MonthGraphData", "date");
    var weights = getData("#MonthGraphData", "weight");

    var context = $("#MonthGraph")[0].getContext("2d");
    var chart = new Chart(context, {
        type: "line",
        data: {
            labels: $.makeArray(dates).reverse(),
            datasets: [{
                label: "Weight (kg)",
                data: $.makeArray(weights).reverse(),
                backgroundColor: 'rgba(0,0,0,0)',
                borderColor: 'rgba(0,0,255,1)',
                borderWidth: 2,
                lineTension: 0
            }]
        }
    });
}

$(document).ready(function () {
    setupWeekGraph();
    setupMonthGraph();
});