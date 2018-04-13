using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.Models
{
    public class Matricula
    {
        public int? Id { get; set; }

        public int AlunoId { get; set; }
        public Aluno Aluno { get; set; }

        public int CursoId { get; set; }
        public Curso Curso { get; set; }

        [Required]
        public int Dia { get; set; }

        [Required]
        public int Hora { get; set; }

        public int ProfessorId { get; set; }
        public Professor Professor { get; set; }

        [Required]
        public bool Ativo { get; set; }

        [Required]
        public DateTime DataMatricula { get; set; }
    }
}
