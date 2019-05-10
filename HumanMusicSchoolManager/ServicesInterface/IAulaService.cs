using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.ServicesInterface
{
    public interface IAulaService
    {
        void Cadastrar(Aula aula);
        void Alterar(Aula aula);
        List<Aula> BuscarTodas();
        Aula BuscarPorId(int aulaId);
        Aula BuscarPorDiaHora(DateTime data, DispSala dispSala);
        void Excluir(int aulaId);
        List<Aula> BuscarPorSala(int salaId);
        List<Aula> Atrasadas();
        List<Aula> AtrasadasPorProfessor(int professorId);
        void AtualizarDescAtividades(int id, string descAtividades);
        void AlterarTempoLimite(int TempoLancamento);
        List<Aula> CalendarioSala(int salaId, DateTime inicial, DateTime final);
    }
}
