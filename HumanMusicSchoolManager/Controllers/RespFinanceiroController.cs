using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.ServicesInterface;
using Microsoft.AspNetCore.Mvc;

namespace HumanMusicSchoolManager.Controllers
{
    public class RespFinanceiroController : Controller
    {
        private readonly IRespFinanceiroService _respFinanceiroService;

        public RespFinanceiroController(IRespFinanceiroService respFinanceiroService)
        {
            this._respFinanceiroService = respFinanceiroService;
        }

        public IActionResult Index()
        {
            return View(_respFinanceiroService.BuscarTodos());
        }

        [HttpGet]
        public IActionResult Form(int? respFinanceiroId)
        {
            if (respFinanceiroId == null)
            {
                return View();
            }
            else
            {
                return View(_respFinanceiroService.BuscarPorId(respFinanceiroId.Value));
            }
        }

        [HttpPost]
        public IActionResult Form(RespFinanceiro respFinanceiro)
        {
            if (respFinanceiro.Id == null)
            {
                if (ModelState.IsValid)
                {
                    var cpfUnico = _respFinanceiroService.BuscarPorCPF(respFinanceiro.CPF);
                    if (cpfUnico == null)
                    {
                        _respFinanceiroService.Cadastrar(respFinanceiro);

                        TempData["Success"] = "Responsável financeiro Cadastrado com sucesso!";

                        return RedirectToAction("Form");
                    }
                    else
                    {
                        ModelState.AddModelError("CPF", "CPF já cadastrado para outro responsável financeiro");
                        return View(respFinanceiro);
                    }

                }
                else
                {
                    return View(respFinanceiro);
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    _respFinanceiroService.Alterar(respFinanceiro);

                    TempData["Success"] = "Responsável financeiro Alterado com sucesso!";

                    return RedirectToAction("Form");

                }
                else
                {
                    return View(respFinanceiro);
                }
            }
        }

        public IActionResult Excluir(int respFinanceiroId)
        {
            _respFinanceiroService.Excluir(respFinanceiroId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult BuscarPorNome(string nome)
        {
            var respFinanceiro = _respFinanceiroService.BuscarPorNome(nome);
            return Json(respFinanceiro);
        }
    }
}