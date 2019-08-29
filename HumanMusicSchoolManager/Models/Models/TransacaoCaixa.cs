using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.Models
{
    public class TransacaoCaixa
    {
        public int Id { get; set; }
        public int CaixaId { get; set; }
        public Caixa Caixa { get; set; }
        [Required]
        public DateTime Data { get; set; }
        [Required(ErrorMessage = "A descrição deve ser preenchida")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }
        [Required(ErrorMessage = "O valor deve ser informado")]
        public decimal Valor { get; set; }
        public bool Entrada { get; set; }
        public int FuncionarioId { get; set; }
        public Funcionario Funcionario { get; set; }
        [Required(ErrorMessage = "A forma de pagamento deve ser informada")]
        public FormaPagamento FormaPagamento { get; set; }
    }
}