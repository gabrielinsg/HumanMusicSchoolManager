using HumanMusicSchoolManager.Data;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.Models.ViewModels;
using HumanMusicSchoolManager.ServicesInterface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Services
{
    public class ProfessorService : IProfessorService
    {
        private readonly ApplicationDbContext _context;

        public ProfessorService(ApplicationDbContext context)
        {
            this._context = context;
        }

        public void Alterar(Professor professor)
        {
            _context.Professores.Update(professor);
            _context.SaveChanges();
        }

        public Professor BuscarPorId(int id)
        {
            return _context.Professores
                .Include(c => c.Cursos)
                .ThenInclude(c => c.Curso)
                .Include(c => c.Endereco)
                .Include(p => p.Aulas)
                .ThenInclude(a => a.Chamadas)
                .ThenInclude(c => c.PacoteCompra)
                .ThenInclude(pc => pc.Matricula)
                .ThenInclude(m => m.Aluno)
                .Include(p => p.Aulas)
                .ThenInclude(a => a.Demostrativas)
                .ThenInclude(d => d.Candidato)
                .Include(p => p.DispSalas)
                .ThenInclude(ds => ds.Matriculas)
                .FirstOrDefault(p => p.Id == id);
        }

        public List<Professor> BuscarTodos()
        {
            return _context.Professores
                .Include(p => p.Endereco)
                .Include(c => c.Cursos)
                .ThenInclude(c => c.Curso)
                .OrderBy(p => p.Nome).ToList();
        }

        public void Cadastrar(Professor professor)
        {
            _context.Professores.Add(professor);
            _context.SaveChanges();
        }

        public void Excluir(Professor professor)
        {
            if (_context.Enderecos.FirstOrDefault(e => e.Id == professor.Endereco.Id) != null)
            {
                _context.Enderecos.Remove(_context.Enderecos.FirstOrDefault(e => e.Id == professor.Endereco.Id));

            }
            _context.Professores.Remove(professor);
            _context.SaveChanges();
        }

        public void AdicionarCurso(int professorId, int cursoId)
        {
            var professor = _context.Professores.Where(p => p.Id == professorId).Single();
            var curso = _context.Cursos.Where(c => c.Id == cursoId).Single();

            if (professor != null && curso != null)
            {
                professor.IncluiCurso(curso);
            }
            _context.SaveChanges();
        }

        public void RemoverCurso(int professorId, int cursoId)
        {
            var professor = _context.Professores.Include(c => c.Cursos).FirstOrDefault(p => p.Id == professorId);
            var curso = _context.Cursos.Where(c => c.Id == cursoId).FirstOrDefault();

            if (professor != null && curso != null)
            {
                professor.RemoveCurso(curso);
            }
            _context.SaveChanges();
        }

        public List<Professor> BuscarProfessorPorCurso(int cursoId)
        {
            var professores = _context.Professores.Where(p => p.Cursos.Any(c => c.CursoId == cursoId)).OrderBy(p => p.Nome).ToList();

            return professores;
        }

        public Professor BuscarPorCPF(string cpf)
        {
            return _context.Professores.SingleOrDefault(p => p.CPF == cpf);
        }

        public List<Professor> BuscarPorNome(string nome)
        {
            return _context.Professores.Where(p => p.Nome.Contains(nome)).OrderBy(P => P.Nome).ToList();
        }

        public Professor BuscarPorIdData(int professorId, DateTime inicial, DateTime final)
        {
            inicial = inicial.AddHours(-inicial.Hour);
            inicial = inicial.AddMinutes(-inicial.Minute);
            inicial = inicial.AddMilliseconds(-inicial.Millisecond);
            final = final.AddHours(-final.Hour);
            final = final.AddMinutes(-final.Minute);
            final = final.AddMilliseconds(-final.Millisecond);
            final = final.AddHours(23);
            var professor = _context.Professores
                .Include(p => p.Aulas)
                .ThenInclude(a => a.Chamadas)
                .ThenInclude(c => c.Reposicao)
                .FirstOrDefault(p => p.Id == professorId);
            var listAulas = new List<Aula>();
            if (professor.Aulas != null)
            {
                foreach (var aula in professor.Aulas)
                {
                    if (aula.Data >= inicial && aula.Data <= final && aula.AulaDada == true)
                    {
                        listAulas.Add(aula);
                    }
                }
            }
            professor.Aulas = listAulas;
            return professor;
        }

        public List<Professor> BuscarTodosData(DateTime inicial, DateTime final)
        {
            inicial = inicial.AddHours(-inicial.Hour);
            inicial = inicial.AddMinutes(-inicial.Minute);
            inicial = inicial.AddMilliseconds(-inicial.Millisecond);
            final = final.AddHours(-final.Hour);
            final = final.AddMinutes(-final.Minute);
            final = final.AddMilliseconds(-final.Millisecond);
            final = final.AddHours(23);
            var professores = _context.Professores
                .Include(p => p.Aulas)
                .ThenInclude(a => a.Chamadas)
                .ThenInclude(c => c.Reposicao)
                .ToList();
            if (professores != null)
            {
                foreach (var professor in professores)
                {
                    var listAulas = new List<Aula>();
                    if (professor.Aulas != null)
                    {
                        foreach (var aula in professor.Aulas)
                        {
                            if (aula.Data >= inicial && aula.Data <= final && aula.AulaDada == true)
                            {
                                listAulas.Add(aula);
                            }
                        }
                    }
                    professores[professores.IndexOf(professor)].Aulas = listAulas;
                }
                
            }
            
            return professores;
        }

        public ProfessorCompletoViewModel RelatorioCompleto(int professorId, DateTime inicial, DateTime final)
        {
            inicial = inicial.AddHours(-inicial.Hour);
            inicial = inicial.AddMinutes(-inicial.Minute);
            inicial = inicial.AddMilliseconds(-inicial.Millisecond);
            final = final.AddHours(-final.Hour);
            final = final.AddMinutes(-final.Minute);
            final = final.AddMilliseconds(-final.Millisecond);
            final = final.AddHours(23);
            var relatorio = new ProfessorCompletoViewModel
            {
                Professor = _context.Professores.FirstOrDefault(p => p.Id == professorId),
                Aulas = _context.Aulas
                    .Where(a => a.ProfessorId == professorId && (a.Data >= inicial && a.Data <= final))
                    .Include(a => a.Chamadas)
                    .ThenInclude(c => c.Reposicao)
                    .Include(a => a.Demostrativas)
                    .Include(a => a.Sala)
                    .ToList(),
                DispSalas = _context.DispSalas
                    .Where(ds => ds.Professor.Id == professorId)
                    .Include(ds => ds.Matriculas)
                    .Include(ds => ds.Sala)
                    .ToList(),
                Matriculas = _context.Matriculas
                .Where(m => m.DispSala.Professor.Id == professorId && m.DispSalaId != null)
                .Include(m => m.DispSala)
                .ThenInclude(ds => ds.Professor)
                .ToList()
            };
            return relatorio;
        }

        public Professor CalendarioProfessor(int professorId, DateTime inicial, DateTime final)
        {
            inicial = inicial.AddHours(-inicial.Hour);
            inicial = inicial.AddMinutes(-inicial.Minute);
            inicial = inicial.AddMilliseconds(-inicial.Millisecond);
            final = final.AddHours(-final.Hour);
            final = final.AddMinutes(-final.Minute);
            final = final.AddMilliseconds(-final.Millisecond);
            final = final.AddHours(23);

            var professor = _context.Professores
                .Include(c => c.Cursos)
                .ThenInclude(c => c.Curso)
                .Include(c => c.Endereco)
                .Include(p => p.Aulas)
                .ThenInclude(a => a.Demostrativas)
                .ThenInclude(d => d.Candidato)
                .FirstOrDefault(p => p.Id == professorId);
            var aulas = _context.Aulas
                .Where(a => a.ProfessorId == professorId && (a.Data >= inicial && a.Data <= final))
                .Include(a => a.Chamadas)
                .ThenInclude(c => c.PacoteCompra)
                .ThenInclude(pc => pc.Matricula)
                .ThenInclude(m => m.Aluno)
                .ToList();

            professor.Aulas = aulas;

            return professor;
        }
    }
}
