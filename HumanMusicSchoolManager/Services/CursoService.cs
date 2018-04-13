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

        public Curso BuscarPorId(int id)
        {
            return _context.Cursos.Where(c => c.Id == id).Single();
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

        public void Excluir(Curso curso)
        {
            _context.Remove(curso);
            _context.SaveChanges();
        }
    }
}
