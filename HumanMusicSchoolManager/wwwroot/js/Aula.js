$(document).ready(function () {

    var qtdAluno = $("#qtdAluno").val();
    for (i = 0; i < qtdAluno; i++) {
        var presenca = $("#Chamadas_" + i + "__Presenca").val();
        if (presenca === "") {
            presenca = "True";
            $("#Chamadas_" + i + "__Presenca").val("True");
        }
        if (presenca === "True") {
            $("#switch" + i).prop("checked", true);
            $("#labelSwitch" + i).text("Presente");
        }
        else {
            $("#switch" + i).prop("checked", false);
            $("#labelSwitch" + i).text("Faltou");
        }
    }
});

function presenca(i) {

    var presenca = $("#Chamadas_" + i + "__Presenca").val();
    if (presenca === "True") {
        $("#Chamadas_" + i + "__Presenca").val("False");
        $("#labelSwitch" + i).text("Faltou");
    }
    else {
        $("#Chamadas_" + i + "__Presenca").val("True");
        $("#labelSwitch" + i).text("Presente");
    }
}




