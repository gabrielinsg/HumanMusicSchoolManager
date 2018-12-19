using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.ServicesInterface
{
    public interface IEmailConfigService
    {
        void Cadastrar(EmailConfig emailConfig);
        void Alterar(EmailConfig emailConfig);
        EmailConfig Buscar();
        void Excluir();
    }
}
