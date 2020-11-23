function getData(ParentSelector, DataName) {
    return $(ParentSelector + " div").map(function (index, element) {
        return $(element).data(DataName);
    });
}

function setupWeekGraph() {
    var dates = getData("#WeekGraphData", "date");
    var calories = getData("#WeekGraphData", "calories");
    var carbs = getData("#WeekGraphData", "carbs");
    var protein = getData("#WeekGraphData", "protein");
    var fat = getData("#WeekGraphData", "fat");


    var weekCaloriesContext = $("#WeekCaloriesGraph")[0].getContext("2d");
    var weekCaloriesChart = new Chart(weekCaloriesContext, {
        type: "line",
        data: {
            labels: $.makeArray(dates).reverse(),
            datasets: [{
                label: "Calories",
                data: $.makeArray(calories).reverse(),
                backgroundColor: 'rgba(0,0,0,0)',
                borderColor: 'rgba(0,0,255,1)',
                borderWidth: 2,
                lineTension: 0
            }]
        }
    });

    var weekMacroContext = $("#WeekMacroGraph")[0].getContext("2d");
    var weekMacroChart = new Chart(weekMacroContext, {
        type: "line",
        data: {
            labels: $.makeArray(dates).reverse(),
            datasets: [{
                label: "Carbs",
                data: $.makeArray(carbs).reverse(),
                backgroundColor: 'rgba(0,0,0,0)',
                borderColor: 'rgba(0, 199, 0, 1)',
                borderWidth: 2,
                lineTension: 0
            },
            {
                label: "Protein",
                data: $.makeArray(protein).reverse(),
                backgroundColor: 'rgba(0,0,0,0)',
                borderColor: 'rgba(240, 220, 0, 1)',
                borderWidth: 2,
                lineTension: 0
            },
            {
                label: "Fat",
                data: $.makeArray(fat).reverse(),
                backgroundColor: 'rgba(0,0,0,0)',
                borderColor: 'rgba(240, 0, 0, 1)',
                borderWidth: 2,
                lineTension: 0
            }]
        }
    });
}

function setupMonthGraph() {
    var dates = getData("#MonthGraphData", "date");
    var calories = getData("#MonthGraphData", "calories");
    var carbs = getData("#MonthGraphData", "carbs");
    var protein = getData("#MonthGraphData", "protein");
    var fat = getData("#MonthGraphData", "fat");


    var MonthCaloriesContext = $("#MonthCaloriesGraph")[0].getContext("2d");
    var MonthCaloriesChart = new Chart(MonthCaloriesContext, {
        type: "line",
        data: {
            labels: $.makeArray(dates).reverse(),
            datasets: [{
                label: "Calories",
                data: $.makeArray(calories).reverse(),
                backgroundColor: 'rgba(0,0,0,0)',
                borderColor: 'rgba(0,0,255,1)',
                borderWidth: 2,
                lineTension: 0
            }]
        }
    });

    var MonthMacroContext = $("#MonthMacroGraph")[0].getContext("2d");
    var MonthMacroChart = new Chart(MonthMacroContext, {
        type: "line",
        data: {
            labels: $.makeArray(dates).reverse(),
            datasets: [{
                label: "Carbs",
                data: $.makeArray(carbs).reverse(),
                backgroundColor: 'rgba(0,0,0,0)',
                borderColor: 'rgba(0, 199, 0, 1)',
                borderWidth: 2,
                lineTension: 0
            },
            {
                label: "Protein",
                data: $.makeArray(protein).reverse(),
                backgroundColor: 'rgba(0,0,0,0)',
                borderColor: 'rgba(240, 220, 0, 1)',
                borderWidth: 2,
                lineTension: 0
            },
            {
                label: "Fat",
                data: $.makeArray(fat).reverse(),
                backgroundColor: 'rgba(0,0,0,0)',
                borderColor: 'rgba(240, 0, 0, 1)',
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