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

        [Display(Name = "Disponibilidade de sala")]
        public int? DispSalaId { get; set; }
        public DispSala DispSala { get; set; }

        [Required]
        public int? RespFinanceiroId { get; set; }
        [Display(Name = "Responsável Financeiro")]
        public RespFinanceiro RespFinanceiro { get; set; }

        [Required]
        public bool Ativo { get; set; }

        [Required]
        [Display(Name = "Data da matrícula")]
        public DateTime DataMatricula { get; set; }

        public DateTime? EncerramentoMatricula { get; set; }

        [Required]
        public List<PacoteCompra> PacoteCompras { get; set; }

        public int? Estrelas { get; set; }

        [Display(Name = "Módulo")]
        public int Modulo { get; set; }
    }
}
