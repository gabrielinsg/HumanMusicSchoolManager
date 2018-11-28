using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.ServicesInterface
{
    public interface ICandidatoService
    {
        void Cadastrar(Candidato candidato);
        void Alterar(Candidato candidato);
        List<Candidato> BuscarTodos();
        Candidato BuscarPorId(int candidatoId);
        void Excluir(int candidatoId);
        List<Candidato> BuscarPorNome(string nome);
    }
}
