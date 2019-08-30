using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.ServicesInterface
{
    public interface IRelatorioService
    {
        List<Professor> Professores();
        List<Aluno> Alunos();
        List<PacoteCompra> PacotesAtivos();
        List<PacoteCompra> PacotesContratados(DateTime inicial, DateTime final);
        List<Matricula> MatriculasCanceladas(DateTime inicial, DateTime final);
        List<Matricula> MatriculasNovas(DateTime inicial, DateTime final);
        List<Aluno> AlunosAniversariantes(int mes);
    }
}
