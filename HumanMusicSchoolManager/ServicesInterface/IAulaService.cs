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
        Aula BuscarPorDiaHora(DateTime data);
        void Excluir(int aulaId);
    }
}
