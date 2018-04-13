using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.ViewModels
{
    public class AlunoViewModel
    {
        public List<Aluno> Alunos { get; set; }
        public List<Curso> Cursos { get; set; }

        public AlunoViewModel(List<Aluno> alunos, List<Curso> cursos)
        {
            this.Alunos = alunos;
            this.Cursos = cursos;
        }
    }
}
