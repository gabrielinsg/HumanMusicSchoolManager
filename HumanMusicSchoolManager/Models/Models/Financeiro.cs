using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.Models
{
    public class Financeiro
    {
        public int? Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Valor { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Desconto { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Multa { get; set; }

        [Display(Name = "Valor Pago")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ValorPago { get; set; }
        
        [Display(Name = "Forma de pagamento")]
        public FormaPagamento FormaPagamento { get; set; }

        [Required(ErrorMessage = "O campo Data gerada é obrigatório")]
        [Display(Name = "Data gerada")]
        public DateTime DataGerada { get; set; }

        [Required(ErrorMessage = "O campo Data de vencimento é obrigatório")]
        [Display(Name = "Data de vencimento")]
        public DateTime DataVencimento { get; set; }

        [Required(ErrorMessage = "O Funcionário não foi selecionado")]
        public Pessoa Pessoa { get; set; }

        [Required(ErrorMessage = "O Aluno não foi selecionado")]
        public Aluno Aluno { get; set; }

        public PacoteCompra PacoteCompra { get; set; }
    }
}
