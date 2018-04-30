using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanMusicSchoolManager.Models.Models;
using Microsoft.AspNetCore.Identity;

namespace HumanMusicSchoolManager.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public int PessoaId { get; set; }
        public Pessoa Pessoa { get; set; }
    }
}
