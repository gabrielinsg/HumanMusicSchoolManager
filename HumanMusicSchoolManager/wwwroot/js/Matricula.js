$(function () {
   //$(document).ready(function () {


        // Carrega os Cursos
       $('#cmbCurso').ready(function (e) {
           $('#btnCurso').hide();
            $('#mensagem').html('<span class="mensagem">Aguarde, carregando ...</span>');
            //$.getJSON('matricula/buscarCurso', function (dados) {
            $.ajax({
                url: '/matricula/buscarCurso',
                type: 'GET',
                success: function (dados) {
                    if (dados.length > 0) {
                        var option = '<option>Selecione o Curso</option>';
                        $.each(dados, function (i, obj) {
                            if ($('#professor').attr('value') == obj.id) {
                                option += '<option selected="selected" value="' + obj.id + '">' + obj.nome + '</option>';
                                
                            }
                            else {
                                option += '<option value="' + obj.id + '">' + obj.nome + '</option>';
                            }
                        })
                        $('#mensagem').html('<span class="mensagem">Total de cursos encontrados.: ' + dados.length + '</span>');
                        $('#cmbCurso').html(option).show();
                    } else {
                        Reset();
                        $('#mensagem').html('<span class="mensagem">Não foram encontrados cursos!</span>');
                    }
                },
                error: function (error) {
                    alert("Deu errado");
                    $(that).remove();
                    DisplayError(error.statusText);
                }
            })
        })

        //Carrega as Cidades
        $('#cmbCurso').change(function (e) {
            var cursoId = $('#cmbCurso').val();
            $('#mensagem').html('<span class="mensagem">Aguarde, carregando ...</span>');

            //$.getJSON('matricula/buscarProfessor?cursoId=' + curso, function (dados) {
            $.ajax({
                url: '/matricula/buscarProfessor',
                type: 'POST',
                data: {cursoId: cursoId},
                success: function (dados) {
                    if (dados.length > 0) {
                        var option = '<option>Selecione o Professor</option>';
                        $.each(dados, function (i, obj) {
                            option += '<option value="' + obj.id + '">' + obj.nome + '</option>';
                        })
                        $('#mensagem').html('<span class="mensagem">Total de cursos encontrados.: ' + dados.length + '</span>');
                        $('#cmbProfessor').html(option).show();
                    } else {
                        Reset();
                        $('#mensagem').html('<span class="mensagem">Não foram encontrados professor!</span>');
                    }
                },
                error: function (error) {
                    alert("Deu errado");
                    $(that).remove();
                    DisplayError(error.statusText);
                }
            })
        })

        //Resetar Selects
        function Reset() {
            $('#cmbCurso').empty().append('<option>Carregar Curso</option>>');
            $('#cmbProfessor').empty().append('<option>Carregar Professor</option>');
       }

   //});
})