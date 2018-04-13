using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanMusicSchoolManager.Data;
using HumanMusicSchoolManager.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace HumanMusicSchoolManager.Services
{
    public class DiarioClasseService : IDiarioClasseService
    {
        private readonly ApplicationDbContext _context;

        public DiarioClasseService(ApplicationDbContext context)
        {
            this._context = context;
        }

        public void Alterar(DiarioClasse diario)
        {
            _context.Update(diario);
            _context.SaveChanges();
        }

        public List<DiarioClasse> BuscarPorAluno(int alunoId)
        {
            return _context.DiariosClasse.Where(d => d.Matricula.AlunoId == alunoId).ToList();
        }

        public DiarioClasse BuscarPorId(int diarioId)
        {
            return _context.DiariosClasse
                .Include(d => d.Matricula)
                .Include(d => d.Matricula.Aluno)
                .Include(d => d.Matricula.Curso)
                .Include(d => d.Matricula.Professor)
                .Where(d => d.Id == diarioId)
                .OrderByDescending(d => d.Data)
                .FirstOrDefault();
                                            
        }

        public List<DiarioClasse> BuscarTodos()
        {
            return _context.DiariosClasse.OrderByDescending(d => d.Data).ToList();
        }

        public void Cadastrar(DiarioClasse diario)
        {
            _context.DiariosClasse.Add(diario);
            _context.SaveChanges();
        }

        public List<DiarioClasse> BuscarAlguns(int matriculaId)
        {
            var hitorico = _context.DiariosClasse
                .Where(d => d.Matricula.Id == matriculaId)
                .OrderByDescending(d => d.Data)
                .Take(3)
                .ToList();
            return hitorico;
        }

        public List<DiarioClasse> BuscarPorMatricula(int matriculaId)
        {
            return _context.DiariosClasse
                .Include(d => d.Matricula)
                .Include(d => d.Matricula.Aluno)
                .Include(d => d.Matricula.Curso)
                .OrderByDescending(d => d.Data)
                .Where(d => d.Matricula.Id == matriculaId).ToList();
        }

        public void Excluir(int diarioId)
        {
            var diario = _context.DiariosClasse.Where(d => d.Id == diarioId).FirstOrDefault();

            if (diario != null)
            {
                _context.Remove(diario);
                _context.SaveChanges();
            }
        }
    }
}
