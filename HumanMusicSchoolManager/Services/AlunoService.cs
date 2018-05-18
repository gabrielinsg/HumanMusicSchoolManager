using HumanMusicSchoolManager.Data;
using HumanMusicSchoolManager.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Services
{
    public class AlunoService : IAlunoService
    {
        private readonly ApplicationDbContext _context;

        public AlunoService(ApplicationDbContext context)
        {
            this._context = context;
        }

        public void Alterar(Aluno aluno)
        {
            _context.Alunos.Update(aluno);
            _context.SaveChanges();
        }

        public Aluno BuscarPorId(int alunoId)
        {
            return _context.Alunos.Include(a => a.Matriculas).FirstOrDefault(a => a.Id == alunoId);
        }

        public List<Aluno> BuscarTodos()
        {
            return _context.Alunos.Include(a => a.Matriculas).OrderBy(a => a.Nome).ToList();
        }

        public void Cadastrar(Aluno aluno)
        {
            _context.Alunos.Add(aluno);
            _context.SaveChanges();
        }

        public void Excluir(int alunoId)
        {
            var aluno = _context.Alunos.Where(a => a.Id == alunoId).SingleOrDefault();
            if (aluno != null)
            {
                _context.Alunos.Remove(aluno);
                _context.SaveChanges();
            }
        }

        public bool VerificarRm(int rm)
        {
            if(_context.Alunos.Where(a => a.RM == rm).FirstOrDefault() == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
