using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.ViewModels
{
    public class CursoViewModel
    {
        public List<Curso> Cursos { get; set; }

        public CursoViewModel(List<Curso> cursos)
        {
            this.Cursos = cursos;
        }
    }
}
