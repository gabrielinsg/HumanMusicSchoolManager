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
    [Authorize(Roles = "Admin, Atendimento, Coordenacao, Secretaria")]
    public class ValeController : Controller
    {
        private readonly IValeService _valeService;
        private readonly IProfessorService _professorService;
        private readonly IFuncionarioService _funcionarioService;
        private readonly ICaixaService _caixaService;
        private readonly IPessoaService _pessoaService;

        public ValeController(IValeService valeService,
            IProfessorService professorService,
            IFuncionarioService funcionarioService,
            ICaixaService caixaService,
            IPessoaService pessoaService)
        {
            this._valeService = valeService;
            this._professorService = professorService;
            this._funcionarioService = funcionarioService;
            this._caixaService = caixaService;
            this._pessoaService = pessoaService;
        }

        public IActionResult Index(DateTime? inicial, DateTime? final)
        {
            if (inicial == null)
            {
                inicial = NowHorarioBrasilia.GetNow().AddDays(-NowHorarioBrasilia.GetNow().Day + 1);
            }

            if (final == null)
            {
                final = NowHorarioBrasilia.GetNow().AddDays(-NowHorarioBrasilia.GetNow().Day + 1).AddMonths(1).AddDays(-1);
            }

            ViewBag.Inicial = inicial;
            ViewBag.Final = final;

            return View(_valeService.BuscarTodos(inicial.Value, final.Value));
        }

        [HttpGet]
        public IActionResult LancarVale()
        {
            if (_caixaService.CaixaAberto())
            {
                return View();
            }

            TempData["Warning"] = "Não existe caixa aberto para realizar essa operação!";

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult LancarVale(Vale vale)
        {
            vale.Data = NowHorarioBrasilia.GetNow();

            if (ModelState.IsValid)
            {
                if (_caixaService.CaixaAberto())
                {
                    _valeService.Lancar(vale);

                    var transacaoCaixa = new TransacaoCaixa()
                    {
                        Data = NowHorarioBrasilia.GetNow(),
                        FuncionarioId = _pessoaService.BusacarPorUserName(User.Identity.Name).Id.Value,
                        CaixaId = _caixaService.BuscarCaixaAberto().Id,
                        Valor = vale.Valor,
                        Entrada = false,
                        Descricao = "Vale para " + _pessoaService.BuscarPorId(vale.PessoaId).Nome,
                        FormaPagamento = FormaPagamento.DINHEIRO
                    };

                    _caixaService.IncluirTransacao(transacaoCaixa);

                    TempData["Success"] = "Vale lançado com Sucesso!";
                    return RedirectToAction("Index");
                }

                TempData["Warning"] = "Não existe caixa aberto para realizar essa operação!";

                return RedirectToAction("Index");
            }

            return View(vale);
        }

        public IActionResult Excluir(int valeId)
        {
            var vale = _valeService.BuscarPorId(valeId);

            var transacaoCaixa = new TransacaoCaixa()
            {
                Data = NowHorarioBrasilia.GetNow(),
                FuncionarioId = _pessoaService.BusacarPorUserName(User.Identity.Name).Id.Value,
                CaixaId = _caixaService.BuscarCaixaAberto().Id,
                Valor = vale.Valor,
                Entrada = true,
                Descricao = "Cancelamento vale para " + _pessoaService.BuscarPorId(vale.PessoaId).Nome,
                FormaPagamento = FormaPagamento.DINHEIRO
            };

            _caixaService.IncluirTransacao(transacaoCaixa);

            _valeService.Excluir(valeId);

            TempData["Success"] = "Vale excluido com sucesso";

            return RedirectToAction("Index");
        }

        public JsonResult BuscarPorNome(string nome, string tipo)
        {
            switch (tipo)
            {
                case "Professor": return Json(_professorService.BuscarPorNome(nome));
                case "Funcionario": return Json(_funcionarioService.BuscarPorNome(nome)); 
                default: return Json(new Object());
            }
        }
    }
}