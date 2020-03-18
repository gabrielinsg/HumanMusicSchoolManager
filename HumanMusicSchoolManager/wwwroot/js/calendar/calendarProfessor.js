
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
var professorId = $("#professorId").val();
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
    events: '/Professor/CalendarioJson?professorId=' + professorId + '&user=' + user,
    eventAfterAllRender: alerta

});

function alerta() {

    try {

        carregarIcones();
    }
    catch (e){
        console.log("error " + e);
    }

    let more = document.querySelectorAll('.fc-more');
    for (let i = 0; i < more.length; i++) {
        more[i].addEventListener('click', carregarIcones);
    }

}

function carregarIcones() {
    const itens = document.querySelectorAll('div.fc-content');
    for (i = 0; i <= itens.length; i++) {
        let lastChild = itens[i].lastChild;
        if (lastChild.innerText.substring(0, 3) === '<G>') {
            let icone = document.createElement('i');
            icone.className += 'fas fa-users';
            itens[i].insertBefore(icone, lastChild);
            lastChild.innerText = ' ' + lastChild.innerText.substring(4, lastChild.innerText.length);
        } else if (lastChild.innerText.substring(0, 3) === '<I>') {
            let icone = document.createElement('i');
            icone.className += 'fas fa-user';
            itens[i].insertBefore(icone, lastChild);
            lastChild.innerText = ' ' + lastChild.innerText.substring(4, lastChild.innerText.length);
        }
    }
}


