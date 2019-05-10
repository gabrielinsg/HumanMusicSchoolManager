function consultar() {
    $.ajax({
        url: '/Aula/AulasAtrasadasJoson',
        type: 'POST',
        success: aulas => {
            if (aulas.length > 0) {
                $('#avisoCount').text(aulas.length);
            }
            let maiorQueDez = false;
            if (aulas.length > 10) {
                aulas = aulas.slice(0, 9);
                maiorQueDez = true;
            }
            aulas.forEach(aula => {
                let data = new Date(aula.data);
                let link = $(`<a class="dropdown-item" href="/Aula/Form?AulaId=${aula.id}">${data.getUTCDate()}/${data.getUTCMonth()+1}/${data.getUTCFullYear()} - ${data.getHours()}:00</a>`);
                $('#conteudoCount').append(link);
            });
            if (maiorQueDez) {
                const maisDez = $('<a class="dropdown-item text-center" href="/Aula/AulasAtrasadas">...mais<a>');
                $('#conteudoCount').append(maisDez);
            }
        }
    });
}

consultar()