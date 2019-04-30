using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.ServicesInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HumanMusicSchoolManager.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AulaConfigController : Controller
    {
        private readonly IAulaConfigService _aulaConfigService;
        private readonly IAulaService _aulaService;

        public AulaConfigController(IAulaConfigService aulaConfigService,
            IAulaService aulaService)
        {
            this._aulaConfigService = aulaConfigService;
            this._aulaService = aulaService;
        }

        [HttpGet]
        public IActionResult Form()
        {
            var aulaConfig = _aulaConfigService.Buscar();
            if (aulaConfig == null)
            {
                return View();
            }
            else
            {
                return View(aulaConfig);
            }
        }

        [HttpPost]
        public IActionResult Form(AulaConfig aulaConfig, int? horaAntiga)
        {
            if (ModelState.IsValid)
            {
                if (aulaConfig.Id == null)
                {
                    _aulaConfigService.Cadastrar(aulaConfig);
                }
                else
                {
                    _aulaConfigService.Alterar(aulaConfig);
                }
                if (horaAntiga == null || horaAntiga != aulaConfig.TempoLimiteLancamento)
                {
                    _aulaService.AlterarTempoLimite(aulaConfig.TempoLimiteLancamento);
                }
                TempData["Success"] = "Configurações de aula alterado com sucesso!";
                return RedirectToAction("Form");
            }
            else
            {
                return View(aulaConfig);
            }
        }
    }
}