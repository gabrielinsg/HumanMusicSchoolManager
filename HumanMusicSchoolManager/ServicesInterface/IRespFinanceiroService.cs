using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.ServicesInterface
{
    public interface IRespFinanceiroService
    {
        void Cadastrar(RespFinanceiro respFinanceiro);
        void Alterar(RespFinanceiro respFinanceiro);
        List<RespFinanceiro> BuscarTodos();
        RespFinanceiro BuscarPorId(int respFinanceiroId);
        List<RespFinanceiro> BuscarPorNome(string nome);
        void Excluir(int respFinanceiroId);
        RespFinanceiro BuscarPorCPF(string cpf);
    }
}
