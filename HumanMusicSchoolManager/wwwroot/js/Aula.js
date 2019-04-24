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

    let editor = CKEDITOR.replace(campo);

    // The "change" event is fired whenever a change is made in the editor.
    editor.on('change', function (evt) {
        // getData() returns CKEditor's HTML content.
        let id = $("#Id").val();
        let conteudo = evt.editor.getData();
        $.post("/Aula/AutoSaveDescAtividades", { id: id, conteudo: conteudo });
    });
}

function Demostrativas(campo, id) {

    let editor = CKEDITOR.replace(campo);

    // The "change" event is fired whenever a change is made in the editor.
    editor.on('change', function (evt) {
        // getData() returns CKEditor's HTML content.
        let conteudo = evt.editor.getData();
        $.post("/Demostrativa/AutoSaveObservacao", { id: id, conteudo: conteudo });
    });
}

function Observacoes(i, id) {
    let conteudo = $('#Chamadas_'+i+'__Observacao').val();
    $.post("/Aula/AutoSaveObservacao", { id: id, conteudo: conteudo });
}