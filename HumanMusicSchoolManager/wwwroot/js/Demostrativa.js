function Confimacao(id, DemostrativaId) {
    const confirmado = $('#'+id).val();
    $.post("/Demostrativa/AutoSaveConfimado", { id: DemostrativaId, confirmado: confirmado });
}