$(document).ready(function () {
    carregarDados();
});

$("#Multa").keyup(function () {
    carregarDados();
});

$("#Desconto").keyup(function () {
    carregarDados();
});

function carregarDados() {
    var subTotal = $("#subTotal").val();
    var desconto = $("#Desconto").val();
    var multa = $("#Multa").val();

    if (subTotal === "") {
        subTotal = 0;
    }
    else {
        subTotal = subTotal.replace(',', '.');
        subTotal = parseFloat(subTotal);
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
    else {

        multa = multa.replace(',', '.');
        multa = parseFloat(multa);
    }
    
    
    
    var total = subTotal - desconto + multa;
    total = total.toLocaleString("pt-BR", { style: "currency", currency: "BRL" });
    $("#total").text(total);
}