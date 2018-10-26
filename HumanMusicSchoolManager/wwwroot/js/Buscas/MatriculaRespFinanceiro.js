$(document).ready(function () {
    $("#BuscaNome").autocomplete({
        source: function (request, response)
        {
            var rota = "Matricula";
            $.ajax({
                url: "/" + rota + "/BuscarPorNome",
                type: "POST",
                dataType: "json",
                data: { nome: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.nome, value: item.nome, id: item.id };
                    }));

                }
            });
        },
        select: function (event, ui) {
            var rota = "/Matricula/Form?respFinanceiroId=" + ui.item.id;
            var alunoId = $("#alunoId").val();
            var cursoId = $("#cursoId").val();
            var pacoteAulaId = $("#pacoteAulaId").val();
            var dispSalaId = $("#dispSalaId").val();
            if (alunoId !== "") { rota += "&alunoId=" + alunoId; }
            if (cursoId !== "") { rota += "&cursoId=" + cursoId; }
            if (pacoteAulaId !== "") { rota += "&pacoteAulaId=" + pacoteAulaId; }
            if (dispSalaId !== "") { rota += "&dispSalaId=" + dispSalaId; }
            window.location.href = rota;
        },
        messages: {
            noResults: "", results: ""
        }
    });


});