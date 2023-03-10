
using HumanMusicSchoolManager.Data;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.ServicesInterface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HumanMusicSchoolManager.Services
{
    public class SalaService : ISalaService
    {
        private readonly ApplicationDbContext _context;

        public SalaService (ApplicationDbContext context)
        {
            this._context = context;
        }

        public void Alterar(Sala sala)
        {
            _context.Salas.Update(sala);
            _context.SaveChanges();
        }

        public Sala BuscarPorId(int salaId)
        {
            var sala =  _context.Salas             
                .FirstOrDefault(s => s.Id == salaId);

            return sala;
        }

        public List<Sala> BuscarTodos()
        {
            return _context.Salas
                .OrderBy(s => s.Nome).ToList();
        }

        public Sala Cadastrar(Sala sala)
        {
            _context.Salas.Add(sala);
            _context.SaveChanges();
            return sala;
        }

        public void Excluir(int salaId)
        {
            var sala = _context.Salas
                .SingleOrDefault(s => s.Id == salaId);
            if (sala != null)
            {
                _context.Salas.Remove(sala);
                _context.SaveChanges();
            }
        }

        public List<Sala> BuscarPorCurso(List<Curso> cursos)
        {
            return _context.Salas
                .Where(s => s.Cursos.Equals(cursos))
                .OrderBy(s => s.Nome)
                .ToList();
        }

        public List<Sala> BuscarPorNome(string nome)
        {
            return _context.Salas.Where(s => s.Nome.Contains(nome)).OrderBy(S => S.Nome).ToList();
        }

        public void AdicionarCurso(int salaId, int cursoId)
        {
            var sala = _context.Salas.Where(p => p.Id == salaId).Single();
            var curso = _context.Cursos.Where(c => c.Id == cursoId).Single();

            if (sala != null && curso != null)
            {
                sala.IncluirCurso(curso);
            }
            _context.SaveChanges();
        }

        public void RemoverCurso(int salaId, int cursoId)
        {
            var sala = _context.Salas.Include(c => c.Cursos).FirstOrDefault(p => p.Id == salaId);
            var curso = _context.Cursos.Where(c => c.Id == cursoId).FirstOrDefault();

            if (sala != null && curso != null)
            {
                sala.RemoverCurso(curso);
                _context.SaveChanges();
            }
        }
    }
}
