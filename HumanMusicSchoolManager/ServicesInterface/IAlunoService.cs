using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.ServicesInterface
{
    public interface IAlunoService
    {
        Aluno Cadastrar(Aluno aluno);
        void Alterar(Aluno aluno);
        List<Aluno> BuscarTodos();
        Aluno BuscarPorId(int alunoId);
        void Excluir(int alunoId);
        Aluno BuscarPorCPF(string CPF);
        List<Aluno> BuscarPorNome(string nome);
        List<Aluno> BuscarPorCPFLike(string cpf);
    }
}
