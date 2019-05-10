function confirmar() {
    var cf = confirm("Tem certeza que deseja remanejar a aula para o final do calendário? Essa operação não poderá ser desfeita!");
    if (cf === true) {
        return true;
    } else {
        return false;
    }
}