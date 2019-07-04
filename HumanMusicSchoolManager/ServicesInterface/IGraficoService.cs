using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.ServicesInterface
{
    public interface IGraficoService
    {
        Object EntradasSaidas(DateTime inicial, DateTime final, List<Curso> cursos);
    }
}
