using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.Models
{
    public class PacoteAula
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "O Campo Nome é obrigatório")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo Quantidade de aulas é obrigatório")]
        [Display(Name = "Qunatidade de aulas")]
        public int QtdAula { get; set; }

        [Required(ErrorMessage = "O campo Valor é obrigatório")]
        [Range(0, double.MaxValue, ErrorMessage = "Valor inválido")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "O campo Parcela é obrigatório")]
        [Display(Name = "Número de parcelas")]
        public int Parcela { get; set; }

        [Required(ErrorMessage = "O campo Tipo de aula é obrigatório")]
        [Display(Name = "Tipo de aula")]
        public TipoAula TipoAula { get; set; }

        [Required]
        [Display(Name = "Ativo")]
        public bool Ativo { get; set; }
    }
}

public enum TipoAula
{
    [Display(Name = "Grupo")]
    GRUPO,
    [Display(Name = "Individual")]
    INDIVIDUAL
}