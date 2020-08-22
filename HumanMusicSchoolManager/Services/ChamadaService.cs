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
    public class ChamadaService : IChamadaService
    {
        private readonly ApplicationDbContext _context;

        public ChamadaService(ApplicationDbContext context)
        {
            this._context = context;
        }

        public void Alterar(Chamada chamada)
        {
            _context.Update(chamada);
            _context.SaveChanges();
        }

        public void AtualizarObservacao(int id, string conteudo)
        {
            var chamada = _context.Chamadas.FirstOrDefault(c => c.Id == id);
            if (chamada != null)
            {
                chamada.Observacao = conteudo;
                _context.Chamadas.Update(chamada);
                _context.SaveChanges();
            }
        }

        public Chamada BuscarPorId(int chamadaId)
        {
            return _context.Chamadas
                .FirstOrDefault(c => c.Id == chamadaId);
        }

        public void Cadastrar(Chamada chamada)
        {
            _context.Add(chamada);
            _context.SaveChanges();
        }

        public void Excluir(int chamadaId)
        {
            var chamada = _context.Chamadas.FirstOrDefault(c => c.Id == chamadaId);
            if (chamada != null)
            {
                _context.Chamadas.Remove(chamada);
                _context.SaveChanges();
            }
        }

        public List<Chamada> HistoricoAlunoCurso(Matricula matricula)
        {
            var chamadas = _context.Chamadas
                .Where(c => c.PacoteCompra.Matricula.AlunoId == matricula.AlunoId 
                    && c.PacoteCompra.Matricula.CursoId == matricula.CursoId
                    && c.Presenca != null)
                .OrderByDescending(c => c.Aula.Data)
                .Take(3)
                .ToList();
            return chamadas;
        }

        public List<Chamada> HistoricoCompleto(int alunoId, int cursoId)
        {
            return _context.Chamadas
                .Where(c => c.PacoteCompra.Matricula.AlunoId == alunoId && c.PacoteCompra.Matricula.CursoId == cursoId && c.Presenca != null)
                .ToList();
        }
    }
}
