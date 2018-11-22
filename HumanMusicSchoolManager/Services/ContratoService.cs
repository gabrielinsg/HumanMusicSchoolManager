using HumanMusicSchoolManager.Data;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.ServicesInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Services
{
    public class ContratoService : IContratoService
    {
        private readonly ApplicationDbContext _context;

        public ContratoService(ApplicationDbContext context)
        {
            this._context = context;
        }

        public void Alterar(Contrato contrato)
        {
            _context.Contratos.Update(contrato);
            _context.SaveChanges();
        }

        public Contrato BuscarPorId(int contratoId)
        {
            return _context.Contratos.FirstOrDefault(c => c.Id == contratoId);
        }

        public List<Contrato> BuscarPorNome(string nome)
        {
            return _context.Contratos.Where(c => c.Nome.Contains(nome)).ToList();
        }

        public List<Contrato> BuscarTodos()
        {
            return _context.Contratos.ToList();
        }

        public void Cadastrar(Contrato contrato)
        {
            _context.Contratos.Add(contrato);
            _context.SaveChanges();
        }

        public void Excluir(int contratoId)
        {
            var contrato = _context.Contratos.FirstOrDefault(c => c.Id == contratoId);
            if (contrato != null)
            {
                _context.Contratos.Remove(contrato);
                _context.SaveChanges();
            }
        }
    }
}
