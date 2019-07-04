carregarDdados();

function buscarDados() {
    return new Promise(resolve => {
        $.ajax({
            type: "POST",
            url: '/grafico/entradasaida',
            data: {
                inicial: '2019-06-01',
                final: '2019-06-30'
            },
            success: data => {
                resolve(data);
            }
        });
    });
} 
    

async function carregarDdados() {
    try {
        let dados = await buscarDados();
        console.log(dados);
        let tEntradaSaida = totalEntradaSaida(dados);
        gerarGrafico('Total Entradas e Saídas', tEntradaSaida.label, tEntradaSaida.data);
    } catch (erro) {
        console.log(erro);
    }
    
}

//Gerar cores aleatórias
function getRandomColor(n) {
    let cores = [];
    for (let i = 0; i < n; i++) {
        const letters = '0123456789ABCDEF';
        let color = '#';
        for (let i = 0; i < 6; i++) {
            color += letters[Math.floor(Math.random() * 16)];
        }
        cores.push(color);
    }
    
    return cores;
}


function gerarGrafico(titulo, labels, dados) {
    let config = {
        type: 'pie',
        data: {
            datasets: [{
                data: dados,
                backgroundColor: getRandomColor(dados.length),
                label: titulo
            }],
            labels: labels
        },
        options: {
            responsive: true
        }
    };


     let ctx = document.getElementById('chart-area').getContext('2d');
     window.myPie = new Chart(ctx, config);

}


function totalEntradaSaida(dados) {
    let resultado = { label: ['Matriculas', 'Encerramentos'] };
    const matriculas = dados.map(curso => curso.Matriculas);
    const totalMatriculas = matriculas.reduce((soma, curso) => soma + curso);
    const encerramentos = dados.map(curso => curso.Encerramentos);
    const totalEncerramentos = encerramentos.reduce((soma, curso) => soma + curso);
    resultado.data = [totalMatriculas, totalEncerramentos];
    return resultado;
}