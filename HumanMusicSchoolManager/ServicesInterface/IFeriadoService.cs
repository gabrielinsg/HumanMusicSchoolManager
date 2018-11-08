
using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.ServicesInterface
{
    public interface IFeriadoService
    {
        List<Feriado> BuscarTodos();
        void Cadastrar(Feriado feriado);
        void Alterar(Feriado feriado);
        Feriado BuscarPorId(int feriadoId);
        Feriado BuscarPorData(DateTime data);
        List<Feriado> BuscarEntreDatas(DateTime dataInicial, DateTime dataFinal);
        void Excluir(int feriadoId);
    }
}
