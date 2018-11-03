using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.Models.ViewModels;
using HumanMusicSchoolManager.ServicesInterface;
using Microsoft.AspNetCore.Mvc;

namespace HumanMusicSchoolManager.Controllers
{
    public class PacoteCompraController : Controller
    {
        private readonly IPacoteCompraService _pacoteCompraService;
        private readonly IMatriculaService _matriculaService;
        private readonly IPacoteAulaService _pacoteAulaService;
        private readonly IAlunoService _alunoService;
        private readonly IPessoaService _pessoaService;
        private readonly IFinanceiroService _financeiroService;

        public PacoteCompraController(IPacoteCompraService pacoteCompraService,
            IMatriculaService matriculaService,
            IPacoteAulaService pacoteAulaService,
            IAlunoService alunoService,
            IPessoaService pessoaService,
            IFinanceiroService financeiroService)
        {
            this._pacoteCompraService = pacoteCompraService;
            this._matriculaService = matriculaService;
            this._pacoteAulaService = pacoteAulaService;
            this._alunoService = alunoService;
            this._pessoaService = pessoaService;
            this._financeiroService = financeiroService;
        }

        [HttpGet]
        public IActionResult Form(int? matriculaId, int? pacoteAulaId)
        {
            if (matriculaId != null)
            {
                var pacoteCompraViewModel = new PacoteCompraViewModel()
                {
                    Matricula = _matriculaService.BuscarPorId(matriculaId.Value),
                    PacotesAula = _pacoteAulaService.BuscarTodos(),
                    Vencimento = DateTime.Now
                };

                if (pacoteAulaId != null)
                {
                    pacoteCompraViewModel.PacoteAula = _pacoteAulaService.BuscarPorId(pacoteAulaId.Value);
                }

                return View(pacoteCompraViewModel);
            }
            else
            {
                return RedirectToAction("Financeiro", "Index");
            }
        }

        [HttpPost]
        public IActionResult Form(PacoteCompraViewModel pacoteCompraViewModel)
        {
            foreach (var model in ModelState)
            {
                ModelState.Remove(model.Key);
            }
            pacoteCompraViewModel.PacotesAula = _pacoteAulaService.BuscarTodos();
            if (pacoteCompraViewModel.Matricula.Id != null)
            {
                pacoteCompraViewModel.Matricula = _matriculaService.BuscarPorId(pacoteCompraViewModel.Matricula.Id.Value);
                pacoteCompraViewModel.PacoteCompra.Matricula = pacoteCompraViewModel.Matricula;
            }
            else
            {
                ModelState.AddModelError("Matricula", "Compra sem Matrícula");
            }

            if (pacoteCompraViewModel.PacoteAula.Id != null)
            {
                pacoteCompraViewModel.PacoteAula = _pacoteAulaService.BuscarPorId(pacoteCompraViewModel.PacoteAula.Id.Value);
                pacoteCompraViewModel.PacoteCompra.PacoteAula = pacoteCompraViewModel.PacoteAula;
            }
            else
            {
                ModelState.AddModelError("PacoteAula", "Selecione um pacote de aula");
            }

            if (pacoteCompraViewModel.PacoteCompra.Desconto == 0)
            {
                pacoteCompraViewModel.PacoteCompra.Desconto = null;
            }

            if (pacoteCompraViewModel.Vencimento == null)
            {
                ModelState.AddModelError("Vencimento", "Escolha uma data de vencimento");
            }

            if (ModelState.IsValid)
            {
                decimal? desconto = 0;
                if (pacoteCompraViewModel.PacoteCompra.Desconto != null)
                {
                    desconto = pacoteCompraViewModel.PacoteCompra.Desconto;
                }
                var valor = (pacoteCompraViewModel.PacoteCompra.PacoteAula.Valor - desconto) / pacoteCompraViewModel.PacoteCompra.QtdParcela;
                pacoteCompraViewModel.PacoteCompra = _pacoteCompraService.Cadastrar(pacoteCompraViewModel.PacoteCompra);
                for (int i = 0; i < pacoteCompraViewModel.PacoteCompra.QtdParcela; i++)
                {
                    var financeiro = new Financeiro()
                    {
                        Aluno = pacoteCompraViewModel.Matricula.Aluno,
                        Nome = pacoteCompraViewModel.PacoteAula.Nome + " - " + pacoteCompraViewModel.Matricula.Curso.Nome + " - Parcela " + (i + 1) + " de " + pacoteCompraViewModel.PacoteCompra.QtdParcela,
                        DataGerada = DateTime.Now,
                        FormaPagamento = pacoteCompraViewModel.FormaPagamento,
                        Pessoa = _pessoaService.GetUser(User.Identity.Name),
                        Valor = Math.Round(valor.Value, 2),
                        DataVencimento = pacoteCompraViewModel.Vencimento.AddMonths(i),
                        PacoteCompra = pacoteCompraViewModel.PacoteCompra
                    };
                    _financeiroService.Cadastrar(financeiro);
                }
                return RedirectToAction("Financeiro", "Financeiro", new { alunoId = pacoteCompraViewModel.Matricula.Aluno.Id.Value });
            }
            else
            {

                return View(pacoteCompraViewModel);
            }
        }
    }
}