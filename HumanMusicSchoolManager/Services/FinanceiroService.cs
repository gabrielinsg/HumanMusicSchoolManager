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
    public class FinanceiroService : IFinanceiroService
    {
        private readonly ApplicationDbContext _context;

        public FinanceiroService(ApplicationDbContext context)
        {
            this._context = context;
        }

        public void Alterar(Financeiro financeiro)
        {
            var entry = _context.Entry<Financeiro>(financeiro);
            if (entry.State == EntityState.Detached)
            {
                var fin = _context.Financeiros.FirstOrDefault(f => f.Id == financeiro.Id);
                var ent = _context.Entry<Financeiro>(fin);
                ent.State = EntityState.Detached;
                entry.State = EntityState.Modified;
            }
            _context.Financeiros.Update(financeiro);
            _context.SaveChanges();
        }

        public List<Financeiro> BuscarAtrasador()
        {
            return _context.Financeiros
                .Where(f => f.DataVencimento < NowHorarioBrasilia.GetNow() && ((f.Valor - (f.Desconto == null ? 0 : f.Desconto) + (f.Multa == null ? 0 : f.Multa)) > (f.ValorPago == null ? 0 : f.ValorPago)))
                .ToList();
        }

        public List<Financeiro> BuscarPorAluno(int alunoId)
        {
            return _context.Financeiros
                .Where(f => f.Aluno.Id == alunoId)
                .ToList();
        }

        public Financeiro BuscarPorId(int financeiroId)
        {
            return _context.Financeiros.FirstOrDefault(f => f.Id == financeiroId);
        }

        public void Cadastrar(Financeiro financeiro)
        {
            _context.Financeiros.Add(financeiro);
            _context.SaveChanges();
        }

        public void Excluir(int financeiroId)
        {
            var financeiro = _context.Financeiros.FirstOrDefault(f => f.Id == financeiroId);
            if (financeiro != null)
            {
                _context.Financeiros.Remove(financeiro);
                _context.SaveChanges();
            }
        }

        public List<Financeiro> ParcelasEmAberto(DateTime inicial, DateTime final)
        {
            inicial = inicial.AddHours(-inicial.Hour);
            inicial = inicial.AddMinutes(-inicial.Minute);
            inicial = inicial.AddMilliseconds(-inicial.Millisecond);
            final = final.AddHours(-final.Hour);
            final = final.AddMinutes(-final.Minute);
            final = final.AddMilliseconds(-final.Millisecond);
            final = final.AddHours(23);

            return _context.Financeiros
                .Where(f => (f.Valor - (f.Desconto == null ? 0 : f.Desconto) + (f.Multa == null ? 0 : f.Multa)) > (f.ValorPago == null ? 0 : f.ValorPago) && f.DataVencimento >= inicial && f.DataVencimento <= final)
                .ToList();
        }
    }
}
