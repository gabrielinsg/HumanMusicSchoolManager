using HumanMusicSchoolManager.Data;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.ServicesInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Services
{
    public class ChamadaService : IChamadaService
    {
        private readonly ApplicationDbContext _context;

        public ChamadaService(ApplicationDbContext context)
        {
            this._context = context;
        }

        public void Alterar(Chamada chamada)
        {
            _context.Update(chamada);
            _context.SaveChanges();
        }

        public Chamada BuscarPorId(int chamadaId)
        {
            return _context.Chamadas.FirstOrDefault(c => c.Id == chamadaId);
        }

        public void Cadastrar(Chamada chamada)
        {
            _context.Add(chamada);
            _context.SaveChanges();
        }
    }
}
