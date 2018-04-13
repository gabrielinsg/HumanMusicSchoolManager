using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.ViewModels
{
    public class ProfessorCursoViewModel
    {
        public Professor Professor { get; set; }
        public List<Curso> Cursos { get; set; }

        public ProfessorCursoViewModel(Professor professor, List<Curso> cursos)
        {
            this.Professor = professor;
            this.Cursos = cursos;
            foreach (var pCursos in professor.Cursos)
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
