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
    public class DispSalaService : IDispSalaService
    {
        private readonly ApplicationDbContext _context;

        public DispSalaService(ApplicationDbContext context)
        {
            this._context = context;
        }

        public List<DispSala> BuscarTodos()
        {
            var dispSalas = _context.DispSalas
                .Include(dp => dp.Professor)
                .ThenInclude(p => p.Cursos)
                .Include(dp => dp.Sala)
                .Include(ds => ds.Matriculas)
                .ThenInclude(m => m.Aluno)
                .Include(ds => ds.Matriculas)
                .ThenInclude(m => m.Curso)
                .ToList();

            return dispSalas;
        }

        public DispSala BuscarPorId(int dispSalaId)
        {
            return _context.DispSalas.SingleOrDefault(ds => ds.Id == dispSalaId);
        }

        public List<DispSala> HorariosDisponiveis()
        {

            var hr = _context.DispSalas
                .Where(ds => ds.Sala.Capacidade > ds.Matriculas.Count || 
                ds.Matriculas.Where(m => m.PacoteCompras.Equals(m.PacoteCompras
                    .Where(p => p.PacoteAula.TipoAula == TipoAula.INDIVIDUAL).ToList())).ToList() == null)
                .Include(dp => dp.Professor)
                .ThenInclude(p => p.Cursos)
                .Include(dp => dp.Sala)
                .Include(ds => ds.Matriculas)
                .ThenInclude(m => m.Aluno)
                .Include(ds => ds.Matriculas)
                .ThenInclude(m => m.Curso)
                .ToList();

            return hr
                .OrderBy(h => h.Professor.Nome)
                .OrderBy(h => h.Hora)
                .OrderBy(h => h.Dia)
                .ToList();
        }
    }
}
