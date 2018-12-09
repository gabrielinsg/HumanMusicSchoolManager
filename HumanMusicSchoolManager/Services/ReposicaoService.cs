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
    public class ReposicaoService : IReposicaoService
    {
        private readonly ApplicationDbContext _context;

        public ReposicaoService(ApplicationDbContext context)
        {
            this._context = context;
        }

        public void Alterar(Reposicao reposicao)
        {
            var entry = _context.Entry<Reposicao>(reposicao);
            if (entry.State == EntityState.Detached)
            {
                var rep = _context.Reposicoes.FirstOrDefault(r => r.Id == reposicao.Id);
                var ent = _context.Entry<Reposicao>(rep);
                ent.State = EntityState.Detached;
                entry.State = EntityState.Modified;
            }
            _context.SaveChanges();
        }

        public Reposicao BuscarPorChamada(int chamadaId)
        {
            return _context.Reposicoes.FirstOrDefault(r => r.ChamadaId == chamadaId);
        }

        public Reposicao BuscarPorId(int reposicaoId)
        {
            return _context.Reposicoes.FirstOrDefault(r => r.Id == reposicaoId);
        }

        public List<Reposicao> BuscarTodos()
        {
            return _context.Reposicoes.ToList();
        }

        public void Cadastrar(Reposicao reposicao)
        {
            _context.Reposicoes.Add(reposicao);
            _context.SaveChanges();
        }

        public void Excluir(int reposicaoId)
        {
            var reposicao = _context.Reposicoes.FirstOrDefault(r => r.Id == reposicaoId);
            if (reposicao != null)
            {
                _context.Reposicoes.Remove(reposicao);
                _context.SaveChanges();
            }
        }
    }
}
