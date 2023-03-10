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

$("#Valor").keyup(function () {
    carregarDados();
});

function carregarDados() {
    var valor = $("#Valor").val();
    var desconto = $("#PacoteCompra_Desconto").val();
    var parcelas = parseInt($("#PacoteCompra_QtdParcela").val());
    var formaPagamento = parseInt($("#FormaPagamento").val());

    if (valor === "") {
        valor = 0;
    }
    else {
        valor = valor.replace(',', '.');
        valor = parseFloat(valor);
    }

    if (desconto === "") {
        desconto = 0;
    }
    else {
        desconto = desconto.replace(',', '.');
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