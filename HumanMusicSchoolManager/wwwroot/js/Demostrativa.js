function Confimacao(id, demostrativaId) {
    const confirmado = $('#'+id).val();
    $.post("/Demostrativa/AutoSaveConfimado", { id: demostrativaId, confirmado: confirmado });
}