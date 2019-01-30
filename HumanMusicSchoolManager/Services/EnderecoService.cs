using HumanMusicSchoolManager.Data;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.ServicesInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Services
{
    public class EnderecoService : IEnderecoService
    {
        private readonly ApplicationDbContext _context;

        public EnderecoService(ApplicationDbContext context)
        {
            this._context = context;
        }

        public List<Endereco> EnderecosSemPessoa()
        {
            return _context.Enderecos.ToList();
        }

        public void Excluir(int enderecoId)
        {
            var endereco = _context.Enderecos.SingleOrDefault(e => e.Id == enderecoId);
            if (endereco != null)
            {
                _context.Remove(endereco);
                _context.SaveChanges();
            }
        }
    }
}
