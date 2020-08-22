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
        public virtual Aluno Aluno { get; set; }

        public int CursoId { get; set; }
        public virtual Curso Curso { get; set; }

        [Display(Name = "Disponibilidade de sala")]
        public int? DispSalaId { get; set; }
        public virtual DispSala DispSala { get; set; }

        [Required]
        public int? RespFinanceiroId { get; set; }
        [Display(Name = "Responsável Financeiro")]
        public virtual RespFinanceiro RespFinanceiro { get; set; }

        [Required]
        public bool Ativo { get; set; }

        [Required]
        [Display(Name = "Data da matrícula")]
        public DateTime DataMatricula { get; set; }

        public DateTime? EncerramentoMatricula { get; set; }

        [Required]
        public virtual List<PacoteCompra> PacoteCompras { get; set; }

        public int? Estrelas { get; set; }

        [Display(Name = "Módulo")]
        public int Modulo { get; set; }

        public Motivo? Motivo { get; set; }

        public string Outros { get; set; }

        public int? ProfessorId { get; set; }
        public virtual Professor Professor { get; set; }
    }
}
