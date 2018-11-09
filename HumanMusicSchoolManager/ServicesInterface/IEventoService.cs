using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.ServicesInterface
{
    public interface IEventoService
    {
        List<Evento> BuscarTodos();
        void Cadastrar(Evento evento);
        void Alterar(Evento evento);
        Evento BuscarPorId(int eventoId);
        Evento BuscarPorData(DateTime data);
        List<Evento> BuscarEntreDatas(DateTime dataInicial, DateTime dataFinal);
        void Excluir(int eventoId);
    }
}
