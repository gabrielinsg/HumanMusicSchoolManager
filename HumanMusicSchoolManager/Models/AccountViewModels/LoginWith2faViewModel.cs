using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.AccountViewModels
{
    public class LoginWith2faViewModel
    {
        [Required]
        [StringLength(7, ErrorMessage = "O {0} deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 4)]
        [DataType(DataType.Text)]
        [Display(Name = "Código autenticador")]
        public string TwoFactorCode { get; set; }

        [Display(Name = "Lembre-se desta máquina")]
        public bool RememberMachine { get; set; }

        public bool RememberMe { get; set; }
    }
}
