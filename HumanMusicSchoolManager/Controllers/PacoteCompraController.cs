﻿using System;
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
                    if (financeiro.FormaPagamento == FormaPagamento.DEBITO || financeiro.FormaPagamento == FormaPagamento.CREDITO)
                    {
                        financeiro.ValorPago = valor;
                        financeiro.DataPagamento = DateTime.Now;
                    }
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
                                Data = diaAula,
                                DataLimite = diaAula.AddDays(3)
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

        public IActionResult PacoteCompra(int? pacoteCompraId)
        {
            if (pacoteCompraId != null)
            {
                return View(_pacoteCompraService.BuscarPorId(pacoteCompraId.Value));
            }
            else
            {
                return RedirectToAction("Index", "Aluno");
            }
        }

        public JsonResult Aulas(int pacoteCompraId)
        {

            var pacoteCompra = _pacoteCompraService.BuscarPorId(pacoteCompraId);
            var compradas = pacoteCompra.PacoteAula.QtdAula;
            var feitas = pacoteCompra.Chamadas.Where(c => c.Presenca != null).ToList().Count();

            var chart = new Chart()
            {
                Labels = new string[] { "Disponíveis", "Feitas" },
                Datasets = new double[] { compradas - feitas, feitas },
                Color = new string[] { "#007bff", "#28a745" }
            };

            return Json(chart);
        }

        public JsonResult Presencas(int pacoteCompraId)
        {

            var pacoteCompra = _pacoteCompraService.BuscarPorId(pacoteCompraId);
            var presencas = pacoteCompra.Chamadas.Where(c => c.Presenca == true).ToList().Count();
            var faltas = pacoteCompra.Chamadas.Where(c => c.Presenca == false).ToList().Count();


            var chart = new Chart()
            {
                Labels = new string[] { "Presenças", "Faltas" },
                Datasets = new double[] { presencas, faltas },
                Color = new string[] { "#28a745", "#dc3545" }
            };

            return Json(chart);
        }

        internal class Chart
        {
            public string[] Labels { get; set; }
            public double[] Datasets { get; set; }
            public string[] Color { get; set; }
        }

        public JsonResult Calendario(int pacoteCompraId)
        {
            var pacoteCompra = _pacoteCompraService.BuscarPorId(pacoteCompraId);
            var feriados = _feriadoService.BuscarTodos();
            var calendar = new List<Calendar>();
            var cont = 1;
            foreach (var chamada in pacoteCompra.Chamadas)
            {
                var start = chamada.Aula.Data;
                var end = start.AddMinutes(55);
                var color = "";
                if (chamada.Presenca == null)
                {
                    color = "#007bff";
                }
                else if (chamada.Presenca == true)
                {
                    color = "#28a745";
                }
                else
                {
                    color = "#dc3545";
                }
                var cal = new Calendar()
                {
                    Title = "Aula " + cont,
                    Start = start.ToString("yyyy-MM-ddTHH:mm:ss"),
                    End = end.ToString("yyyy-MM-ddTHH:mm:ss"),
                    Color = color,
                    Url = "/PacoteCompra/Aula?chamadaId=" + chamada.Id
                };
                calendar.Add(cal);
                cont++;
            }
            foreach (var feriado in feriados)
            {
                var cal = new Calendar()
                {
                    Title = feriado.Nome,
                    Start = feriado.DataInicial.ToString("yyyy-MM-dd"),
                    Color = "#ffc107"
                };
                if (feriado.DataFinal != null)
                {
                    cal.End = feriado.DataFinal.Value.ToString("yyyy-MM-dd");
                }
                calendar.Add(cal);
            }

            return Json(calendar);

        }

        internal class Calendar
        {
            public string Title { get; set; }
            public string Start { get; set; }
            public string End { get; set; }
            public string Color { get; set; }
            public string Url { get; set; }
        }

        public IActionResult PrintCalendario(int? pacoteCompraId)
        {
            if (pacoteCompraId != null)
            {
                return View(_pacoteCompraService.BuscarPorId(pacoteCompraId.Value));
            }
            else
            {
                return RedirectToAction("Index", "Aluno");
            }
        }

        public IActionResult Aula(int? chamadaId)
        {
            if (chamadaId != null)
            {
                var chamada = _chamadaService.BuscarPorId(chamadaId.Value);
                if (chamada != null)
                {
                    return View(chamada);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}