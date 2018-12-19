using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.Models
{
    public class EmailConfig
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "O campo Domínio deve ser preenchido")]
        [Display(Name = "Domínio")]
        public string Dominio { get; set; }

        [Required(ErrorMessage = "O campo E-mail deve ser preenchido")]
        [Display(Name = "E-mail")]
        [EmailAddress(ErrorMessage = "Entre com um E-mail válido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo Senha deve ser preenchido")]
        public string Senha { get; set; }
    }
}
