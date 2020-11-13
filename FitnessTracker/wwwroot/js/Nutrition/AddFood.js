
function onExistingFoodSelected() {
    var selectedValue = $("#ExistingFoodDropdown").val();
    var selectedOption = $("#ExistingFoodDropdown").find("option[value=" + String(selectedValue) + "]");
    addFoodRecord(selectedOption[0]);
    $("#ExistingFoodDropdown").val(0);
}

function addFoodRecord(selectedOption) {
    var id = $(selectedOption).data("id");
    var name = $(selectedOption).data("name");
    var carbs = $(selectedOption).data("carbs");
    var protein = $(selectedOption).data("protein");
    var fat = $(selectedOption).data("fat");
    var calories = $(selectedOption).data("calories");

    var rowClone = $("#NewRowTemplate").clone();
    rowClone.removeAttr("id");
    rowClone.removeClass("d-none");

    rowClone.find(".recordID").val(id);
    rowClone.find(".recordName").val(name);
    rowClone.find(".recordCarbs").val(carbs);
    rowClone.find(".recordProtein").val(protein);
    rowClone.find(".recordFat").val(fat);
    rowClone.find(".recordCalories").val(calories);

    $("#RecordBody").append(rowClone);
    updateInputNames();
}
function updateInputNames() {
    $("#RecordBody").find("tr").each(function (index, row) {
        $(row).find(".recordID").attr("name", "FoodIDs[" + String(index) + "]");
        $(row).find(".recordQuantity").attr("name", "FoodIDs[" + String(index) + "]");
    });
}

function removeRow(row) {
    $(row).parents("tr").remove();
    updateInputNames();
}

$(document).ready(function () {
    window.alert("Add Edit Food functionality");
});