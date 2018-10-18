using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.Models
{
    public class PacoteAula
    {
        public int? Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        [Display(Name = "Qunatidade de aulas")]
        public int QtdAula { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Valor inválido")]
        public decimal Valor { get; set; }

        [Required]
        [Display(Name = "Número de parcelas")]
        public int Parcela { get; set; }

        [Required]
        [Display(Name = "Ativo")]
        public bool Ativo { get; set; }
    }
}
