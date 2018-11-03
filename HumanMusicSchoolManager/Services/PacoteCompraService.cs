using HumanMusicSchoolManager.Data;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.ServicesInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Services
{
    public class PacoteCompraService : IPacoteCompraService
    {
        private readonly ApplicationDbContext _context;

        public PacoteCompraService(ApplicationDbContext context)
        {
            this._context = context;
        }

        public PacoteCompra Alterar(PacoteCompra pacoteCompra)
        {
            _context.PacoteCompras.Update(pacoteCompra);
            _context.SaveChanges();
            return pacoteCompra;
        }

        public PacoteCompra BuscarPorId(int pacoteCompraId)
        {
            return _context.PacoteCompras.FirstOrDefault(pc => pc.Id == pacoteCompraId);
        }

        public List<PacoteCompra> BuscarTodos()
        {
            return _context.PacoteCompras.ToList();
        }

        public PacoteCompra Cadastrar(PacoteCompra pacoteCompra)
        {
            _context.Add(pacoteCompra);
            _context.SaveChanges();
            return pacoteCompra;
        }

        public void Excluir(int pacoteCompraId)
        {
            var pacoteCompra = _context.PacoteCompras.FirstOrDefault(pc => pc.Id == pacoteCompraId);

            if (pacoteCompra != null)
            {
                _context.PacoteCompras.Remove(pacoteCompra);
                _context.SaveChanges();
            }
        }
    }
}
