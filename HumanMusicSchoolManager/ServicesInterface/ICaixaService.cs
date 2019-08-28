using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.ServicesInterface
{
    public interface ICaixaService
    {
        void AbrirCaixa(Funcionario funcionario);
        void FecharCaixa(Funcionario funcionario);
        List<Caixa> ListarCaixas(DateTime inical, DateTime final);
        Caixa BuscarCaixa(int caixaId);
        bool CaixaAberto();
    }
}
