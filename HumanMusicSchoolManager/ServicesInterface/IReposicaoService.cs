using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.ServicesInterface
{
    public interface IReposicaoService
    {
        void Cadastrar(Reposicao reposicao);
        void Alterar(Reposicao reposicao);
        List<Reposicao> BuscarTodos();
        Reposicao BuscarPorId(int reposicaoId);
        Reposicao BuscarPorChamada(int chamadaId);
        void Excluir(int reposicaoId);
    }
}
