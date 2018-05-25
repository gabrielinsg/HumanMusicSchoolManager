using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Services
{
    public interface IAlunoService
    {
        Aluno Cadastrar(Aluno aluno);
        void Alterar(Aluno aluno);
        List<Aluno> BuscarTodos();
        Aluno BuscarPorId(int alunoId);
        void Excluir(int alunoId);
        bool VerificarRm(int rm);
    }
}
