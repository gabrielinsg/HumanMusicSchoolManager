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
                .FirstOrDefault(p => p.Id == id);
        }

        public List<Professor> BuscarTodos()
        {
            return _context.Professores
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
    }
}
