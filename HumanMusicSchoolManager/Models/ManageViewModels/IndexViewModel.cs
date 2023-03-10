using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.ManageViewModels
{
    public class IndexViewModel
    {
        [Display(Name = "Login")]
        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Número de telefone")]
        public string PhoneNumber { get; set; }

        public string StatusMessage { get; set; }
    }
}
