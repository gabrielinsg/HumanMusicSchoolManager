$(document).ready(function () {
    carregarDados();
});

$("#Desconto").keyup(function () {
    carregarDados();
});

$("#Multa").keyup(function () {
    carregarDados();
});

$("#Valor").keyup(function () {
    carregarDados();
});

$("#ValorPago").keyup(function () {
    carregarDados();
});


function carregarDados() {
    var valor = $("#Valor").val();
    var desconto = $("#Desconto").val();
    var multa = $("#Multa").val();
    var valorPago = $("#ValorPago").val();

    if (valor === "") {
        valor = 0;
    }
    else
    {
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

    if (multa === "") {
        multa = 0;
    }
    else
    {
        multa = multa.replace(',', '.');
        multa = parseFloat(multa);
    }
    if (valorPago === "") {
        valorPago = 0;
    }
    else
    {
        valorPago = valorPago.replace(',', '.');
        valorPago = parseFloat(valorPago);
    }
    

    total = valor - desconto + multa - valorPago;

    $("#total").text(total.toLocaleString("pt-BR", { style: "currency", currency: "BRL" }));

    if (total > valorPago) {
        $("#total").removeClass();
        $("#total").addClass("text-danger");
    }
    else {
        $("#total").removeClass();
        $("#total").addClass("text-success");
    }

}