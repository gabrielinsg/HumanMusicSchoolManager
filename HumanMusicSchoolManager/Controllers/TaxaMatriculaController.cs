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
    [Authorize]
    public class TaxaMatriculaController : Controller
    {
        private readonly ITaxaMatriculaService _taxaMatriculaService;

        public TaxaMatriculaController(ITaxaMatriculaService taxaMatriculaService)
        {
            this._taxaMatriculaService = taxaMatriculaService;
        }

        public IActionResult Index()
        {
            return View(_taxaMatriculaService.BuscarTodos());
        }

        [HttpGet]
        public IActionResult Form(int? taxaMatriculaId)
        {
            if (taxaMatriculaId == null)
            {
                return View();
            }
            else
            {
                var taxaMatricula = _taxaMatriculaService.BuscarPorId(taxaMatriculaId.Value);
                if (taxaMatricula == null)
                {
                    return View();
                }
                else
                {
                    return View(taxaMatricula);
                }
            }
        }

        [HttpPost]
        public IActionResult Form(TaxaMatricula taxaMatricula)
        {
            if (taxaMatricula.Id == null)
            {
                if (ModelState.IsValid)
                {
                    _taxaMatriculaService.Cadastrar(taxaMatricula);
                    TempData["Success"] = "Taxa de Matrícula cadastrado com sucesso!";
                    return RedirectToAction("Form");
                }
                else
                {
                    return View(taxaMatricula);
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    _taxaMatriculaService.Alterar(taxaMatricula);
                    TempData["Success"] = "Taxa de Matrícula alterado com sucesso";
                    return RedirectToAction("Form");
                }
                else
                {
                    return View(taxaMatricula);
                }
            }
        }

        public JsonResult BuscarPorNome(string nome)
        {
            return Json(_taxaMatriculaService.BuscarPorNome(nome));
        }
    }
}