using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Services
{
    public interface ICursoService
    {
        void Cadastrar(Curso curso);
        List<Curso> BuscarTodos();
        Curso BuscarPorId(int id);
        void Alterar(Curso curso);
        void Excluir(Curso curso);
        List<Curso> BuscarPorNome(string nome);
        int DucacaoAula(int cursoId);
    }
}
