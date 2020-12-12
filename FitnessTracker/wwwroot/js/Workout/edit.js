function addNewSession() {
    createSessionTable();
    createActivityTable();
    createSessionLI();
    var index = $("#SessionList li").length - 1;
    showActivityTable(index);
    setSessionSelection(index);
}

function createSessionTable() {
    var clone = $("#SessionTemplate").clone();
    clone.removeAttr("id").removeClass("d-none").attr("id", "CurrentSessionForm");
    $("#SessionContainer").append(clone);
}

function createActivityTable() {
    var tableClone = $("#ActivityTemplate").clone();
    tableClone.attr("id", null);
    tableClone.insertBefore("#AddActivityButton");
}

function createSessionLI() {
    var clone = $("#SessionListTemplate li").clone();
    $("#SessionList").append(clone);
    $("#SessionList li").last().text($("#SessionContainer .sessionForm").last().find("input").first().val());
}

function setSessionSelection(index) {
    var element = $($("#SessionList li")[index]);

    element.siblings().removeClass("active");
    element.addClass("active");

    $("#CurrentSessionForm").attr("id", null).addClass("d-none");
    var newSelectedForm = $($("#SessionContainer .sessionForm")[index]);
    newSelectedForm.attr("id", "CurrentSessionForm").removeClass("d-none");
}


function showActivityTable(sessionIndex) {
    $("#AddActivityButton").removeClass("d-none");
    $("#ActivityContainer .activityForm").addClass("d-none").attr("id", null); //Hide current form
    $("#ActivityContainer .activityForm").eq(sessionIndex).removeClass("d-none").attr("id", "CurrentActivityForm");
}

//////////////////////
//  Event handlers //
/////////////////////
function sessionListItem_Clicked(element) {
    var index = $("#SessionList li").index(element);
    setSessionSelection(index);
    showActivityTable(index);
}

function sessionName_Input(sender) {
    var text = $(sender).val();
    $("#SessionList li.active").text(text);
}

function addActivity_Clicked() {
    var rowClone = $("#ActivityRowTemplate").clone();
    rowClone.attr("id", null);
    $("#CurrentActivityForm").find("tbody").append(rowClone);
}

function planName_Input(element) {
    $("#PlanNameHeader").text($(element).val());
}

function deleteSession_Clicked(sender) {
    var sessionID = $(sender).parents("tbody").find("input[type=hidden]").val();
    $.post("/Workout/DeleteSession", { SessionID: sessionID }).always(function() {
        $(sender).parents("table").remove();
        $("#SessionList li.active").remove();
        $("#CurrentActivityForm").remove();
    });
}

function deleteActivity_Clicked(sender) {
    var activityID = $(sender).parents("tr").find("input[type=hidden]").val();
    $.post("/Workout/DeleteActivity", { ActivityID: activityID }).always(function () {
        $(sender).parents("tr").remove();
    });
}

function formPreSubmit() {
    var sessions = [];

    $("#SessionContainer .sessionForm").each(function (index, element) {
        var name = $(element).find("input").eq(0).val();
        var dayNumber = Number($(element).find("input").eq(1).val());

        var activityForm = $("#ActivityContainer .activityForm").eq(index);
        var activities = [];
        activityForm.find("tbody tr").each(function (innerIndex, innerElement) {
            var act = {
                Name: $(innerElement).find("input").eq(0).val(),
                Quantity: $(innerElement).find("input").eq(1).val(),
                Sets: Number($(innerElement).find("input").eq(2).val()),
                RestPeriodSeconds: Number($(innerElement).find("input").eq(3).val())
            };
            activities.push(act);
        });

        var session = {
            Name: name,
            DayNumber: dayNumber,
            Activities: activities
        };
        sessions.push(session);
        $("#SessionJSONInput").val(JSON.stringify(sessions));
    });
}