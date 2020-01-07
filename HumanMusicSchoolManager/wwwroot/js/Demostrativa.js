function Confimacao(id, DemostrativaId) {
    const confirmado = $('#' + id).val();
    $.post("/Demostrativa/AutoSaveConfimado", { id: DemostrativaId, confirmado: confirmado });
}

function Presenca(id, DemostrativaId) {
    const presenca = $('#presenca' + id).val();
    const presencatxt = $('#presencatxt' + id + ' p')[0];

    $.post("/Demostrativa/AutoSavePresenca", { id: DemostrativaId, presenca: presenca });

    switch (presenca) {
        case 'true': presencatxt.innerText = 'Candidato presente'; break;
        case 'false': presencatxt.innerText = 'Candidato faltou'; break;
        default: presencatxt.innerText = 'Presença não lançada'; break;
    }   
}