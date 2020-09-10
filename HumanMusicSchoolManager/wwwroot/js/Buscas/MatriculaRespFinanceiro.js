
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
        var respFinanceiro = JSON.parse(data);
        $("#BuscaNome").val("");
        $("#RespFinanceiro_Id").val(respFinanceiro.Id);
        $("#RespFinanceiro_Nome").val(respFinanceiro.Nome);
        $("#RespFinanceiro_DataNascimento").val(respFinanceiro.DataNascimento.substring(0,10));
        $("#RespFinanceiro_RG").val(respFinanceiro.RG);
        $("#RespFinanceiro_CPF").val(respFinanceiro.CPF);
        $("#RespFinanceiro_Endereco_Id").val(respFinanceiro.Endereco.Id);
        $("#RespFinanceiro_Endereco_Logradouro").val(respFinanceiro.Endereco.Logradouro);
        $("#RespFinanceiro_Endereco_Numero").val(respFinanceiro.Endereco.Numero);
        $("#RespFinanceiro_Endereco_Bairro").val(respFinanceiro.Endereco.Bairro);
        $("#RespFinanceiro_Endereco_Complemento").val(respFinanceiro.Endereco.Complemento);
        $("#RespFinanceiro_Endereco_Cidade").val(respFinanceiro.Endereco.Cidade);
        $("#RespFinanceiro_Endereco_UF").val(respFinanceiro.Endereco.UF);
        $("#RespFinanceiro_Endereco_CEP").val(respFinanceiro.Endereco.CEP);
        $("#RespFinanceiro_Tel").val(respFinanceiro.Tel);
        $("#RespFinanceiro_Cel").val(respFinanceiro.Cel);
        $("#RespFinanceiro_Email").val(respFinanceiro.Email);
        $("#RespFinanceiro_Nacionalidade").val(respFinanceiro.Nacionalidade);
        $("#RespFinanceiro_Naturalidade").val(respFinanceiro.Naturalidade);
        $("#RespFinanceiro_EstadoCivil").val(respFinanceiro.EstadoCivil);
        $("#RespFinanceiro_Profissao").val(respFinanceiro.Profissao);
        $("#RespFinanceiro_OrgaoExpedidor").val(respFinanceiro.OrgaoExpedidor);
        if (data.ativo === true) {
            $("#RespFinanceiro_Ativo").prop("checked", true);
        }
    }
});