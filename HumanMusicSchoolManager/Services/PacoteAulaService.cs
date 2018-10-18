using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanMusicSchoolManager.Data;
using HumanMusicSchoolManager.Models.Models;

namespace HumanMusicSchoolManager.Services
{
    public class PacoteAulaService : IPacoteAulaService
    {
        private readonly ApplicationDbContext _context;

        public PacoteAulaService(ApplicationDbContext context)
        {
            this._context = context;
        }

        public void Alterar(PacoteAula pacoteAula)
        {
            _context.PacotesAula.Update(pacoteAula);
            _context.SaveChanges();
        }

        public PacoteAula BuscarPorId(int pacoteAulaId)
        {
            return _context.PacotesAula.FirstOrDefault(pa => pa.Id == pacoteAulaId);
        }

        public List<PacoteAula> BuscarPorNome(string nome)
        {
            return _context.PacotesAula.Where(pa => pa.Nome.Contains(nome)).ToList();
        }

        public List<PacoteAula> BuscarTodos()
        {
            return _context.PacotesAula.ToList();
        }

        public void Cadastrar(PacoteAula pacoteAula)
        {
            _context.PacotesAula.Add(pacoteAula);
            _context.SaveChanges();
        }

        public void Excluir(int pacoteAulaId)
        {
            var pacoteAula = _context.PacotesAula.FirstOrDefault(pa => pa.Id == pacoteAulaId);
            if (pacoteAula != null)
            {
                _context.PacotesAula.Remove(pacoteAula);
                _context.SaveChanges();
            }
        }
    }
}
