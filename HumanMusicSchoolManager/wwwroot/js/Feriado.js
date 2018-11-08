$(document).ready(function () {
    var dataFinal = $("#DataFinal").val();
    if (dataFinal !== "") {
        $("#switch").prop("checked", true);
        $("#visible").show();
    }
    else {
        $("#switch").prop("checked", false);
        $("#visible").hide();
    }
});

$("#switch").click(function () {
    if ($("#switch").prop("checked")) {
        $("#visible").show();
    }
    else {
        $("#visible").hide();
        $("#DataFinal").val(null);
    }
});