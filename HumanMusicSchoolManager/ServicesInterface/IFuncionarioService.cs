using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.ServicesInterface
{
    public interface IFuncionarioService
    {
        void Cadastrar(Funcionario funcionario);
        void Alterar(Funcionario funcionario);
        void Excluir(int funcionarioId);
        Funcionario BuscarPorId(int funcionarioId);
        List<Funcionario> BuscarTodos();
        List<Funcionario> BuscarPorNome(string nome);
        Funcionario BuscarPorCPF(string CPF);
    }
}
