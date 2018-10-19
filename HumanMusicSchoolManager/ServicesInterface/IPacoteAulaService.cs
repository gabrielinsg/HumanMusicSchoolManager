using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.ServicesInterface
{
    public interface IPacoteAulaService
    {
        void Cadastrar(PacoteAula pacoteAula);
        void Alterar(PacoteAula pacoteAula);
        List<PacoteAula> BuscarTodos();
        PacoteAula BuscarPorId(int pacoteAulaId);
        List<PacoteAula> BuscarPorNome(string nome);
        void Excluir(int pacoteAulaId);
    }
}
