using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.Models.ViewModels;
using HumanMusicSchoolManager.ServicesInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HumanMusicSchoolManager.Controllers
{
    [Authorize]
    public class CaixaController : Controller
    {
        private readonly ICaixaService _caixaService;
        private readonly IFuncionarioService _funcionarioService;
        private readonly IPessoaService _pessoaService;

        public CaixaController(
            ICaixaService caixaService,
            IFuncionarioService funcionarioService,
            IPessoaService pessoaService)
        {
            this._caixaService = caixaService;
            this._funcionarioService = funcionarioService;
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

            var caixaViewModel = new CaixaIndexViewModel()
            {
                Caixas = _caixaService.ListarCaixas(inicial.Value, final.Value),
                Inicial = inicial,
                Final = final
            };

            return View(caixaViewModel);
        }

        public IActionResult Form()
        {

            if (_caixaService.CaixaAberto())
            {
                var user = User.Identity;
                var pessoa = _pessoaService.BusacarPorUserName(user.Name);
                var funcionario = _funcionarioService.BuscarPorId(pessoa.Id.Value) ;
                _caixaService.AbrirCaixa(funcionario);
                TempData["Success"] = "Caixa aberto com sucesso";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "O Caixa anterior ainda não foi fechado";
                return RedirectToAction("Index");
            }
        }

        public IActionResult Caixa(int? caixaId)
        {
            var caixaViewModel = new CaixaViewModel();
            if (caixaId != null)
            {
                caixaViewModel.Caixa = _caixaService.BuscarCaixa(caixaId.Value);
            }
            else
            {
                var caixa = _caixaService.BuscarCaixaAberto();
                if (caixa == null)
                {
                    TempData["Warning"] = "Não existe caixa aberto para realizar essa operação";
                    return RedirectToAction("Index");
                }
                else
                {
                    caixaViewModel.Caixa = caixa;
                }
            }

            return View(caixaViewModel);
        }

        [HttpPost]
        public IActionResult Fechar()
        {
            var funcionario = _funcionarioService.BuscarPorId(_pessoaService.BusacarPorUserName(User.Identity.Name).Id.Value);
            _caixaService.FecharCaixa(funcionario);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult LancarTransacao(CaixaViewModel caixaViewModel)
        {
            caixaViewModel.TransacaoCaixa.Data = NowHorarioBrasilia.GetNow();
            caixaViewModel.TransacaoCaixa.Funcionario = _funcionarioService.BuscarPorId(_pessoaService.BusacarPorUserName(User.Identity.Name).Id.Value);            
            caixaViewModel.Caixa = _caixaService.BuscarCaixa(caixaViewModel.Caixa.Id);
            caixaViewModel.TransacaoCaixa.Caixa = caixaViewModel.Caixa;

            if (ModelState.IsValid)
            {
                _caixaService.IncluirTransacao(caixaViewModel.TransacaoCaixa);
                caixaViewModel.TransacaoCaixa = new TransacaoCaixa();
                TempData["Success"] = "Transação lançada com sucesso";
                return RedirectToAction("Caixa", new { caixaId = caixaViewModel.Caixa.Id});
            }

            
            return View("Caixa", caixaViewModel);

        }

        public IActionResult Imprimir(int caixaId)
        {
            return View(_caixaService.BuscarCaixa(caixaId));
        }
    }
}