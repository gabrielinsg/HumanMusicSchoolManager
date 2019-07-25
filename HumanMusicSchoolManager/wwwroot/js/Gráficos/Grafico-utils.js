
function buscarDados(url) {
    return new Promise(resolve => {
        $.ajax({
            type: "POST",
            url: url,
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