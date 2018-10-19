using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.ServicesInterface
{
    public interface ISalaService
    {
        Sala Cadastrar(Sala sala);
        void Alterar(Sala sala);
        List<Sala> BuscarTodos();
        Sala BuscarPorId(int salaId);
        void Excluir(int salaId);
        List<Sala> BuscarPorNome(string nome);
        void AdicionarCurso(int salaId, int cursoId);
        void RemoverCurso(int salaId, int cursoId);
    }
}
