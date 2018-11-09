using HumanMusicSchoolManager.Data;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.ServicesInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Services
{
    public class FeriadoService : IFeriadoService
    {
        private readonly ApplicationDbContext _context;

        public FeriadoService(ApplicationDbContext context)
        {
            this._context = context;
        }

        public void Alterar(Feriado feriado)
        {
            _context.Update(feriado);
            _context.SaveChanges();
        }

        public List<Feriado> BuscarEntreDatas(DateTime dataInicial, DateTime dataFinal)
        {
            return _context.Feriados.Where(f => f.DataInicial >= dataFinal && f.DataFinal <= dataFinal).ToList();
        }

        public Feriado BuscarPorData(DateTime data)
        {
            return _context.Feriados.FirstOrDefault(f => data >= f.DataInicial && data <= f.DataFinal);
        }

        public Feriado BuscarPorId(int feriadoId)
        {
            return _context.Feriados.FirstOrDefault(f => f.Id == feriadoId);
        }

        public List<Feriado> BuscarTodos()
        {
            return _context.Feriados.ToList();
        }

        public void Cadastrar(Feriado feriado)
        {
            _context.Feriados.Add(feriado);
            _context.SaveChanges();
        }

        public void Excluir(int feriadoId)
        {
            var feriado = _context.Feriados.FirstOrDefault(f => f.Id == feriadoId);
            if (feriado != null)
            {
                _context.Feriados.Remove(feriado);
                _context.SaveChanges();
            }
        }
    }
}
