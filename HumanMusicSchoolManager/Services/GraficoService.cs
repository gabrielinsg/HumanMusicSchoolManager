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
    public class GraficoService : IGraficoService
    {
        private readonly ApplicationDbContext _context;

        public GraficoService(ApplicationDbContext context)
        {
            this._context = context;
        }

        public Object EntradasSaidas(DateTime inicial, DateTime final, List<Curso> cursos)
        {
            inicial = DateTimeEntradaSaida.Inicial(inicial);
            final = DateTimeEntradaSaida.Final(final);

            var matriculas = _context.Matriculas
                .Where(m => m.DataMatricula >= inicial && m.DataMatricula <= final)
                .Include(m => m.Curso)
                .ToList();
            var encerradas = _context.Matriculas
                .Where(m => m.EncerramentoMatricula >= inicial && m.EncerramentoMatricula <= final)
                .Include(m => m.Curso)
                .ToList();

            List<Object> graficoCurso = new List<object>();

            foreach (var curso in cursos)
            {
                Dictionary<String, object> cursoGrafico = new Dictionary<string, object>();
                cursoGrafico.Add("Nome", curso.Nome);
                cursoGrafico.Add("Matriculas", matriculas.Where(m => m.CursoId == curso.Id).Count());
                cursoGrafico.Add("Encerramentos", encerradas.Where(m => m.CursoId == curso.Id).Count());
                graficoCurso.Add(cursoGrafico);
            }

            return graficoCurso;
        }
    }
}
