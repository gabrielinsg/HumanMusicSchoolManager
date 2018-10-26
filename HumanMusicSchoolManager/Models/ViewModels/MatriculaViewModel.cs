using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.ViewModels
{
    public class MatriculaViewModel
    {
        public Aluno Aluno { get; set; }
        public Matricula Matricula { get; set; }
        public DispSala DispSala { get; set; }
        public Curso Curso { get; set; }
        public RespFinanceiro RespFinanceiro { get; set; }
        public PacoteAula PacoteAula { get; set; }
        public List<PacoteAula> PacotesAula { get; set; }
        public List<DispSala> DispSalas { get; set; }
        public List<Curso> Cursos { get; set; }

        public MatriculaViewModel()
        {
            this.Aluno = new Aluno();
            this.Matricula = new Matricula();
            this.DispSala = new DispSala();
            this.Curso = new Curso();
            this.PacoteAula = new PacoteAula();
            this.RespFinanceiro = new RespFinanceiro();
        }
    }
}
