using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.ServicesInterface
{
    public interface IValeService
    {
        void Lancar(Vale vale);
        List<Vale> BuscarPorPessoa(int pessoaId, DateTime inicial, DateTime final);
        List<Vale> BuscarTodos(DateTime inicial, DateTime final);
        Vale BuscarPorId(int valeId);
        void Excluir(int valeId);
    }
}
