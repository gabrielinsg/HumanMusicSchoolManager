using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.Models
{
    public class PacoteCompra
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Pacote de aula obrigatório")]
        [Display(Name = "Pacote de Aula")]
        public PacoteAula PacoteAula { get; set; }

        [Required(ErrorMessage = "Matricula obrigatório")]
        [Display(Name = "Matrícula")]
        public Matricula Matricula { get; set; }

        [Required(ErrorMessage = "O campo Data da compra é obrigatório")]
        [Display(Name = "Data da compra")]
        public DateTime DataCompra { get; set; }

        [Required(ErrorMessage = "O campo número de parcelas é obrigatório")]
        [Display(Name = "Quantidade de parcelas")]
        public int QtdParcela { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Valor inválido")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Desconto { get; set; }
    }
}
