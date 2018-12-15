using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.ViewModels
{
    public class ProfessorPorDataViewModel
    {
        public List<Professor> Professores { get; set; }
        public DateTime Inicial { get; set; }
        public DateTime Final { get; set; }
        public Professor Professor { get; set; }
    }
}
