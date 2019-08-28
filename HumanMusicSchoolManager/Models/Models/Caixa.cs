﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public Funcionario FuncionarioAberto { get; set; }
        public Funcionario FuncionarioFechado { get; set; }
        [Required(ErrorMessage = "Valor do caixa a ser aberto deve ser informado")]
        public decimal ValorAberto { get; set; }
        public List<TransacaoCaixa> TransacoesCaixa { get; set; }

        public decimal Total()
        {
            decimal total = 0;
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
            return TransacoesCaixa.Where(tc => tc.FormaPagamento == FormaPagamento.DINHEIRO && tc.Entrada).Sum(tc => tc.Valor);
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

    }
}