$(document).ready(function () {
    $('#calendar').fullCalendar({
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,listYear'
        },
        views: {
            listYear: { buttonText: 'Lista' }
        },
        defaultDate: '2018-11-08',
        navLinks: true, // can click day/week names to navigate views
        editable: false,
        eventLimit: true, // allow "more" link when too many events
        events: 'Feriado/BuscarTodos'

    });
});