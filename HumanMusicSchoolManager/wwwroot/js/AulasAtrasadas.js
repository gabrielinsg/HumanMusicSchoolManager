function liberar(aulaId) {
    let isAlert = $('.alert');
    let btn = $('.btnLiberar');
    isAlert.find('#msg').text('Aguarde');
    btn.addClass('disabled');
    isAlert.removeClass('alert-success');
    isAlert.removeClass('alert-danger');
    isAlert.addClass('alert-secondary');
    isAlert.removeClass('alert-none');
    $.ajax({
        url: '/Aula/AddPrazoLancamentoAula',
        type: 'POST',
        data: { aulaId: aulaId },
        success: result => {
            console.log(result);
            if (result.status === 'Sucess') {
                var I=isAlert = $('.alert');
                isAlert.find('#msg').text('Aula liberada para lançamento');
                btn.removeClass('disabled');
                isAlert.removeClass('alert-danger');
                isAlert.removeClass('alert-secondary');
                isAlert.addClass('alert-success');
                isAlert.removeClass("alert-none");
                var td = $('#' + aulaId);
                td.remove();
            }
            else {
                var isAlert = $('.alert');
                isAlert.find('#msg').text('Não foi possível liberar o lnaçamento para essa aula');
                isAlert.removeClass('alert-success');
                isAlert.removeClass('alert-secondary');
                isAlert.addClass('alert-danger');
                isAlert.removeClass("alert-none");
                btn.removeClass('disabled');
            }
        }
    });
}