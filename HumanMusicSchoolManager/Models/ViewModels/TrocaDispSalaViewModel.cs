using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.ViewModels
{
    public class TrocaDispSalaViewModel
    {
        public Matricula Matricula { get; set; }
        public List<DispSala> DispSalas { get; set; }
        public DispSala DispSala { get; set; }
        [Display(Name = "Primeira aula")]
        public DateTime DiaAula { get; set; }
        public List<Curso> Cursos { get; set; }
    }
}
