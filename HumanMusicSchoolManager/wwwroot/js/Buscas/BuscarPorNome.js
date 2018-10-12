$(document).ready(function () {
    $("#BuscaNome").autocomplete({
        source: function (request, response)
        {
            var rota = $("#rota").val();
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
            window.location.href = "/" + rota.value + "/Form?" + rota.value + "Id=" + ui.item.id;
        },
        messages: {
            noResults: "", results: ""
        }
    });
});