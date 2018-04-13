using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.ViewModels
{
    public class ProfessorViewModel
    {
        public List<Professor> Professores { get; set; }
        public List<Curso> Cursos { get; set; }

        public ProfessorViewModel(List<Professor> professores, List<Curso> cursos)
        {
            this.Professores = professores;
            this.Cursos = cursos;
        }
    }
}
