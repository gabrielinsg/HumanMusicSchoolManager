using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanMusicSchoolManager.Extensions;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.Services;
using HumanMusicSchoolManager.ServicesInterface;
using Microsoft.AspNetCore.Mvc;

namespace HumanMusicSchoolManager.Controllers
{
    public class EmailConfigController : Controller
    {
        private readonly IEmailConfigService _emailConfigService;

        public EmailConfigController(IEmailConfigService emailConfigService)
        {
            this._emailConfigService = emailConfigService;
        }

        [HttpGet]
        public IActionResult Form()
        {
            var emailConfig = _emailConfigService.Buscar();

            if (emailConfig == null)
            {
                return View();
            }
            else
            {
                return View(emailConfig);
            }
        }

        [HttpPost]
        public IActionResult Form(EmailConfig emailConfig)
        {

            if (ModelState.IsValid)
            {
                if (emailConfig.Id == null)
                {
                    _emailConfigService.Cadastrar(emailConfig);
                    TempData["Success"] = "Configuração de E-Mail cadastrado com sucesso";
                    return RedirectToAction("Form");
                }
                else
                {
                    _emailConfigService.Alterar(emailConfig);
                    TempData["Success"] = "Configuração de E-Mail alterado com sucesso";
                    return RedirectToAction("Form");
                }
            }

            return View(emailConfig);
        }

        public IActionResult Teste()
        {
            var host = HttpContext.Request;
            var email = new EnvioEmailExtencions(_emailConfigService);
            string corpo = "<b>Teste</b>"+host.Host;
            email.EnviarEmail("Teste com html", corpo, "gabriel.insg@gmail.com");

            return RedirectToAction("Index", "Home");
        }
    }
}