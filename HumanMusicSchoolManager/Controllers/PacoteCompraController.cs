using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
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
        private readonly IFeriadoService _feriadoService;
        private readonly IAulaService _aulaService;
        private readonly IChamadaService _chamadaService;

        public PacoteCompraController(IPacoteCompraService pacoteCompraService,
            IMatriculaService matriculaService,
            IPacoteAulaService pacoteAulaService,
            IAlunoService alunoService,
            IPessoaService pessoaService,
            IFinanceiroService financeiroService,
            IFeriadoService feriadoService,
            IAulaService aulaService,
            IChamadaService chamadaService)
        {
            this._pacoteCompraService = pacoteCompraService;
            this._matriculaService = matriculaService;
            this._pacoteAulaService = pacoteAulaService;
            this._alunoService = alunoService;
            this._pessoaService = pessoaService;
            this._financeiroService = financeiroService;
            this._feriadoService = feriadoService;
            this._aulaService = aulaService;
            this._chamadaService = chamadaService;
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
                    Vencimento = DateTime.Now,
                    PrimeiraAula = DateTime.Now
                };

                if (pacoteAulaId != null)
                {
                    pacoteCompraViewModel.PacoteAula = _pacoteAulaService.BuscarPorId(pacoteAulaId.Value);
                }

                return View(pacoteCompraViewModel);
            }
            else
            {
                return RedirectToAction("Aluno", "Index");
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

            if (pacoteCompraViewModel.PrimeiraAula == null || pacoteCompraViewModel.PrimeiraAula.Date < DateTime.Now.Date)
            {
                ModelState.AddModelError("PrimeiraAula", "Primeira aula deve ser selecionado a partir de hoje");
            }

            if (pacoteCompraViewModel.PrimeiraAula != null)
            {
                if ((DayOfWeek)pacoteCompraViewModel.Matricula.DispSala.Dia != pacoteCompraViewModel.PrimeiraAula.DayOfWeek)
                {
                    var dia = pacoteCompraViewModel.Matricula.DispSala.Dia.GetType()
                        .GetMember(pacoteCompraViewModel.Matricula.DispSala.Dia.ToString())
                        .First()
                        .GetCustomAttribute<DisplayAttribute>()
                        .Name;
                    ModelState.AddModelError("PrimeiraAula", "A primeira aula precisa ser " + dia);
                }
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

                //Gerar Financeiros
                for (int i = 0; i < pacoteCompraViewModel.PacoteCompra.QtdParcela; i++)
                {
                    var financeiro = new Financeiro()
                    {
                        Aluno = pacoteCompraViewModel.Matricula.Aluno,
                        Nome = pacoteCompraViewModel.PacoteAula.Nome + " - " + pacoteCompraViewModel.Matricula.Curso.Nome + " - Parcela " + (i + 1) + " de " + pacoteCompraViewModel.PacoteCompra.QtdParcela,
                        UltimaAlteracao = DateTime.Now,
                        FormaPagamento = pacoteCompraViewModel.FormaPagamento,
                        Pessoa = _pessoaService.GetUser(User.Identity.Name),
                        Valor = Math.Round(valor.Value, 2),
                        DataVencimento = pacoteCompraViewModel.Vencimento.AddMonths(i),
                        PacoteCompra = pacoteCompraViewModel.PacoteCompra
                    };
                    _financeiroService.Cadastrar(financeiro);
                }

                //Gerar Aulas
                var diaAula = pacoteCompraViewModel.PrimeiraAula;
                var qtdAulas = pacoteCompraViewModel.PacoteAula.QtdAula;
                diaAula = diaAula.AddHours((double)pacoteCompraViewModel.Matricula.DispSala.Hora);

                while (qtdAulas > 0)
                {
                    var feriado = _feriadoService.BuscarPorData(diaAula);
                    if (feriado == null)
                    {
                        //criando as aulas
                        var aula = _aulaService.BuscarPorDiaHora(diaAula);
                        var chamada = new Chamada()
                        {
                            PacoteCompraId = pacoteCompraViewModel.PacoteCompra.Id.Value
                        };
                        if (aula == null)
                        {
                            
                            aula = new Aula()
                            {
                                CursoId = pacoteCompraViewModel.Matricula.CursoId,
                                ProfessorId = pacoteCompraViewModel.Matricula.DispSala.Professor.Id.Value,
                                SalaId = pacoteCompraViewModel.Matricula.DispSala.Sala.Id.Value,
                                Data = diaAula
                            };
                            _aulaService.Cadastrar(aula);
                        }

                        chamada.Aula = aula;
                        chamada.Presenca = null;
                        _chamadaService.Cadastrar(chamada);

                        //incremento
                        qtdAulas--;
                    }
                    diaAula = diaAula.AddDays(7);
                }

                return RedirectToAction("Aluno", "Aluno", new { alunoId = pacoteCompraViewModel.Matricula.Aluno.Id.Value });
            }
            else
            {

                return View(pacoteCompraViewModel);
            }
        }
    }
}