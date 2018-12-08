using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.ServicesInterface
{
    public interface IRelatorioMatriculaService
    {
        void Cadastrar(RelatorioMatricula relatorioMatricula);
        List<RelatorioMatricula> BuscarPorMatricula(int matriculaId);
        List<RelatorioMatricula> BuscarPorPessoa(int pessoaId);
    }
}
