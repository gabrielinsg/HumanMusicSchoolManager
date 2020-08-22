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
    public class ValeService : IValeService
    {
        private readonly ApplicationDbContext _context;

        public ValeService(ApplicationDbContext context)
        {
            this._context = context;
        }

        public Vale BuscarPorId(int valeId)
        {
            return _context.Vales
                .FirstOrDefault(v => v.Id == valeId);
        }

        public List<Vale> BuscarPorPessoa(int pessoaId, DateTime inicial, DateTime final)
        {
            inicial = DateTimeEntradaSaida.Inicial(inicial);
            final = DateTimeEntradaSaida.Final(final);

            return _context.Vales
                .Where(v => v.PessoaId == pessoaId && v.Data >= inicial && v.Data <= final)
                .ToList();
        }

        public List<Vale> BuscarTodos(DateTime inicial, DateTime final)
        {
            inicial = DateTimeEntradaSaida.Inicial(inicial);
            final = DateTimeEntradaSaida.Final(final);

            return _context.Vales
                .Where(v => v.Data >= inicial && v.Data <= final)
                .ToList();
        }

        public void Lancar(Vale vale)
        {
            _context.Vales.Add(vale);
            _context.SaveChanges();
        }

        public void Excluir(int valeId)
        {
            var vale = _context.Vales.Find(valeId);
            if (vale != null)
            {
                _context.Vales.Remove(vale);
                _context.SaveChanges();
            }
        }
    }
}
