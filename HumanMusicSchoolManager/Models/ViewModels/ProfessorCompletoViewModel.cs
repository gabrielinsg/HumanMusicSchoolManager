using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.ViewModels
{
    public class ProfessorCompletoViewModel
    {
        public Professor Professor { get; set; }
        public List<Aula> Aulas { get; set; }
        public List<DispSala> DispSalas { get; set; }
        public DateTime Inicial { get; set; }
        public DateTime Final { get; set; }
        public List<Matricula> Matriculas { get; set; }
    }
}
