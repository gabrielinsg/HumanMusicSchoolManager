using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanMusicSchoolManager.Data;
using HumanMusicSchoolManager.Models.Models;

namespace HumanMusicSchoolManager.Services
{
    public class PessoaService : IPessoaService
    {
        private readonly ApplicationDbContext _context;

        public PessoaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Pessoa BuscarPorId(int pessoaId)
        {
            return _context.Pessoas.Where(p => p.Id == pessoaId).SingleOrDefault();
        }

        public List<Pessoa> BuscarTodos()
        {
            return _context.Pessoas.Where(p => p.Ativo == true).ToList();
        }

        public Pessoa Cadastrar(Pessoa pessoa)
        {
            _context.Pessoas.Add(pessoa);
            _context.SaveChanges();
            return _context.Pessoas.Where(p => p.Email == pessoa.Email).SingleOrDefault();
        }
    }
}
