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
            $.ajax({
                url: "/Matricula/BuscarRespFinanceiro",
                type: "POST",
                dataType: "json",
                data: { respFinanceiroId: ui.item.id },
                success: function (data) {
                    dadosRespFinanceiro(data);
                }
            });
        },
        messages: {
            noResults: "", results: ""
        }
    });

    $("#carregarAluno").click(function () {
        var alunoId = $("#Aluno_Id").val();
        $.ajax({
            url: "/Matricula/BuscarAlunoRespFinanceiro",
            type: "POST",
            dataType: "json",
            data: { alunoId: alunoId },
            success: function (data) {
                dadosRespFinanceiro(data);
            }
        });
    });

    function dadosRespFinanceiro(data) {
        $("#BuscaNome").val("");
        $("#RespFinanceiro_Id").val(data.id);
        $("#RespFinanceiro_Nome").val(data.nome);
        $("#RespFinanceiro_DataNascimento").val(data.dataNascimento.substring(0, 10));
        $("#RespFinanceiro_RG").val(data.rg);
        $("#RespFinanceiro_CPF").val(data.cpf);
        $("#RespFinanceiro_Endereco_Id").val(data.endereco.id);
        $("#RespFinanceiro_EnderecoId").val(data.endereco.id);
        $("#RespFinanceiro_Endereco_Logradouro").val(data.endereco.logradouro);
        $("#RespFinanceiro_Endereco_Numero").val(data.endereco.numero);
        $("#RespFinanceiro_Endereco_Bairro").val(data.endereco.bairro);
        $("#RespFinanceiro_Endereco_Complemento").val(data.endereco.complemento);
        $("#RespFinanceiro_Endereco_Cidade").val(data.endereco.cidade);
        $("#RespFinanceiro_Endereco_UF").val(data.endereco.uf);
        $("#RespFinanceiro_Endereco_CEP").val(data.endereco.cep);
        $("#RespFinanceiro_Tel").val(data.tel);
        $("#RespFinanceiro_Cel").val(data.cel);
        $("#RespFinanceiro_Email").val(data.email);
    }
});