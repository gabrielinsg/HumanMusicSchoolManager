// Set new default font family and font color to mimic Bootstrap's default styling
Chart.defaults.global.defaultFontFamily = '-apple-system,system-ui,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif';
Chart.defaults.global.defaultFontColor = '#292b2c';

// Pie Chart Example

$(document).ready(function () {
    var pacoteCompraId = $("#pacoteCompraId").val();
    var ctx = document.getElementById("myPieChart");
    $.ajax({
        url: "/PacoteCompra/Aulas",
        type: "POST",
        dataType: "json",
        data: { pacoteCompraId: pacoteCompraId },
        success: function (data) {
            dados = data;
            var ctx = document.getElementById("aulaChart");
            var aulaChart = new Chart(ctx, {
                type: 'pie',
                data: {
                    labels: dados.labels,
                    datasets: [{
                        data: dados.datasets,
                        backgroundColor: dados.color,
                    }],
                },
            });
        }
    });

    $.ajax({
        url: "/PacoteCompra/Presencas",
        type: "POST",
        dataType: "json",
        data: { pacoteCompraId: pacoteCompraId },
        success: function (data) {
            dados = data;
            var ctx = document.getElementById("presencaChart");
            var presencaChart = new Chart(ctx, {
                type: 'pie',
                data: {
                    labels: dados.labels,
                    datasets: [{
                        data: dados.datasets,
                        backgroundColor: dados.color,
                    }],
                },
            });
        }
    });
});
