using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.Services;
using HumanMusicSchoolManager.ServicesInterface;
using Microsoft.AspNetCore.Mvc;

namespace HumanMusicSchoolManager.Controllers
{
    public class AulaController : Controller
    {
        private readonly IAulaService _aulaService;
        private readonly IProfessorService _professorService;
        private readonly ICursoService _cursoService;
        private readonly ISalaService _salaService;
        private readonly IChamadaService _chamadaService;

        public AulaController(IAulaService aulaService,
            IProfessorService professorService,
            ICursoService cursoService,
            ISalaService salaService,
            IChamadaService chamadaService)
        {
            this._aulaService = aulaService;
            this._professorService = professorService;
            this._cursoService = cursoService;
            this._salaService = salaService;
            this._chamadaService = chamadaService;
        }

        [HttpGet]
        public IActionResult Aula(int? aulaId)
        {
            if (aulaId != null)
            {
                var aula = _aulaService.BuscarPorId(aulaId.Value);

                if (aula != null)
                {

                    if (aula.DataLimite == null || aula.DataLimite > DateTime.Now)
                    {
                        return View(aula);
                    }
                    else
                    {
                        TempData["Error"] = "Data limite para lançamento dessa aula expirou. Favor entrar em contato com a coordenação!";
                        return RedirectToAction("Calendario", "Professor");
                    }

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

            aula.AulaDada = true;

            if (ModelState.IsValid)
            {
                foreach (var chamada in aula.Chamadas)
                {
                    _chamadaService.Alterar(chamada);
                }
                _aulaService.Alterar(aula);
                return RedirectToAction("Calendario", "Professor", new { professorId = aula.ProfessorId });
            }
            else
            {
                return View(aula);
            }
        }
    }
}