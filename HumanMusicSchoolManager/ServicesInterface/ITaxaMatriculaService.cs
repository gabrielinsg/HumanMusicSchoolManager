using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.ServicesInterface
{
    public interface ITaxaMatriculaService
    {
        void Cadastrar(TaxaMatricula taxaMatricula);
        void Alterar(TaxaMatricula taxaMatricula);
        void Excluir(int taxaMatriculaId);
        List<TaxaMatricula> BuscarTodos();
        TaxaMatricula BuscarPorId(int taxaMatriculaId);
        TaxaMatricula BuscarPorNome(string nome);
    }
}
