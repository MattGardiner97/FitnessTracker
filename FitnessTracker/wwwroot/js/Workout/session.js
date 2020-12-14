var workoutStopwatch;
var workoutTime = 0;

var currentRestPeriod = 0;
var currentRestElement;
var currentRestTimer;

function startWorkout_Clicked() {
    $("#StartWorkoutButton").fadeOut(100, function () {
        workoutTime = 0;
        updateWorkoutTime();

        workoutStopwatch = setInterval(function () {
            workoutTime++;
            updateWorkoutTime();
        }, 1000)
        $("#StartWorkoutButton").removeClass("d-block");
        $("#PauseStopDiv").addClass("d-table").hide().fadeIn(100);
    });
}

function stopWorkout_Clicked() {
    $("#PauseStopDiv").fadeOut(100, function () {
        clearInterval(workoutStopwatch);
        $("#PauseStopDiv").removeClass("d-table");
        $("#StartWorkoutButton").addClass("d-block").hide().fadeIn(100);

    });
}

function pauseWorkout_Clicked() {
    if ($("#PauseWorkoutButton").text() === "Pause") {
        $("#PauseWorkoutButton").text("Resume");
        clearInterval(workoutStopwatch);
    }
    else {
        $("#PauseWorkoutButton").text("Pause");
        workoutStopwatch = setInterval(function () {
            workoutTime++;
            updateWorkoutTime();
        }, 1000)
    }
}

function updateWorkoutTime() {
    var minutes = parseInt(workoutTime / 60);
    var seconds = parseInt(workoutTime % 60);

    var minuteString = minutes < 10 ? "0" + String(minutes) : String(minutes);
    var secondString = seconds < 10 ? "0" + String(seconds) : String(seconds);
    var resultString = minuteString + ":" + secondString;

    $("#WorkoutTime").text(resultString);
}

function startRest_Clicked(sender) {
    if (currentRestElement == null) {
        startRestPeriod(sender);
    }
    else if (currentRestElement[0] == $(sender)[0]) {
        resetRestPeriod();
    }
    else {
        resetRestPeriod();
        startRestPeriod(sender);
    }

}

function resetRestPeriod() {
    clearInterval(currentRestTimer);
    $(currentRestElement).removeClass("btn-danger").addClass("btn-primary").text("Start Rest");
    currentRestElement = null;
}

function startRestPeriod(element) {
    currentRestPeriod = $(element).data("restperiod");

    currentRestElement = $(element);
    currentRestElement.removeClass("btn-primary").addClass("btn-danger").text(currentRestPeriod);
    currentRestTimer = setInterval(function () {
        currentRestPeriod--;
        currentRestElement.text(currentRestPeriod);
        if (currentRestPeriod == 0) {
            resetRestPeriod();
        }
    }, 1000);
}