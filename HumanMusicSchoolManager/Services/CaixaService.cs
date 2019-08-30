using HumanMusicSchoolManager.Data;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.ServicesInterface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Services
{
    public class CaixaService : ICaixaService
    {
        private readonly ApplicationDbContext _context;

        public CaixaService(ApplicationDbContext context)
        {
            this._context = context;
        }

        public void AbrirCaixa(Funcionario funcionario)
        {
            var caixaAnterior = _context.Caixas.OrderByDescending(c => c.Id).Include(c => c.TransacoesCaixa).FirstOrDefault();
            var caixa = new Caixa(){
                DataAberto = NowHorarioBrasilia.GetNow(),
                FuncionarioAberto = funcionario,
                TotalAnterior = caixaAnterior == null ? 0 : caixaAnterior.TotalDinheiro()
            };
            
            _context.Caixas.Add(caixa);
            _context.SaveChanges();
        }

        public Caixa BuscarCaixa(int caixaId)
        {
            return _context.Caixas
                .Include(c => c.TransacoesCaixa)
                .ThenInclude(t => t.Funcionario)
                .Include(c => c.FuncionarioAberto)
                .Include(c => c.FuncionarioFechado)
                .FirstOrDefault(c => c.Id == caixaId);
        }

        public bool CaixaAberto()
        {
            var caixa = _context.Caixas.FirstOrDefault(c => c.DataFechado == null);
            return caixa == null ? true : false;
        }

        public void FecharCaixa(Funcionario funcionario)
        {
            var caixa = _context.Caixas.OrderByDescending(c => c.Id).FirstOrDefault();
            if (caixa != null)
            {
                caixa.FuncionarioFechado = funcionario;
                caixa.DataFechado = NowHorarioBrasilia.GetNow();
            }
            _context.Caixas.Update(caixa);
            _context.SaveChanges();
        }

        public void IncluirTransacao(TransacaoCaixa transacaoCaixa)
        {
            _context.TransacoesCaixa.Add(transacaoCaixa);
            _context.SaveChanges();
        }

        public List<Caixa> ListarCaixas(DateTime inicial, DateTime final)
        {
            inicial = DateTimeEntradaSaida.Inicial(inicial);
            final = DateTimeEntradaSaida.Final(final);

            return _context.Caixas.Where(c => c.DataAberto >= inicial && c.DataAberto <= final)
                .Include(c => c.TransacoesCaixa)
                .Include(c => c.FuncionarioAberto)
                .Include(c => c.FuncionarioFechado)
                .ToList();
        }

        public Caixa BuscarCaixaAberto()
        {
            return _context.Caixas
                .Include(c => c.TransacoesCaixa)
                .ThenInclude(t => t.Funcionario)
                .Include(c => c.FuncionarioAberto)
                .Include(c => c.FuncionarioFechado)
                .FirstOrDefault(c => c.DataFechado == null);
        }
    }
}
