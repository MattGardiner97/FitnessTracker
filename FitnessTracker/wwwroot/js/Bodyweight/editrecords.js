function getDateString(DateObject) {
    var year = String(DateObject.getFullYear());
    var month = String(DateObject.getMonth() + 1);
    month = month.length === 1 ? "0".concat(month) : month;
    var day = String(DateObject.getDate());
    day = day.length === 1 ? "0".concat(day) : day;

    return year + "-" + month + "-" + day;
}

function addNewInputRow() {
    //Show remove button on existing last row
    $("#FormTableBody").find("a").last().removeClass("d-none");

    var newRowClone = $("#inputRowTemplate").clone();
    newRowClone.attr("id", null);
    newRowClone.removeClass("d-none");

    var lastDateValue = $("#FormTableBody").find("input[type=date]").last().val();

    var newDateValue = new Date(lastDateValue);
    newDateValue.setDate(newDateValue.getDate() - 1);
    newRowClone.find("input[type=date]").val(getDateString(newDateValue));

    $("#FormTableBody").append(newRowClone);
    updateInputNames();
    checkConflictingDates();
}

function deleteInputRow(removeButton) {
    $(removeButton).parents("tr").remove();
    updateInputNames();
    checkConflictingDates();
}

function updateInputNames() {
    $("#FormTableBody").find("input[type=date]").each(function (index, element) {
        $(element).attr("name", "Dates[" + String(index) + "]");
    })
    $("#FormTableBody").find("input[type=number]").each(function (index, element) {
        $(element).attr("name", "Weights[" + String(index) + "]");
    })
}

function checkConflictingDates() {
    var lastRow = $("#FormTableBody tr").last().find("input[type=date]")[0];

    $("#FormTableBody").find("input[type=date]").removeClass("border-danger");

    $("#FormTableBody").find("input[type=date]").each(function (outerIndex, outerElement) {
        $("#FormTableBody").find("input[type=date]").each(function (innerIndex, innerElement) {
            if (outerElement == innerElement)
                return;
            if (outerElement == lastRow || innerElement == lastRow)
                return;

            if ($(outerElement).val() == $(innerElement).val()) {
                $(outerElement).addClass("border-danger");
                $(innerElement).addClass("border-danger");
            }
        });
    });
}

function formPreSubmit() {
    if ($("input[type=date].border-danger").length > 0){
        window.alert("Please fix all errors before submitting.");
        return false;
    }

    $("#FormTableBody").find("tr").last().remove();
    $("#inputRowTemplate").remove();
    updateInputNames();
}

$(document).ready(function () {
    addNewInputRow();
    $("#FormTableBody").find("input[type=date]").last().val(getDateString(new Date()));
})