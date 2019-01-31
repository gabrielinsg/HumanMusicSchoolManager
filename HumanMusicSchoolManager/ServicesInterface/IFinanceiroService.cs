using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.ServicesInterface
{
    public interface IFinanceiroService
    {
        void Cadastrar(Financeiro financeiro);
        void Alterar(Financeiro financeiro);
        List<Financeiro> BuscarPorAluno(int alunoId);
        List<Financeiro> BuscarAtrasador();
        List<Financeiro> ParcelasEmAberto(DateTime dataInicial, DateTime dataFinal);
        Financeiro BuscarPorId(int financeiroId);
        void Excluir(int financeiroId);
    }
}
