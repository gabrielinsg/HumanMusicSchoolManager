using HumanMusicSchoolManager.Data;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.ServicesInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Services
{
    public class EmailConfigService : IEmailConfigService
    {
        private readonly ApplicationDbContext _context;

        public EmailConfigService(ApplicationDbContext context)
        {
            this._context = context;
        }

        public void Alterar(EmailConfig emailConfig)
        {
            _context.EmailConfig.Update(emailConfig);
            _context.SaveChanges();
        }

        public EmailConfig Buscar()
        {
            return _context.EmailConfig.FirstOrDefault();
        }

        public void Cadastrar(EmailConfig emailConfig)
        {
            _context.EmailConfig.Add(emailConfig);
            _context.SaveChanges();
        }

        public void Excluir()
        {
            var emailConfig = _context.EmailConfig.FirstOrDefault();
            if (emailConfig != null)
            {
                _context.EmailConfig.Remove(emailConfig);
                _context.SaveChanges();
            }
        }
    }
}
