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
    public class DemostrativaService : IDemostrativaService
    {
        private readonly ApplicationDbContext _context;

        public DemostrativaService(ApplicationDbContext context)
        {
            this._context = context;
        }

        public void Alterar(Demostrativa demostrativa)
        {
            var entry = _context.Entry<Demostrativa>(demostrativa);
            if (entry.State == EntityState.Detached)
            {
                var fin = _context.Demostrativas.FirstOrDefault(f => f.Id == demostrativa.Id);
                var ent = _context.Entry<Demostrativa>(fin);
                ent.State = EntityState.Detached;
                entry.State = EntityState.Modified;
            }
            _context.SaveChanges();
        }

        public Demostrativa BuscarPorId(int demostrativaId)
        {
           return _context.Demostrativas
                .Include(d => d.Aula)
                .Include(d => d.Pessoa)
                .FirstOrDefault(d => d.Id == demostrativaId);
        }

        public List<Demostrativa> BuscarTodos()
        {
            return _context.Demostrativas.ToList();
        }

        public void Cadastrar(Demostrativa demostrativa)
        {
            _context.Demostrativas.Add(demostrativa);
            _context.SaveChanges();
        }

        public List<Demostrativa> DemostrativasAbertas()
        {
            return _context.Demostrativas
                .Where(d => d.DispSalaId != null)
                .Include(d => d.Curso)
                .Include(d => d.Candidato)
                .Include(d => d.DispSala)
                .ThenInclude(ds => ds.Professor)
                .Include(d => d.Aula)
                .OrderBy(d => d.Aula.Data)
                .ToList();
        }

        public void Excluir(int demostrativaId)
        {
            var demostrativa = _context.Demostrativas.FirstOrDefault(d => d.Id == demostrativaId);
            if (demostrativa != null)
            {
                _context.Demostrativas.Remove(demostrativa);
                _context.SaveChanges();
            }
        }
    }
}
