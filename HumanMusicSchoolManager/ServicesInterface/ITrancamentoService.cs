using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.ServicesInterface
{
    public interface ITrancamentoService
    {
        void Cadastrar(Trancamento trancamento);
        void Alterar(Trancamento trancamento);
        Trancamento BuscarPorId(int trancamentoId);
        List<Trancamento> BuscarTodos();
        List<Trancamento> BuscarEntreDatas(DateTime dataInicial, DateTime dataFinal);
        void Excluir(int trancamentoId);
    }
}
