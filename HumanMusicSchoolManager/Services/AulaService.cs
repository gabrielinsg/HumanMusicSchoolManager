using HumanMusicSchoolManager.Data;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.ServicesInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Services
{
    public class AulaService : IAulaService
    {
        private readonly ApplicationDbContext _context;

        public AulaService(ApplicationDbContext context)
        {
            this._context = context;
        }

        public void Alterar(Aula aula)
        {
            _context.Update(aula);
            _context.SaveChanges();
        }

        public Aula BuscarPorDiaHora(DateTime data)
        {
            return _context.Aulas.FirstOrDefault(a => a.Data == data);
        }

        public Aula BuscarPorId(int aulaId)
        {
            return _context.Aulas.FirstOrDefault(a => a.Id == aulaId);
        }

        public List<Aula> BuscarTodas()
        {
            return _context.Aulas.ToList();
        }

        public void Cadastrar(Aula aula)
        {
            _context.Aulas.Add(aula);
            _context.SaveChanges();
        }

        public void Excluir(int aulaId)
        {
            var aula = _context.Aulas.FirstOrDefault(a => a.Id == aulaId);
            if (aula != null)
            {
                _context.Aulas.Remove(aula);
                _context.SaveChanges();
            }
        }
    }
}
