$(document).ready(function () {
    $("#BuscaNome").autocomplete({
        source: function (request, response)
        {
            $.ajax({
                url: "/Funcionario/BuscarPorNome",
                type: "POST",
                dataType: "json",
                data: { nome: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.nome, value: item.nome, id: item.id };
                    }))

                }
            })
        },
        select: function (event, ui) {
            window.location.href = '/Funcionario/Form?funcionarioId='+ui.item.id;
        },
        messages: {
            noResults: "", results: ""
        }
    });
});