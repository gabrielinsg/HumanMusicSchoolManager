using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.ViewModels
{
    public class SalaCursoViewModel
    {
        public Sala Sala { get; set; }
        public List<Curso> Cursos { get; set; }

        public SalaCursoViewModel(Sala sala, List<Curso> cursos)
        {
            this.Sala = sala;
            this.Cursos = cursos;
            foreach (var pCursos in sala.Cursos)
            {
                var curso = cursos.Where(c => c.Id == pCursos.Curso.Id).SingleOrDefault();
                if (curso != null)
                {
                    Cursos.Remove(curso);
                }
            }
        }
    }
}
