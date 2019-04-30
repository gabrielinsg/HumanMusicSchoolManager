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
    public class AulaService : IAulaService
    {
        private readonly ApplicationDbContext _context;

        public AulaService(ApplicationDbContext context)
        {
            this._context = context;
        }

        public void Alterar(Aula aula)
        {

            var entry = _context.Entry<Aula>(aula);
            if (entry.State == EntityState.Detached)
            {
                var aul = _context.Aulas.FirstOrDefault(a => a.Id == aula.Id);
                var ent = _context.Entry<Aula>(aul);
                ent.State = EntityState.Detached;
                entry.State = EntityState.Modified;
            }
            _context.SaveChanges();
        }

        public List<Aula> Atrasadas()
        {
            return _context.Aulas
                .Where(a => a.AulaDada == false && a.Data < DateTime.Now)
                .Include(a => a.Professor)
                .Include(a => a.Chamadas)
                .ThenInclude(c => c.PacoteCompra)
                .ThenInclude(pc => pc.Matricula)
                .ThenInclude(m => m.Aluno)
                .Include(a => a.Curso)
                .Include(a => a.Demostrativas)
                .ThenInclude(d => d.Candidato)
                .ToList();
        }

        public List<Aula> AtrasadasPorProfessor(int professorId)
        {
            return _context.Aulas
                .Where(a => a.AulaDada == false && a.Data < DateTime.Now && a.ProfessorId == professorId)
                .Include(a => a.Professor)
                .Include(a => a.Chamadas)
                .ThenInclude(c => c.PacoteCompra)
                .ThenInclude(pc => pc.Matricula)
                .ThenInclude(m => m.Aluno)
                .Include(a => a.Curso)
                .Include(a => a.Demostrativas)
                .ThenInclude(d => d.Candidato)
                .ToList();
        }

        public void AtualizarDescAtividades(int id, string descAtividades)
        {
            var aula = _context.Aulas.FirstOrDefault(a => a.Id == id);
            if (aula != null)
            {
                aula.DescAtividades = descAtividades;
                _context.Update(aula);
                _context.SaveChanges();
            }
        }

        public void AlterarTempoLimite(int TempoLancamento)
        {
            var aulas = _context.Aulas
                .Where(a => a.AulaDada == false)
                .ToList();
            foreach (var aula in aulas)
            {
                aula.DataLimite = aula.Data;
                aula.DataLimite = aula.DataLimite.AddHours(TempoLancamento);
            }
            _context.SaveChanges();
        }

        public Aula BuscarPorDiaHora(DateTime data, DispSala dispSala)
        {
            dispSala = _context.DispSalas
                .Include(ds => ds.Professor)
                .Include(ds => ds.Sala)
                .FirstOrDefault(ds => ds.Id == dispSala.Id);
            return _context.Aulas.FirstOrDefault(a => a.Data == data && a.ProfessorId == dispSala.Professor.Id && a.SalaId == dispSala.Sala.Id);
        }

        public Aula BuscarPorId(int aulaId)
        {
            return _context.Aulas
                .Include(a => a.Professor)
                .Include(a => a.Chamadas)
                .ThenInclude(c => c.PacoteCompra)
                .ThenInclude(pc => pc.Matricula)
                .ThenInclude(m => m.Aluno)
                .Include(a => a.Curso)
                .Include(a => a.Sala)
                .Include(a => a.Chamadas)
                .ThenInclude(c => c.PacoteCompra)
                .ThenInclude(pc => pc.Chamadas)
                .ThenInclude(c => c.Aula)
                .Include(a => a.Chamadas)
                .ThenInclude(c => c.PacoteCompra)
                .ThenInclude(pc => pc.Chamadas)
                .ThenInclude(c => c.Reposicao)
                .Include(a => a.Demostrativas)
                .ThenInclude(d => d.Candidato)
                .Include(a => a.Demostrativas)
                .ThenInclude(d => d.Curso)
                .FirstOrDefault(a => a.Id == aulaId);
        }

        public List<Aula> BuscarPorSala(int salaId)
        {
            return _context.Aulas.Where(a => a.Sala.Id == salaId)
                .Include(a => a.Chamadas)
                .ThenInclude(c => c.PacoteCompra)
                .ThenInclude(pc => pc.Matricula)
                .ThenInclude(m => m.Aluno)
                .ToList();
        }

        public List<Aula> BuscarTodas()
        {
            return _context.Aulas.ToList();
        }

        public void Cadastrar(Aula aula)
        {
            _context.Aulas.Add(aula);
            _context.SaveChanges();
        }

        public void Excluir(int aulaId)
        {
            var aula = _context.Aulas.FirstOrDefault(a => a.Id == aulaId);
            if (aula != null)
            {
                _context.Aulas.Remove(aula);
                _context.SaveChanges();
            }
        }
    }
}
