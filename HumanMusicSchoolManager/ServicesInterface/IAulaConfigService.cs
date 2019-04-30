using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.ServicesInterface
{
    public interface IAulaConfigService
    {
        void Cadastrar(AulaConfig aulaConfig);
        void Alterar(AulaConfig aulaConfig);
        AulaConfig Buscar();
    }
}
