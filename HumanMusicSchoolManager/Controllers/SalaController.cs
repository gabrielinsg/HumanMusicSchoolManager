using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.Models.ViewModels;
using HumanMusicSchoolManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace HumanMusicSchoolManager.Controllers
{
    public class SalaController : Controller
    {
        private readonly ISalaService _salaService;
        private readonly ICursoService _cursoService;

        public SalaController(ISalaService salaService, ICursoService cursoService)
        {
            this._salaService = salaService;
            this._cursoService = cursoService;
        }

        public IActionResult Index()
        {
            var salas = new SalaViewModel(_salaService.BuscarTodos(), _cursoService.BuscarTodos());
            return View(salas);
        }

        [HttpGet]
        public IActionResult Form(int? salaId)
        {
            if (salaId == null)
            {
                return View();
            }
            else
            {
                var sala = _salaService.BuscarPorId(salaId.Value);
                if (sala != null)
                {
                    return View(sala);
                }
                else
                {
                    return View();
                }
            }
        }

        [HttpPost]
        public IActionResult Form(Sala sala)
        {
            if (sala.Id == null)
            {
                if (ModelState.IsValid)
                {
                    _salaService.Cadastrar(sala);
                    TempData["Success"] = "Sala cadastrada com sucesso!";
                    return RedirectToAction("Form");
                }
                else
                {
                    return View(sala);
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    _salaService.Alterar(sala);
                    TempData["Success"] = "Sala alterada com sucesso!";
                    return RedirectToAction("Form");
                }
                else
                {
                    return View(sala);
                }
            }
        }

        public IActionResult AdicionarCurso(int? cursoId, int? salaId)
        {

            if (salaId != null && cursoId != null)
            {
                _salaService.AdicionarCurso(salaId.Value, cursoId.Value);
            }

            return RedirectToAction(actionName: "SalaCurso", routeValues: new { salaId = salaId });

        }

        public IActionResult RemoverCurso(int? cursoId, int? salaId)
        {

            if (salaId != null && cursoId != null)
            {
                _salaService.RemoverCurso(salaId.Value, cursoId.Value);
            }

            return RedirectToAction(actionName: "SalaCurso", routeValues: new { salaId = salaId });

        }

        public IActionResult SalaCurso(int? salaId)
        {
            if (salaId != null)
            {
                var professor = new SalaCursoViewModel(_salaService.BuscarPorId(salaId.Value), _cursoService.BuscarTodos().Where(c => c.Ativo == true).ToList());
                return View(professor);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public JsonResult BuscarPorNome(string nome)
        {
            var sala = _salaService.BuscarPorNome(nome);
            return Json(sala);
        }
    }
}