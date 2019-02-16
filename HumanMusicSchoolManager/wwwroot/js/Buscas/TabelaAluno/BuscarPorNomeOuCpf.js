$(document).ready(function () {
    $("#BuscaNome").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Aluno/BuscarPorNome",
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
            window.location.href = "/Aluno/Aluno?AlunoId=" + ui.item.id;
        },
        messages: {
            noResults: "", results: ""
        }
    });

    $("#BuscaCpf").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Aluno/BuscarPorCpf",
                type: "POST",
                dataType: "json",
                data: { cpf: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.cpf + " " + item.nome, value: item.cpf, id: item.id };
                    }));

                }
            });
        },
        select: function (event, ui) {
            window.location.href = "/Aluno/Aluno?AlunoId=" + ui.item.id;
        },
        messages: {
            noResults: "", results: ""
        }
    });
});