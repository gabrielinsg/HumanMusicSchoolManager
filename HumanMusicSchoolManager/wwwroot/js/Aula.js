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

    var DqtdAluno = $("#DqtdAluno").val();
    for (i = 0; i < DqtdAluno; i++) {
        var Dpresenca = $("#Demostrativas_" + i + "__Presenca").val();
        if (Dpresenca === "") {
            Dpresenca = "True";
            $("#Demostrativas_" + i + "__Presenca").val("True");
        }
        if (Dpresenca === "True") {
            $("#Dswitch" + i).prop("checked", true);
            $("#DlabelSwitch" + i).text("Presente");
        }
        else {
            $("#Dswitch" + i).prop("checked", false);
            $("#DlabelSwitch" + i).text("Faltou");
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

function Dpresenca(i) {

    var presenca = $("#Demostrativas_" + i + "__Presenca").val();
    if (presenca === "True") {
        $("#Demostrativas_" + i + "__Presenca").val("False");
        $("#DlabelSwitch" + i).text("Faltou");
    }
    else {
        $("#Demostrativas_" + i + "__Presenca").val("True");
        $("#DlabelSwitch" + i).text("Presente");
    }
}


function DescAtividades(campo) {

ClassicEditor
        .create(campo, {
            removePlugins: ['Heading', 'Link'],
            toolbar: ['bold', 'italic', 'bulletedList', 'numberedList', 'blockQuote']
        })
        .catch(error => {
            console.log(error);
        });

}

function Demostrativas(campo) {

    ClassicEditor
        .create(campo, {
            removePlugins: ['Heading', 'Link'],
            toolbar: ['bold', 'italic', 'bulletedList', 'numberedList', 'blockQuote']
        })
        .catch(error => {
            console.log(error);
        });
}

function Observacoes(i, id) {
    let conteudo = $('#Chamadas_'+i+'__Observacao').val();
    $.post("/Aula/AutoSaveObservacao", { id: id, conteudo: conteudo });
}