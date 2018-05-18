using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanMusicSchoolManager.Data;
using HumanMusicSchoolManager.Models.Models;

namespace HumanMusicSchoolManager.Services
{
    public class FuncionarioService : IFuncionarioService
    {
        private readonly ApplicationDbContext _context;

        public FuncionarioService(ApplicationDbContext context)
        {
            this._context = context;
        }

        public void Alterar(Funcionario funcionario)
        {
            _context.Funcionarios.Update(funcionario);
            _context.SaveChanges();
        }

        public Funcionario BuscarPorId(int funcionarioId)
        {
            return _context.Funcionarios.Where(f => f.Id == funcionarioId).FirstOrDefault();
        }

        public List<Funcionario> BuscarTodos()
        {
            return _context.Funcionarios.OrderByDescending(f => f.Nome).ToList();
        }

        public void Cadastrar(Funcionario funcionario)
        {
            _context.Funcionarios.Add(funcionario);
            _context.SaveChanges();
        }

        public void Excluir(int funcionarioId)
        {
            var funcionario = _context.Funcionarios.Where(f => f.Id == funcionarioId).FirstOrDefault();

            if (funcionario != null)
            {
                _context.Funcionarios.Remove(funcionario);
                _context.SaveChanges();
            }
        }
    }
}
