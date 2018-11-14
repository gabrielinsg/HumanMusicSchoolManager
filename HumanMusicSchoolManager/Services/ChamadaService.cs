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
            return _context.Chamadas
                .Include(c => c.Aula)
                .ThenInclude(a => a.Curso)
                .Include(c => c.Aula)
                .ThenInclude(a => a.Professor)
                .Include(c => c.Aula)
                .ThenInclude(a => a.Sala)
                .Include(c => c.PacoteCompra)
                .ThenInclude(pc => pc.Matricula)
                .ThenInclude(m => m.Aluno)
                .FirstOrDefault(c => c.Id == chamadaId);
        }

        public void Cadastrar(Chamada chamada)
        {
            _context.Add(chamada);
            _context.SaveChanges();
        }
    }
}
