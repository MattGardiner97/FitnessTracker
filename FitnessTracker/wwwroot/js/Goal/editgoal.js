function typeRadio_Changed() {
    if ($("#WeightliftingRadio:checked").length === 1) {
        $("#WeightliftingGroup").removeClass("d-none");
        $("#TimedGroup").addClass("d-none");
    }
    else {
        $("#WeightliftingGroup").addClass("d-none");
        $("#TimedGroup").removeClass("d-none");
    }
}

$(document).ready(function () {
    typeRadio_Changed();
});