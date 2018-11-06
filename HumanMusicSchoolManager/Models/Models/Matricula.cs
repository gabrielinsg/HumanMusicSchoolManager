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
        public DispSala DispSala { get; set; }

        [Required]
        public int? RespFinanceiroId { get; set; }
        public RespFinanceiro RespFinanceiro { get; set; }

        [Required]
        public bool Ativo { get; set; }

        [Required]
        public DateTime DataMatricula { get; set; }

        [Required]
        public List<PacoteCompra> PacoteCompras { get; set; }
    }
}
