

EntradaSaida();




async function EntradaSaida() {
    try {
        let dados = await buscarDados('/grafico/entradasaida');

        let resultado = { label: ['Matriculas', 'Encerramentos'] };
        const matriculas = dados.map(curso => curso.Matriculas);
        const totalMatriculas = matriculas.reduce((soma, curso) => soma + curso);
        const encerramentos = dados.map(curso => curso.Encerramentos);
        const totalEncerramentos = encerramentos.reduce((soma, curso) => soma + curso);
        resultado.data = [totalMatriculas, totalEncerramentos];

        let config = {
            type: 'pie',
            data: {
                datasets: [{
                    data: resultado.data,
                    backgroundColor: getRandomColor(dados.length)                    
                }],
                labels: resultado.label
            },
            options: {
                responsive: true
            }
        };

        let ctx = document.getElementById('entradas-saidas').getContext('2d');
        window.myPie = new Chart(ctx, config);
    } catch (erro) {
        console.log(erro);
    }
    
}


