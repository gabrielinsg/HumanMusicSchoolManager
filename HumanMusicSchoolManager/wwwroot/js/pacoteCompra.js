$(document).ready(function () {
    carregarDados();
});

$("#PacoteCompra_Desconto").keyup(function () {
    carregarDados();
});

$("#PacoteCompra_QtdParcela").on('change', function () {
    carregarDados();
});

$("#FormaPagamento").on('change', function () {
    carregarDados();
});

function carregarDados() {
    var valor = parseFloat($("#PacoteAula_Valor").val());
    var desconto = $("#PacoteCompra_Desconto").val();
    var parcelas = parseInt($("#PacoteCompra_QtdParcela").val());
    var formaPagamento = parseInt($("#FormaPagamento").val());
    if (valor === "") {
        valor = 0;
    }
    if (desconto === "") {
        desconto = 0;
    }
    else {
        desconto = parseFloat(desconto);
    }
    if (parcelas === "") {
        parcelas = 0;
    }

    switch (formaPagamento) {
        case 0: formaPagamento = "Boleto"; break;
        case 1: formaPagamento = "Crédito"; break;
        case 2: formaPagamento = "Débito"; break;
        case 3: formaPagamento = "Dinheiro"; break;
        case 4: formaPagamento = "Cheque"; break;
    }

    var total = valor - desconto;
    var valorParcela = total / parcelas;
    total = total.toLocaleString("pt-BR", { style: "currency", currency: "BRL" });
    valorParcela = valorParcela.toLocaleString("pt-BR", { style: "currency", currency: "BRL" });

    $("#valorPacote").text(total);
    $("#parcelas").text(parcelas);
    $("#valorParcela").text(valorParcela);
    $("#formaPagamento").text(formaPagamento);
}