using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.ServicesInterface
{
    public interface IProfessorService
    {
        void Cadastrar(Professor professor);
        void Alterar(Professor professor);
        List<Professor> BuscarTodos();
        Professor BuscarPorId(int id);
        void Excluir(Professor professor);
        void AdicionarCurso(int professorId, int cursoId);
        void RemoverCurso(int professorId, int cursoId);
        List<Professor> BuscarProfessorPorCurso(int cursoId);
        Professor BuscarPorCPF(string cpf);
        List<Professor> BuscarPorNome(string nome);
        Professor BuscarPorIdData(int professorId, DateTime inicial, DateTime final);
        List<Professor> BuscarTodosData(DateTime inicial, DateTime final);
        ProfessorCompletoViewModel RelatorioCompleto(int professorId, DateTime inicial, DateTime final);
    }
}
