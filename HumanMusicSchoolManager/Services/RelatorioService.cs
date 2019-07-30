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
    public class RelatorioService : IRelatorioService
    {
        private readonly ApplicationDbContext _context;

        public RelatorioService(ApplicationDbContext context)
        {
            this._context = context;
        }

        public List<Professor> Professores()
        {
            return _context.Professores
                .Where(p => p.DispSalas.Count > 0)
                .Include(p => p.DispSalas)
                .ThenInclude(ds => ds.Matriculas)
                .ThenInclude(m => m.PacoteCompras)
                .ThenInclude(pc => pc.Chamadas)
                .Include(p => p.DispSalas)
                .ThenInclude(ds => ds.Sala)
                .Include(p => p.DispSalas)
                .ThenInclude(ds => ds.Matriculas)
                .ThenInclude(m => m.PacoteCompras)
                .ThenInclude(pc => pc.PacoteAula)
                .ToList();
        }

        public List<Aluno> Alunos()
        {
            return _context.Alunos
                .Where(a => a.Matriculas.Any(m => m.DispSalaId != null))
                .Include(a => a.Matriculas)
                .ToList();
        }
    }
}
