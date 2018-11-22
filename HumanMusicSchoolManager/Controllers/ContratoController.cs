using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.ServicesInterface;
using Microsoft.AspNetCore.Mvc;

namespace HumanMusicSchoolManager.Controllers
{
    public class ContratoController : Controller
    {
        private readonly IContratoService _contratoService;

        public ContratoController(IContratoService contratoService)
        {
            this._contratoService = contratoService;
        }

        public IActionResult Index()
        {
            return View(_contratoService.BuscarTodos());
        }

        [HttpGet]
        public IActionResult Form(int? contratoId)
        {
            if (contratoId == null)
            {
                return View();
            }
            else
            {
                var contrato = _contratoService.BuscarPorId(contratoId.Value);
                if (contrato != null)
                {
                    return View(contrato);
                }
                else
                {
                    TempData["Error"] = "Contrato não encontrato!";
                    return RedirectToAction("Index");
                }
            }
        }

        [HttpPost]
        public IActionResult Form(Contrato contrato)
        {
            if (ModelState.IsValid)
            {
                var mensagem = "";
                if (contrato.Id == null)
                {
                    _contratoService.Cadastrar(contrato);
                    mensagem = "Contrato Cadastrado com sucesso!";
                }
                else
                {
                    _contratoService.Alterar(contrato);
                    mensagem = "Contrato Alterado com sucesso!";
                }
                TempData["Success"] = mensagem;
                return RedirectToAction("Form");
            }
            else
            {
                return View(contrato);
            }
        }

        public IActionResult Excluir(int? contratoId)
        {
            if (contratoId != null)
            {
                _contratoService.Excluir(contratoId.Value);
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Contrato não encontrado!";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public JsonResult BuscarPorNome(string nome)
        {
            var contrato = _contratoService.BuscarPorNome(nome);
            return Json(contrato);
        }
    }
}