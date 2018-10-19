using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.ServicesInterface
{
    public interface IMatriculaService
    {
        void Cadastrar(Matricula matricula);
        void Alterar(Matricula matricula);
        List<Matricula> BuscarTodos();
        Matricula BuscarPorId(int matriculaId);
        void Excluir(int matriculaId);
        List<Matricula> BuscarPorProfessor(int professorId);
    }
}
