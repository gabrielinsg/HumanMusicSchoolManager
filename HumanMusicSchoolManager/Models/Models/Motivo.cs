using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.Models
{
    public enum Motivo
    {
        [Display(Name = "Financeiro")]
        FINANCEIRO,
        [Display(Name = "Falta de tempo")]
        TEMPO,
        [Display(Name = "Não gostou do professor")]
        PROFESSOR,
        [Display(Name = "Não gostou da aula")]
        AULA,
        [Display(Name = "Não gostou da escola")]
        ESCOLA,
        [Display(Name = "Não retornou o contato")]
        RETORNO,
        [Display(Name = "Outros")]
        OUTROS
    }
}
