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
    public class FinanceiroController : Controller
    {
        private readonly IFinanceiroService _financeiroService;
        private readonly IAlunoService _alunoService;
        private readonly IPessoaService _pessoaService;

        public FinanceiroController(IFinanceiroService financeiroService,
            IAlunoService alunoService,
            IPessoaService pessoaService)
        {
            this._financeiroService = financeiroService;
            this._alunoService = alunoService;
            this._pessoaService = pessoaService;
        }

        public IActionResult Financeiro(int? alunoId)
        {
            if (alunoId != null)
            {
                return View(_alunoService.BuscarPorId(alunoId.Value));
            }
            else
            {
                return RedirectToAction("Index", "Aluno");
            }
        }

        [HttpGet]
        public IActionResult Form(int? financeiroId, int? alunoId)
        {
            if (alunoId != null)
            {
                ViewBag.Aluno = _alunoService.BuscarPorId(alunoId.Value);
                if (financeiroId == null)
                {
                    return View();
                }
                else
                {
                    return View(_financeiroService.BuscarPorId(financeiroId.Value));
                }
            }
            else
            {
                return RedirectToAction("Index", "Aluno");
            }
        }

        [HttpPost]
        public IActionResult Form(Financeiro financeiro)
        {

            if (financeiro.Id == null)
            {
                if (financeiro.AlunoId != null)
                {
                    ViewBag.Aluno = _alunoService.BuscarPorId(financeiro.AlunoId.Value);
                }

                financeiro.PessoaId = _pessoaService.GetUser(User.Identity.Name).Id;
                financeiro.UltimaAlteracao = NowHorarioBrasilia.GetNow();
                if (ModelState.IsValid)
                {
                    _financeiroService.Cadastrar(financeiro);
                    TempData["Success"] = "Lançamento cadastrado com sucesso";
                    return RedirectToAction("Financeiro", new { alunoId = financeiro.AlunoId });
                }
                else
                {
                    return View(financeiro);
                }
            }
            else
            {
                if (financeiro.AlunoId != null)
                {
                    ViewBag.Aluno = _alunoService.BuscarPorId(financeiro.AlunoId.Value);
                }

                financeiro.PessoaId = _pessoaService.GetUser(User.Identity.Name).Id;
                financeiro.UltimaAlteracao = NowHorarioBrasilia.GetNow();
                if (ModelState.IsValid)
                {
                    _financeiroService.Alterar(financeiro);
                    TempData["Success"] = "Lançamento alterado com sucesso";
                    return RedirectToAction("Financeiro", new { alunoId = financeiro.Aluno.Id });
                }
                else
                {
                    return View(financeiro);
                }
            }
        }

        public IActionResult ParcelasEmAberto(DateTime? inicial, DateTime? final)
        {

            if (inicial == null)
            {
                inicial = NowHorarioBrasilia.GetNow().AddDays(-NowHorarioBrasilia.GetNow().Day + 1);
            }

            if (final == null)
            {
                final = NowHorarioBrasilia.GetNow().AddDays(-NowHorarioBrasilia.GetNow().Day + 1).AddMonths(1).AddDays(-1);
            }

            ViewBag.Inicial = (DateTime)inicial;
            ViewBag.Final = (DateTime)final;

            return View(_financeiroService.ParcelasEmAberto((DateTime)inicial, (DateTime)final));
        }

        public IActionResult ParcelasVencidas()
        {
            return View(_financeiroService.BuscarAtrasador());
        }
    }
}