using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.Models
{
    public enum Sexo
    {
        [Display(Name = "Masculino")]
        MASCULINO = 0,
        [Display(Name = "Feminino")]
        FEMININO = 1
    }
}
