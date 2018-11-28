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
    public class CandidatoService : ICandidatoService
    {
        private readonly ApplicationDbContext _context;

        public CandidatoService(ApplicationDbContext context)
        {
            this._context = context;
        }

        public void Alterar(Candidato candidato)
        {
            _context.Candidatos.Update(candidato);
            _context.SaveChanges();
        }

        public Candidato BuscarPorId(int candidatoId)
        {
            return _context.Candidatos
                .Include(c => c.Demostrativas)
                .ThenInclude(d => d.Curso)
                .Include(c => c.Demostrativas)
                .ThenInclude(d => d.DispSala)
                .ThenInclude(ds => ds.Sala)
                .Include(c => c.Demostrativas)
                .ThenInclude(d => d.DispSala)
                .ThenInclude(ds => ds.Professor)
                .Include(c => c.Demostrativas)
                .ThenInclude(d => d.Aula)
                .FirstOrDefault(c => c.Id == candidatoId);
        }

        public List<Candidato> BuscarPorNome(string nome)
        {
            return _context.Candidatos.Where(c => c.Nome.Contains(nome)).ToList();
        }

        public List<Candidato> BuscarTodos()
        {
            return _context.Candidatos.ToList();
        }

        public void Cadastrar(Candidato candidato)
        {
            _context.Candidatos.Add(candidato);
            _context.SaveChanges();
        }

        public void Excluir(int candidatoId)
        {
            var candidato = _context.Candidatos.FirstOrDefault(c => c.Id == candidatoId);
            if (candidato != null)
            {
                _context.Candidatos.Remove(candidato);
                _context.SaveChanges();
            }
        }
    }
}
