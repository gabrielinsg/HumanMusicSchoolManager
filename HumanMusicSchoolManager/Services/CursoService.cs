using HumanMusicSchoolManager.Data;
using HumanMusicSchoolManager.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Services
{
    public class CursoService : ICursoService
    {
        private readonly ApplicationDbContext _context;

        public CursoService(ApplicationDbContext context)
        {
            this._context = context;
        }

        public void Alterar(Curso curso)
        {
            _context.Update(curso);
            _context.SaveChanges();
        }

        public Curso BuscarPorId(int cursoId)
        {
            return _context.Cursos.SingleOrDefault(c => c.Id == cursoId);
        }

        public List<Curso> BuscarPorNome(string nome)
        {
            return _context.Cursos.Where(p => p.Nome.Contains(nome)).OrderBy(p => p.Nome).ToList();
        }

        public List<Curso> BuscarTodos()
        {
            var cursos = _context.Cursos.ToList();

            return cursos;
        }

        public void Cadastrar(Curso curso)
        {
            _context.Add(curso);
            _context.SaveChanges();
        }

        public int DucacaoAula(int cursoId)
        {
            return _context.Cursos.FirstOrDefault(c => c.Id == cursoId).DuracaoAula;
        }

        public void Excluir(Curso curso)
        {
            _context.Remove(curso);
            _context.SaveChanges();
        }
    }
}
