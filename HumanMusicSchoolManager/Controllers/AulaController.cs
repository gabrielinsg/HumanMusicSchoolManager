using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.ServicesInterface;
using Microsoft.AspNetCore.Mvc;

namespace HumanMusicSchoolManager.Controllers
{
    public class AulaController : Controller
    {
        private readonly IAulaService _aulaService;
        private readonly IProfessorService _professorService;

        public AulaController(IAulaService aulaService,
            IProfessorService professorService)
        {
            this._aulaService = aulaService;
            this._professorService = professorService;
        }

        [HttpGet]
        public IActionResult Aula(int? aulaId)
        {
            if (aulaId != null)
            {
                var aula = _aulaService.BuscarPorId(aulaId.Value);
                if (aula != null)
                {
                    return View(aula);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult Aula(Aula aula)
        {
            if (aula.DescAtividades == null)
            {
                ModelState.AddModelError("DescAtividades", "Descrição de atividades obrigatória");
            }
            else if (aula.DescAtividades.Length < 7)
            {
                ModelState.AddModelError("DescAtividades", "Descrição de atividades deve ter no mínimo 6 caracteres");
            }

            if (ModelState.IsValid)
            {
                _aulaService.Alterar(aula);
                return RedirectToAction("Calendario", "Professor", new { professorId = aula.Professor.Id });
            }
            else
            {
                return View(aula);
            }
        }
    }
}