using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanMusicSchoolManager.Data;
using HumanMusicSchoolManager.Models.Models;
using Microsoft.EntityFrameworkCore;

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
            return _context.Salas.Include(s => s.Cursos).FirstOrDefault(s => s.Id == salaId);
        }

        public List<Sala> BuscarTodos()
        {
            return _context.Salas.Include(s => s.Cursos).OrderBy(s => s.Nome).ToList();
        }

        public Sala Cadastrar(Sala sala)
        {
            _context.Salas.Add(sala);
            _context.SaveChanges();
            return sala;
        }

        public void Excluir(int salaId)
        {
            var sala = _context.Salas.SingleOrDefault(s => s.Id == salaId);
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
    }
}
