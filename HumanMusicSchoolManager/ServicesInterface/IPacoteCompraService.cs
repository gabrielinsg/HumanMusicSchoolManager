using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.ServicesInterface
{
    public interface IPacoteCompraService
    {
        void Cadastrar(PacoteCompra pacoteCompra);
        void Alterar(PacoteCompra pacoteCompra);
        List<PacoteCompra> BuscarTodos();
        PacoteCompra BuscarPorId(int pacoteCompraId);
        void Excluir(int pacoteCompraId);
    }
}
