$(document).ready(function () {

    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!

    var yyyy = today.getFullYear();
    if (dd < 10) {
        dd = '0' + dd;
    }
    if (mm < 10) {
        mm = '0' + mm;
    }
    var date = yyyy + '-' + mm + '-' + dd;
    var salaId = $("#salaId").val();
    var user = $("#user").val();
    $('#calendar').fullCalendar({
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek,agendaDay,listWeek'
        },
        views: {
            listYear: { buttonText: 'Lista' },
            month: { buttonText: 'Mês' },
            agendaWeek: { buttonText: 'Semana' },
            agendaDay: { buttonText: 'Dia' }
        },
        defaultDate: date,
        navLinks: true, // can click day/week names to navigate views
        editable: false,
        eventLimit: true, // allow "more" link when too many events
        events: '/Sala/CalendarioJson?salaId=' + salaId

    });
});