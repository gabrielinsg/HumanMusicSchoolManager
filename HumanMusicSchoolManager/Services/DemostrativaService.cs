using HumanMusicSchoolManager.Data;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.ServicesInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Services
{
    public class DemostrativaService : IDemostrativaService
    {
        private readonly ApplicationDbContext _context;

        public DemostrativaService(ApplicationDbContext context)
        {
            this._context = context;
        }

        public void Alterar(Demostrativa demostrativa)
        {
            _context.Demostrativas.Update(demostrativa);
            _context.SaveChanges();
        }

        public Demostrativa BuscarPorId(int demostrativaId)
        {
           return _context.Demostrativas.FirstOrDefault(d => d.Id == demostrativaId);
        }

        public List<Demostrativa> BuscarTodos()
        {
            return _context.Demostrativas.ToList();
        }

        public void Cadastrar(Demostrativa demostrativa)
        {
            _context.Demostrativas.Add(demostrativa);
            _context.SaveChanges();
        }

        public void Excluir(int demostrativaId)
        {
            var demostrativa = _context.Demostrativas.FirstOrDefault(d => d.Id == demostrativaId);
            if (demostrativa != null)
            {
                _context.Demostrativas.Remove(demostrativa);
                _context.SaveChanges();
            }
        }
    }
}
