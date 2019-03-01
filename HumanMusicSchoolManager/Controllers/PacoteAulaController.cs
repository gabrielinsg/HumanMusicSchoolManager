using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.Services;
using HumanMusicSchoolManager.ServicesInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HumanMusicSchoolManager.Controllers
{
    [Authorize(Roles = "Admin, Secretaria")]
    public class PacoteAulaController : Controller
    {
        private readonly IPacoteAulaService _pacoteAulaService;
        private readonly IContratoService _contratoService;

        public PacoteAulaController(IPacoteAulaService pacoteAulaService,
            IContratoService contratoService)
        {
            this._pacoteAulaService = pacoteAulaService;
            this._contratoService = contratoService;
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
                var selectListItens = new List<SelectListItem>();
                var contratos = _contratoService.BuscarTodos();
                foreach (var contrato in contratos)
                {
                    var selectListItem = new SelectListItem
                    {
                        Value = contrato.Id.ToString(),
                        Text = contrato.Nome
                    };
                    selectListItens.Add(selectListItem);
                }
                ViewBag.Contratos = selectListItens;
                return View();
            }
            else
            {
                var selectListItens = new List<SelectListItem>();
                var contratos = _contratoService.BuscarTodos();
                foreach (var contrato in contratos)
                {
                    var selectListItem = new SelectListItem
                    {
                        Value = contrato.Id.ToString(),
                        Text = contrato.Nome
                    };
                    selectListItens.Add(selectListItem);
                }
                ViewBag.Contratos = selectListItens;
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
                    var selectListItens = new List<SelectListItem>();
                    var contratos = _contratoService.BuscarTodos();
                    foreach (var contrato in contratos)
                    {
                        var selectListItem = new SelectListItem
                        {
                            Value = contrato.Id.ToString(),
                            Text = contrato.Nome
                        };
                        selectListItens.Add(selectListItem);
                    }
                    ViewBag.Contratos = selectListItens;
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