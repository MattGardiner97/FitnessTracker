function getDateString(DateObject) {
    var year = String(DateObject.getFullYear());
    var month = String(DateObject.getMonth() + 1);
    month = month.length === 1 ? "0".concat(month) : month;
    var day = String(DateObject.getDate());
    day = day.length === 1 ? "0".concat(day) : day;

    return year + "-" + month + "-" + day;
}

function addNewInputRow() {
    var dateClone = $("#dateInputTemplate").clone();
    var existingDateInputCount = $("input[type=date]").length - 1;
    dateClone.attr("name", "Dates[" + String(existingDateInputCount) + "]");
    dateClone.removeClass("d-none");
    dateClone.attr("id", null);
    var newDateValue = new Date();
    newDateValue.setDate(newDateValue.getDate() - existingDateInputCount);
    dateClone.val(getDateString(newDateValue));
    $("#dateFormGroup").append(dateClone);

    var weightClone = $("#weightInputTemplate").clone();
    var existingWeightInputCount = $("input[type=number]").length - 1;
    weightClone.attr("name", "Weights[" + String(existingWeightInputCount) + "]");
    weightClone.removeClass("d-none");
    weightClone.attr("id", null);
    $("#weightFormGroup").append(weightClone);
}

$(document).ready(function () {
    addNewInputRow();  
})