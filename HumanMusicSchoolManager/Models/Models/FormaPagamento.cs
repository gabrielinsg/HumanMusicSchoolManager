using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.Models
{
    public enum FormaPagamento
    {
        [Display(Name = "Boleto")]
        BOLETO,
        [Display(Name = "Crédito")]
        CREDITO,
        [Display(Name = "Débito")]
        DEBITO,
        [Display(Name = "Dinheiro")]
        DINHEIRO,
        [Display(Name = "Cheque")]
        CHEQUE
    }
}
