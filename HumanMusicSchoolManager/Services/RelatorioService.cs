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
                .Where(p => p.DispSalas.Count > 0 && p.Ativo)
                .ToList();
        }

        public List<Aluno> Alunos()
        {
            return _context.Alunos
                .Where(a => a.Matriculas.Any(m => m.DispSalaId != null))
                .Include(a => a.Matriculas)
                .ToList();
        }

        public List<PacoteCompra> PacotesAtivos()
        {
            return _context.PacoteCompras
                .Where(pc => pc.Chamadas.Any(c => c.Presenca == null))
                .Include(pc => pc.PacoteAula)
                .ToList();
        }

        public List<PacoteCompra> PacotesContratados(DateTime inicial, DateTime final)
        {
            inicial = inicial.AddHours(-inicial.Hour);
            inicial = inicial.AddMinutes(-inicial.Minute);
            inicial = inicial.AddMilliseconds(-inicial.Millisecond);
            final = final.AddHours(-final.Hour);
            final = final.AddMinutes(-final.Minute);
            final = final.AddMilliseconds(-final.Millisecond);
            final = final.AddHours(23);

            var pcs = _context.PacoteCompras
                .Where(pc => pc.DataCompra.Date >= inicial.Date && pc.DataCompra.Date <= final.Date)
                .Include(pc => pc.PacoteAula)
                .ToList();

            return pcs;
        }

        public List<Matricula> MatriculasCanceladas(DateTime inicial, DateTime final)
        {
            inicial = DateTimeEntradaSaida.Inicial(inicial);
            final = DateTimeEntradaSaida.Final(final);
            return _context.Matriculas
                .Where(m => m.EncerramentoMatricula >= inicial && m.EncerramentoMatricula <= final)
                .ToList();
        }

        public List<Matricula> MatriculasNovas(DateTime inicial, DateTime final)
        {
            inicial = DateTimeEntradaSaida.Inicial(inicial);
            final = DateTimeEntradaSaida.Final(final);
            return _context.Matriculas
                .Where(m => m.DataMatricula >= inicial && m.DataMatricula <= final)
                .ToList();
        }

        public List<Aluno> AlunosAniversariantes(int mes)
        {
            return _context.Alunos
                .Where(a => 
                    a.Matriculas.Any(m => m.EncerramentoMatricula == null) 
                    && (a.DataNascimento.Month == mes)
                )
                .ToList();
        }

        public List<Demostrativa> Demostrativas(DateTime inicial, DateTime final)
        {
            inicial = DateTimeEntradaSaida.Inicial(inicial);
            final = DateTimeEntradaSaida.Final(final);

            return _context.Demostrativas.Where(d => d.Data >= inicial && d.Data <= final)
                .ToList();
        }
    }
}
