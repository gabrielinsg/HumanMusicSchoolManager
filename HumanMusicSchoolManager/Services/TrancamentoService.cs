using HumanMusicSchoolManager.Data;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.ServicesInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Services
{
    public class TrancamentoService : ITrancamentoService
    {
        private readonly ApplicationDbContext _context;

        public TrancamentoService(ApplicationDbContext context)
        {
            this._context = context;
        }

        public void Alterar(Trancamento trancamento)
        {
            _context.Trancamentos.Update(trancamento);
            _context.SaveChanges();
        }

        public List<Trancamento> BuscarEntreDatas(DateTime dataInicial, DateTime dataFinal)
        {
            return _context.Trancamentos.Where(t => t.DataInicial >= dataInicial && t.DataFinal <= dataFinal).ToList();
        }

        public Trancamento BuscarPorId(int trancamentoId)
        {
            return _context.Trancamentos.FirstOrDefault(t => t.Id == trancamentoId);
        }

        public List<Trancamento> BuscarTodos()
        {
            return _context.Trancamentos.ToList();
        }

        public void Cadastrar(Trancamento trancamento)
        {
            _context.Trancamentos.Add(trancamento);
            _context.SaveChanges();
        }

        public void Excluir(int trancamentoId)
        {
            var trancamento = _context.Trancamentos.FirstOrDefault(t => t.Id == trancamentoId);

            if (trancamento != null)
            {
                _context.Trancamentos.Remove(trancamento);
                _context.SaveChanges();
            }
        }
    }
}
