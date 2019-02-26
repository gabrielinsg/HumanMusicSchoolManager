using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanMusicSchoolManager.Data;
using HumanMusicSchoolManager.Models;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.ServicesInterface;
using Microsoft.EntityFrameworkCore;

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
            return _context.Pessoas.Include(p => p.Endereco).Where(p => p.Ativo == true).ToList();
        }

        public Pessoa Cadastrar(Pessoa pessoa)
        {
            _context.Pessoas.Add(pessoa);
            _context.SaveChanges();
            return _context.Pessoas.Where(p => p.Email == pessoa.Email).SingleOrDefault();
        }

        public Pessoa BusacarPorUserName(string userName)
        {

            var user = _context.Users.FirstOrDefault(u => u.UserName == userName);
            Pessoa pessoa = null;
            if (user != null)
            {
                pessoa = _context.Pessoas
                    .Include(p => p.Endereco)
                    .FirstOrDefault(p => p.Id == user.PessoaId);
            }
            return pessoa;
        }

        public ApplicationUser BuscarUserPorPessoaId(int pessoaId)
        {
            var user = _context.Users.FirstOrDefault(u => u.PessoaId == pessoaId);

            return user;
        }
    }
}
