using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.Models
{
    public class Professor : Pessoa
    {
        public List<CursoProfessor> Cursos { get; set; }
        public List<Aula> Aulas { get; set; }

        public Professor()
        {
            this.Cursos = new List<CursoProfessor>();
        }

        public void IncluiCurso(Curso curso)
        {
            this.Cursos.Add(new CursoProfessor() { Curso = curso });
        }

        public void RemoveCurso(Curso curso)
        {
            var cProfessor = this.Cursos.Where(c => c.Curso == curso).Single();
            this.Cursos.Remove(cProfessor);
        }
    }
}
