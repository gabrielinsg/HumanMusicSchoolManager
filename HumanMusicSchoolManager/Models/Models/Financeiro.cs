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
        public virtual FormaPagamento FormaPagamento { get; set; }

        [Required(ErrorMessage = "O campo Data gerada é obrigatório")]
        [Display(Name = "Data gerada")]
        public DateTime UltimaAlteracao { get; set; }

        [Required(ErrorMessage = "O campo Data de vencimento é obrigatório")]
        [Display(Name = "Data de vencimento")]
        public DateTime DataVencimento { get; set; }

        [Display(Name = "Data do pagamento")]
        public DateTime? DataPagamento { get; set; }

        public int? PessoaId { get; set; }
        public virtual Pessoa Pessoa { get; set; }

        public int? AlunoId { get; set; }
        public virtual Aluno Aluno { get; set; }

        public virtual PacoteCompra PacoteCompra { get; set; }
    }
}
