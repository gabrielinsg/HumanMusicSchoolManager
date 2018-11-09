using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanMusicSchoolManager.Data;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.ServicesInterface;
using Microsoft.EntityFrameworkCore;

namespace HumanMusicSchoolManager.Services
{
    public class MatriculaService : IMatriculaService
    {
        private readonly ApplicationDbContext _context;

        public MatriculaService(ApplicationDbContext context)
        {
            this._context = context;
        }

        public void Alterar(Matricula matricula)
        {
            _context.Matriculas.Update(matricula);
            _context.SaveChanges();
        }

        public Matricula BuscarPorId(int matriculaId)
        {
            return _context.Matriculas
                .Include(m => m.Aluno)
                .Include(m => m.Curso)
                .Include(m => m.DispSala)
                .ThenInclude(ds => ds.Professor)
                .Include(m => m.DispSala)
                .ThenInclude(ds => ds.Sala)
                .Where(m => m.Id == matriculaId)
                .FirstOrDefault();
        }

        public List<Matricula> BuscarPorProfessor(int professorId)
        {
            return _context.Matriculas
                .Include(m => m.Aluno)
                .Include(m => m.Curso).ToList();
        }

        public List<Matricula> BuscarTodos()
        {
            return _context.Matriculas.ToList();
        }

        public void Cadastrar(Matricula matricula)
        {
            _context.Matriculas.Add(matricula);
            _context.SaveChanges();
        }

        public void Excluir(int matriculaId)
        {
            var matricula = _context.Matriculas.Where(m => m.Id == matriculaId).SingleOrDefault();

            if (matricula != null)
            {
                _context.Matriculas.Remove(matricula);
                _context.SaveChanges();
            }
        }
    }
}
