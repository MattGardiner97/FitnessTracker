$(document).ready(function () {
    var type = $("#ProgressType").data("goaltype");
    if (type == "weightlifting")
        LoadWeightliftingGraph();
    else
        LoadTimedGraph();

});

function LoadWeightliftingGraph() {
    var id = $("#ProgressType").data("goalid");

    var progressData = $.get("/Goal/GetWeightliftingProgress", { GoalID: id }, function (result) {
        var dates = result.map(function (record) { return record.date; });
        var weights = result.map(function (record) { return record.weight; });
        var goalWeight = Array(dates.length).fill($("#WeightliftingGoal").data("goal"));

        var minWeight = Math.min(...weights);
        var minValue = Math.min(minWeight, goalWeight[0]) - 5;

        var maxWeight = Math.max(...weights);
        var maxValue = Math.max(maxWeight, goalWeight[0]) + 5;

        var ctx = $("#ProgressChart")[0].getContext("2d");

        var chart = new Chart(ctx,
            {
                type: 'line',
                data: {
                    labels: dates,
                    datasets: [{
                        label: "Weight (kg)",
                        data: weights,
                        backgroundColor: 'rgba(0,0,0,0)',

                        borderColor: ["#0089dc"]
                    },
                    {
                        label: "Goal (kg)",
                        data: goalWeight,
                        borderDash: [5, 5],
                        backgroundColor: 'rgba(0,0,0,0)',

                        borderColor: ["#0089dc"]
                    }]
                },
                options: {
                    scales: {
                        yAxes: [{
                            ticks: {
                                suggestedMin: minValue,
                                suggestedMax: maxValue
                            }
                        }]
                    }
                }
            });

    });
}

function LoadTimedGraph() {

}