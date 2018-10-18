using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace HumanMusicSchoolManager.Controllers
{
    public class PacoteAulaController : Controller
    {
        private readonly IPacoteAulaService _pacoteAulaService;

        public PacoteAulaController(IPacoteAulaService pacoteAulaService)
        {
            this._pacoteAulaService = pacoteAulaService;
        }

        public IActionResult Index()
        {
            return View(_pacoteAulaService.BuscarTodos());
        }

        [HttpGet]
        public IActionResult Form(int? pacoteAulaId)
        {
            if (pacoteAulaId == null)
            {
                return View();
            }
            else
            {
                var pacoteAula = _pacoteAulaService.BuscarPorId(pacoteAulaId.Value);
                return View(pacoteAula);
            }
        }

        [HttpPost]
        public IActionResult Form(PacoteAula pacoteAula)
        {
            if (pacoteAula.Id == null)
            {
                if (ModelState.IsValid)
                {
                    _pacoteAulaService.Cadastrar(pacoteAula);
                    TempData["Success"] = "Pacote de Aula cadastrado com sucesso!";
                    return RedirectToAction("Form");
                }
                else
                {
                    return View(pacoteAula);
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    _pacoteAulaService.Alterar(pacoteAula);
                    TempData["Success"] = "Pacote de Aula alterado com sucesso!";
                    return RedirectToAction("Form");
                }
                else
                {
                    return View(pacoteAula);
                }
            }
        }

        [HttpPost]
        public JsonResult BuscarPorNome(string nome)
        {
            var pacoteAula = _pacoteAulaService.BuscarPorNome(nome);
            return Json(pacoteAula);
        }
    }
}