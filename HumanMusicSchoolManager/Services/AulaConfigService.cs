using HumanMusicSchoolManager.Data;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.ServicesInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Services
{
    public class AulaConfigService : IAulaConfigService
    {
        private readonly ApplicationDbContext _context;

        public AulaConfigService(ApplicationDbContext context)
        {
            this._context = context;
        }
        public void Alterar(AulaConfig aulaConfig)
        {
            _context.AulaConfig.Update(aulaConfig);
            _context.SaveChanges();
        }

        public AulaConfig Buscar()
        {
            return _context.AulaConfig.FirstOrDefault();
        }

        public void Cadastrar(AulaConfig aulaConfig)
        {
            _context.AulaConfig.Add(aulaConfig);
            _context.SaveChanges();
        }
    }
}
