using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.ServicesInterface
{
    public interface IPacoteCompraService
    {
        PacoteCompra Cadastrar(PacoteCompra pacoteCompra);
        PacoteCompra Alterar(PacoteCompra pacoteCompra);
        List<PacoteCompra> BuscarTodos();
        PacoteCompra BuscarPorId(int pacoteCompraId);
        void Excluir(int pacoteCompraId);
        List<PacoteCompra> FaltasSeguidas();
        List<PacoteCompra> UtimaAulaPorPeriodo(DateTime inicial, DateTime final);
        PacoteCompra CalendarioAluno(int pacoteCompraId, DateTime inicial, DateTime final);
    }
}
