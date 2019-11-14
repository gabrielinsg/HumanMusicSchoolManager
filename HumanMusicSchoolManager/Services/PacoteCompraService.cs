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
            return _context.PacoteCompras
                .Include(pc => pc.PacoteAula)
                .ThenInclude(pa => pa.Contrato)
                .Include(pc => pc.Matricula)
                .ThenInclude(m => m.Aluno)
                .ThenInclude(a => a.Endereco)
                .Include(pc => pc.Chamadas)
                .ThenInclude(c => c.Aula)
                .Include(pc => pc.Matricula)
                .ThenInclude(m => m.Curso)
                .Include(pc => pc.Matricula)
                .ThenInclude(m => m.DispSala)
                .ThenInclude(ds => ds.Professor)
                .Include(pc => pc.Matricula)
                .ThenInclude(m => m.DispSala)
                .ThenInclude(ds => ds.Sala)
                .Include(ds => ds.Matricula)
                .ThenInclude(m => m.RespFinanceiro)
                .ThenInclude(rf => rf.Endereco)
                .Include(pc => pc.Trancamento)
                .Include(pc => pc.Financeiros)
                .Include(pc => pc.Matricula)
                .ThenInclude(m => m.Professor)
                .FirstOrDefault(pc => pc.Id == pacoteCompraId);
        }

        public List<PacoteCompra> BuscarTodos()
        {
            return _context.PacoteCompras
            .Include(pc => pc.Financeiros)
            .Include(pc => pc.Matricula)
            .ThenInclude(m => m.PacoteCompras)
            .ToList();
        }

        public PacoteCompra Cadastrar(PacoteCompra pacoteCompra)
        {
            _context.Add(pacoteCompra);
            _context.SaveChanges();
            return pacoteCompra;
        }

        public PacoteCompra CalendarioAluno(int pacoteCompraId, DateTime inicial, DateTime final)
        {
            inicial = inicial.AddHours(-inicial.Hour);
            inicial = inicial.AddMinutes(-inicial.Minute);
            inicial = inicial.AddMilliseconds(-inicial.Millisecond);
            final = final.AddHours(-final.Hour);
            final = final.AddMinutes(-final.Minute);
            final = final.AddMilliseconds(-final.Millisecond);
            final = final.AddHours(23);

            var pacoteCompra = _context.PacoteCompras
                    .Include(pc => pc.PacoteAula)
                    .ThenInclude(pa => pa.Contrato)
                    .Include(pc => pc.Matricula)
                    .ThenInclude(m => m.Aluno)
                    .ThenInclude(a => a.Endereco)
                    .Include(pc => pc.Matricula)
                    .ThenInclude(m => m.Curso)
                    .Include(pc => pc.Matricula)
                    .ThenInclude(m => m.DispSala)
                    .ThenInclude(ds => ds.Professor)
                    .Include(pc => pc.Matricula)
                    .ThenInclude(m => m.DispSala)
                    .ThenInclude(ds => ds.Sala)
                    .Include(ds => ds.Matricula)
                    .ThenInclude(m => m.RespFinanceiro)
                    .ThenInclude(rf => rf.Endereco)
                    .Include(pc => pc.Trancamento)
                    .Include(pc => pc.Financeiros)
                    .FirstOrDefault(pc => pc.Id == pacoteCompraId);
            var chamadas = _context.Chamadas
                .Where(ch => ch.PacoteCompraId == pacoteCompraId && (ch.Aula.Data >= inicial && ch.Aula.Data <= final))
                .Include(ch => ch.Aula)
                .ToList();

            pacoteCompra.Chamadas = chamadas;
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

        public List<PacoteCompra> FaltasSeguidas()
        {
            var pacoteCompraFull = _context.PacoteCompras
                .Where(pc => pc.Matricula.DispSalaId != null && pc.Chamadas.Any(c => c.Presenca == null))
                .Include(pc => pc.Chamadas)
                .ThenInclude(c => c.Aula)
                .Include(pc => pc.Matricula)
                .ThenInclude(m => m.Aluno)
                .Include(pc => pc.Matricula)
                .ThenInclude(m => m.Curso)
                .ToList();

            var pacotesCompra = new List<PacoteCompra>();

            if (pacoteCompraFull.Count > 0)
            {
                foreach (var pacoteCompra in pacoteCompraFull)
                {
                    pacoteCompra.Chamadas = pacoteCompra.Chamadas.OrderByDescending(c => c.Aula.Data).Where(c => c.Presenca != null).ToList();
                    if (pacoteCompra.Chamadas.Count > 1)
                    {
                        if (pacoteCompra.Chamadas[0].Presenca == false && pacoteCompra.Chamadas[1].Presenca == false)
                        {
                            pacotesCompra.Add(pacoteCompra);
                        }
                    }
                }
            }

            return pacotesCompra.OrderBy(p => p.Matricula.Aluno.Nome).ToList();
        }

        public List<PacoteCompra> UtimaAulaPorPeriodo(DateTime inicial, DateTime final)
        {
            inicial = inicial.AddHours(-inicial.Hour);
            inicial = inicial.AddMinutes(-inicial.Minute);
            inicial = inicial.AddMilliseconds(-inicial.Millisecond);
            final = final.AddHours(-final.Hour);
            final = final.AddMinutes(-final.Minute);
            final = final.AddMilliseconds(-final.Millisecond);
            final = final.AddHours(23);

            return _context.PacoteCompras.Where(pc => pc.Chamadas.OrderByDescending(c => c.Aula.Data).FirstOrDefault().Aula.Data >= inicial && pc.Chamadas.OrderByDescending(c => c.Aula.Data).FirstOrDefault().Aula.Data <= final)
                .Include(pc => pc.Chamadas)
                .ThenInclude(c => c.Aula)
                .Include(pc => pc.Matricula)
                .ThenInclude(m => m.Aluno)
                .Include(pc => pc.Matricula)
                .ThenInclude(m => m.Curso)
                .ToList();
        }
    }
}
