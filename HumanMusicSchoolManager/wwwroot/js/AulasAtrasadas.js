function liberar(aulaId) {
    var alert = $('.alert');
    alert.find('#msg').text('Aguarde')
    alert.removeClass('alert-success')
    alert.removeClass('alert-danger')
    alert.addClass('alert-secondary')
    alert.removeClass('alert-none');
    $.ajax({
        url: '/Aula/AddPrazoLancamentoAula',
        type: 'POST',
        data: { aulaId: aulaId },
        success: result => {
            console.log(result)
            if (result.status == 'Sucess') {
                var alert = $('.alert');
                alert.find('#msg').text('Aula liberada para lançamento')
                alert.removeClass('alert-danger')
                alert.removeClass('alert-secondary')
                alert.addClass('alert-success')
                alert.removeClass("alert-none");
                var td = $('#' + aulaId);
                td.remove();
            }
            else {
                var alert = $('.alert');
                alert.find('#msg').text('Não foi possível liberar o lnaçamento para essa aula')
                alert.removeClass('alert-success')
                alert.removeClass('alert-secondary')
                alert.addClass('alert-danger')
                alert.removeClass("alert-none");
            }
        }
    });
}