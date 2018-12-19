using HumanMusicSchoolManager.ServicesInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Extensions
{
    public class EnvioEmailExtencions
    {
        private readonly IEmailConfigService _emailConfigService;
        public string Dominio { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }

        public EnvioEmailExtencions(IEmailConfigService emailConfigService)
        {
            this._emailConfigService = emailConfigService;
            var emailConfig = _emailConfigService.Buscar();
            this.Email = emailConfig.Email;
            this.Senha = emailConfig.Senha;
            this.Dominio = emailConfig.Dominio;
        }

        public void EnviarEmail(string titulo, string corpo, string remetente)
        {
            //create the mail message

            MailMessage mail = new MailMessage();

            //set the addresses
            mail.From = new MailAddress(Email); //IMPORTANT: This must be same as your smtp authentication address.
            mail.To.Add(remetente);

            //set the content
            mail.Subject = titulo;
            mail.Body = corpo;
            //send the message
            SmtpClient smtp = new SmtpClient(Dominio);

            //IMPORANT:  Your smtp login email MUST be same as your FROM address.
            NetworkCredential Credentials = new NetworkCredential(Email, Senha);
            smtp.Credentials = Credentials;
            smtp.Send(mail);
        }

    }
}
