using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.Models
{
    public class Caixa
    {
        public int Id { get; set; }
        [Required]
        public DateTime DataAberto { get; set; }
        public DateTime? DataFechado { get; set; }
        public int FuncionarioAbertoId { get; set; }
        public virtual Funcionario FuncionarioAberto { get; set; }
        public virtual Funcionario FuncionarioFechado { get; set; }

        public virtual List<TransacaoCaixa> TransacoesCaixa { get; set; }

        public decimal? TotalAnterior { get; set; }

        public decimal Total()
        {
            decimal total = TotalAnterior == null ? 0 : TotalAnterior.Value;
            foreach (var transacaoCaixa in TransacoesCaixa)
            {
                if (transacaoCaixa.Entrada)
                {
                    total += transacaoCaixa.Valor;
                }
                else
                {
                    total -= transacaoCaixa.Valor;
                }
            }
            return total;
        }

        public decimal TotalEntradaDinheiro()
        {
            return (TransacoesCaixa.Where(tc => tc.FormaPagamento == FormaPagamento.DINHEIRO && tc.Entrada).Sum(tc => tc.Valor)) + (TotalAnterior == null ? 0 : TotalAnterior.Value);
        }
        
        public decimal TotalEntradaDebito()
        {
            return TransacoesCaixa.Where(tc => tc.FormaPagamento == FormaPagamento.DEBITO && tc.Entrada).Sum(tc => tc.Valor);
        }

        public decimal TotalEntradaCredito()
        {
            return TransacoesCaixa.Where(tc => tc.FormaPagamento == FormaPagamento.CREDITO && tc.Entrada).Sum(tc => tc.Valor);
        }

        public decimal TotalEntradaCheque()
        {
            return TransacoesCaixa.Where(tc => tc.FormaPagamento == FormaPagamento.CHEQUE && tc.Entrada).Sum(tc => tc.Valor);
        }

        public decimal TotalEntrada()
        {
            return TransacoesCaixa.Where(tc => tc.Entrada).Sum(tc => tc.Valor);
        }

        public decimal TotalSaidaDinheiro()
        {
            return TransacoesCaixa.Where(tc => tc.FormaPagamento == FormaPagamento.DINHEIRO && !tc.Entrada).Sum(tc => tc.Valor);
        }

        public decimal TotalSaidaDebito()
        {
            return TransacoesCaixa.Where(tc => tc.FormaPagamento == FormaPagamento.DEBITO && !tc.Entrada).Sum(tc => tc.Valor);
        }

        public decimal TotalSaidaCredito()
        {
            return TransacoesCaixa.Where(tc => tc.FormaPagamento == FormaPagamento.CREDITO && !tc.Entrada).Sum(tc => tc.Valor);
        }

        public decimal TotalSaidaCheque()
        {
            return TransacoesCaixa.Where(tc => tc.FormaPagamento == FormaPagamento.CHEQUE && !tc.Entrada).Sum(tc => tc.Valor);
        }

        public decimal TotalSaida()
        {
            return TransacoesCaixa.Where(tc => !tc.Entrada).Sum(tc => tc.Valor);
        }

        public decimal TotalDinheiro()
        {
            return TotalEntradaDinheiro() - TotalSaidaDinheiro();
        }

        public decimal TotalDebito()
        {
            return TotalEntradaDebito() - TotalSaidaDebito();
        }

        public decimal TotalCredito()
        {
            return TotalEntradaCredito() - TotalSaidaCredito();
        }

        public decimal TotalCheque()
        {
            return TotalEntradaCheque() - TotalSaidaCheque();
        }

    }
}
