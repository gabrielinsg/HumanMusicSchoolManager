using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.ViewModels
{
    public class DiarioClasseViewModel
    {
        public List<DiarioClasse> DiariosClasse { get; set; }

        public DiarioClasseViewModel(List<DiarioClasse> diariosClasse)
        {
            this.DiariosClasse = diariosClasse;
        }
    }
}
