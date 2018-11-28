using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.ViewModels
{
    public class DemostrativaViewModel
    {
        public Candidato Candidato { get; set; }
        public Demostrativa Demostrativa { get; set; }
        public DispSala DispSala { get; set; }
        public List<DispSala> DispSalas { get; set; }
        public Curso Curso { get; set; }
        public List<Curso> Cursos { get; set; }
        public DateTime DiaAula { get; set; }
    }
}
