using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.ViewModels
{
    public class CandidatoViewModel
    {
        public Candidato Candidato { get; set; }
        public Demostrativa Demostrativa { get; set; }
        public bool Contratou { get; set; }
    }
}
