using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.ServicesInterface
{
    public interface IContratoService
    {
        void Cadastrar(Contrato contrato);
        void Alterar(Contrato contrato);
        List<Contrato> BuscarTodos();
        Contrato BuscarPorId(int contratoId);
        void Excluir(int contratoId);
        List<Contrato> BuscarPorNome(string nome);
    }
}
