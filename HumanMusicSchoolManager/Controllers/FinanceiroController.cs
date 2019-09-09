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
    [Authorize(Roles = "Admin, Atendimento, Financeiro")]
    public class FinanceiroController : Controller
    {
        private readonly IFinanceiroService _financeiroService;
        private readonly IAlunoService _alunoService;
        private readonly IPessoaService _pessoaService;
        private readonly ICaixaService _caixaService;

        public FinanceiroController(IFinanceiroService financeiroService,
            IAlunoService alunoService,
            IPessoaService pessoaService,
            ICaixaService caixaService)
        {
            this._financeiroService = financeiroService;
            this._alunoService = alunoService;
            this._pessoaService = pessoaService;
            this._caixaService = caixaService;
        }

        [Authorize(Roles = "Admin, Atendimento, Financeiro, Aluno")]
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
        public IActionResult Form(Financeiro financeiro, bool lancarCaixa)
        {
            
            if ((financeiro.ValorPago != null || financeiro.ValorPago == ' ' ) && financeiro.DataPagamento == null)
            {
                ModelState.AddModelError("DataPagamento", "Data de Pagamento obrigatório se Valor Pago for preenchido");
            }

            if (lancarCaixa && _caixaService.CaixaAberto())
            {
                ModelState.AddModelError("CaixaAberto", "Não existe caixa aberto");
                TempData["Error"] = "Não existe caixa aberto para o lançamento!";
            }

            if (lancarCaixa && (financeiro.ValorPago == null || financeiro.ValorPago <= 0))
            {
                ModelState.AddModelError("ValorPago", "Não é possível lançar um valor nulo ou menos que 0 no caixa");
                TempData["Error"] = "Não é possível lançar um valor nulo ou menos que 0 no caixa";
            }

            if (ModelState.IsValid)
            {
                if (financeiro.Id == null)
                {
                    if (financeiro.AlunoId != null)
                    {
                        ViewBag.Aluno = _alunoService.BuscarPorId(financeiro.AlunoId.Value);
                    }

                    financeiro.PessoaId = _pessoaService.BusacarPorUserName(User.Identity.Name).Id;
                    financeiro.UltimaAlteracao = NowHorarioBrasilia.GetNow();
                    if (ModelState.IsValid)
                    {
                        _financeiroService.Cadastrar(financeiro);

                        if (lancarCaixa)
                        {
                            var transacaoCaixa = new TransacaoCaixa()
                            {
                                Data = NowHorarioBrasilia.GetNow(),
                                FuncionarioId = _pessoaService.BusacarPorUserName(User.Identity.Name).Id.Value,
                                CaixaId = _caixaService.BuscarCaixaAberto().Id,
                                Valor = financeiro.ValorPago.Value,
                                Entrada = true,
                                Descricao = financeiro.Nome + ". Aluno: " + _alunoService.BuscarPorId(financeiro.AlunoId.Value).Nome,
                                FormaPagamento = financeiro.FormaPagamento
                            };

                            _caixaService.IncluirTransacao(transacaoCaixa);
                        }

                        

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

                    financeiro.PessoaId = _pessoaService.BusacarPorUserName(User.Identity.Name).Id;
                    financeiro.UltimaAlteracao = NowHorarioBrasilia.GetNow();
                    if (ModelState.IsValid)
                    {
                        _financeiroService.Alterar(financeiro);

                        if (lancarCaixa)
                        {
                            var transacaoCaixa = new TransacaoCaixa()
                            {
                                Data = NowHorarioBrasilia.GetNow(),
                                FuncionarioId = _pessoaService.BusacarPorUserName(User.Identity.Name).Id.Value,
                                CaixaId = _caixaService.BuscarCaixaAberto().Id,
                                Valor = financeiro.ValorPago.Value,
                                Entrada = true,
                                Descricao = financeiro.Nome + ". Aluno: " + _alunoService.BuscarPorId(financeiro.AlunoId.Value).Nome,
                                FormaPagamento = financeiro.FormaPagamento
                            };

                            _caixaService.IncluirTransacao(transacaoCaixa);
                        }

                        TempData["Success"] = "Lançamento alterado com sucesso";
                        return RedirectToAction("Financeiro", new { alunoId = financeiro.Aluno.Id });
                    }
                    else
                    {
                        return View(financeiro);
                    }
                }
            }
            ViewBag.LancarCaixa = lancarCaixa;
            ViewBag.Aluno = _alunoService.BuscarPorId(financeiro.AlunoId.Value);
            return View(financeiro);
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