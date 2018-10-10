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

        public Aluno BuscarPorCPF(string CPF)
        {
            return _context.Alunos.SingleOrDefault(a => a.CPF == CPF);
        }

        public Aluno BuscarPorId(int alunoId)
        {
            return _context.Alunos.Include(a => a.Matriculas)
                .Include(a => a.Endereco)
                .FirstOrDefault(a => a.Id == alunoId);
        }

        public List<Aluno> BuscarPorNome(string nome)
        {
            return _context.Alunos.Where(a => a.Nome.Contains(nome)).ToList();
        }

        public List<Aluno> BuscarTodos()
        {
            return _context.Alunos.Include(a => a.Matriculas).OrderBy(a => a.Nome).ToList();
        }

        public Aluno Cadastrar(Aluno aluno)
        {
            _context.Alunos.Add(aluno);
            _context.SaveChanges();
            return aluno;
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
