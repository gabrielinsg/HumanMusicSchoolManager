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
        public decimal Valor { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Desconto { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Multa { get; set; }

        [Display(Name = "Valor Pago")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorPago { get; set; }

        [Required]
        [Display(Name = "Forma de pagamento")]
        public FormaPagamento FormaPagamento { get; set; }

        [Required]
        [Display(Name = "Data gerada")]
        public DateTime DataGerada { get; set; }

        [Required]
        [Display(Name = "Data de vencimento")]
        public DateTime DateVencimento { get; set; }

        [Required]
        public Funcionario Funcionario { get; set; }
    }

    public enum FormaPagamento
    {
        [Display(Name = "Cartão de Credito")]
        CREDITO,
        [Display(Name = "Cartão de Débito")]
        DEBITO,
        [Display(Name = "Dinheiro")]
        DINHEIRO,
        [Display(Name = "Cheque")]
        CHEQUE,
        [Display(Name = "Boleto")]
        BOLETO
    }
}
