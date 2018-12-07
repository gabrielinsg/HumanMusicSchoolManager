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
    public class TrancamentoController : Controller
    {
        private readonly ITrancamentoService _trancamentoService;
        private readonly IPacoteCompraService _pacoteCompraService;
        private readonly IAulaService _aulaService;
        private readonly IFeriadoService _feriadoService;
        private readonly IChamadaService _chamadaService;

        public TrancamentoController(ITrancamentoService trancamentoService,
            IPacoteCompraService pacoteCompraService,
            IAulaService aulaService,
            IFeriadoService feriadoService,
            IChamadaService chamadaService)
        {
            this._trancamentoService = trancamentoService;
            this._pacoteCompraService = pacoteCompraService;
            this._aulaService = aulaService;
            this._feriadoService = feriadoService;
            this._chamadaService = chamadaService;
        }

        [HttpGet]
        public IActionResult Form(int? pacoteCompraId)
        {
            if (pacoteCompraId != null)
            {
                var pacoteCompra = _pacoteCompraService.BuscarPorId(pacoteCompraId.Value);
                if (pacoteCompra != null)
                {
                    if (pacoteCompra.Trancamento == null)
                    {
                        if (pacoteCompra.Chamadas.FirstOrDefault(c => c.Presenca != null) != null)
                        {
                            ViewBag.PacoteCompra = pacoteCompra;
                            return View();
                        }

                        TempData["Warning"] = "Para trancamento é preciso ter feito pelo menos uma aula";
                        return RedirectToAction("Aluno", "Aluno", new { alunoId = pacoteCompra.Matricula.AlunoId });

                    }
                    else
                    {
                        TempData["Warning"] = "Aluno já fez trancamento para esse pacote de aula";
                        return RedirectToAction("Aluno", "Aluno", new { alunoId = pacoteCompra.Matricula.AlunoId });
                    }
                }
            }
            TempData["Error"] = "Não foi possível localizar o pacote";
            return RedirectToAction("Index", "Aluno");
        }

        [HttpPost]
        public IActionResult Form(Trancamento trancamento)
        {
            var pacoteCompra = _pacoteCompraService.BuscarPorId(trancamento.PacoteCompraId);
            if (trancamento.DataFinal > pacoteCompra.Chamadas.OrderByDescending(c => c.Aula.Data).FirstOrDefault().Aula.Data)
            {
                ModelState.AddModelError("DataFinal", "Data final não pode ser maior que a última aula do aluno que é dia " + pacoteCompra.Chamadas.OrderByDescending(c => c.Aula.Data).FirstOrDefault().Aula.Data.ToString("dd/MM/yyyy"));
            }

            if (trancamento.DataInicial.DayOfWeek != (DayOfWeek)pacoteCompra.Matricula.DispSala.Dia)
            {
                ModelState.AddModelError("DataInicial", "Data Inicial deve ser " + pacoteCompra.Matricula.DispSala.Dia.GetType()
                        .GetMember(pacoteCompra.Matricula.DispSala.Dia.ToString())
                        .First()
                        .GetCustomAttribute<DisplayAttribute>()
                        .Name);
            }

            if (trancamento.DataFinal.DayOfWeek != (DayOfWeek)pacoteCompra.Matricula.DispSala.Dia)
            {
                ModelState.AddModelError("DataFinal", "Data Final deve ser " + pacoteCompra.Matricula.DispSala.Dia.GetType()
                        .GetMember(pacoteCompra.Matricula.DispSala.Dia.ToString())
                        .First()
                        .GetCustomAttribute<DisplayAttribute>()
                        .Name);
            }

            if (ModelState.IsValid)
            {
                trancamento.DataFinal = trancamento.DataFinal.AddHours(23);
                List<Chamada> chamadas = pacoteCompra.Chamadas.Where(c => (c.Aula.Data >= trancamento.DataInicial && c.Aula.Data <= trancamento.DataFinal) && c.Presenca == null).ToList();
                foreach (var chamada in chamadas)
                {
                    var aulaAntiga = chamada.Aula;

                    DateTime dataAula = chamadas.OrderByDescending(c => c.Aula.Data).FirstOrDefault().Aula.Data;
                    Feriado feriado = null;
                    do
                    {
                        dataAula = dataAula.AddDays(7);
                        feriado = _feriadoService.BuscarPorData(dataAula);
                    } while (feriado != null);

                    var aula = _aulaService.BuscarPorDiaHora(dataAula);

                    if (aula == null)
                    {

                        aula = new Aula()
                        {
                            CursoId = chamada.PacoteCompra.Matricula.CursoId,
                            ProfessorId = chamada.PacoteCompra.Matricula.DispSala.Professor.Id.Value,
                            SalaId = chamada.PacoteCompra.Matricula.DispSala.Sala.Id.Value,
                            Data = dataAula,
                            DataLimite = dataAula.AddDays(3)
                        };
                        _aulaService.Cadastrar(aula);
                    }

                    chamada.AulaId = aula.Id.Value;
                    _chamadaService.Alterar(chamada);
                    aulaAntiga = _aulaService.BuscarPorId(aulaAntiga.Id.Value);
                    if (aulaAntiga.Chamadas.Count == 0 && aulaAntiga.Demostrativas.Count == 0)
                    {
                        _aulaService.Excluir(aulaAntiga.Id.Value);
                    }
                }

                _trancamentoService.Cadastrar(trancamento);

                TempData["Success"] = "Trancamento realizado com sucesso.";

                return RedirectToAction("Aluno", "Aluno", new { alunoId = pacoteCompra.Matricula.AlunoId });
            }
            else
            {
                ViewBag.PacoteCompra = pacoteCompra;
                return View(trancamento);
            }
        }

        public IActionResult ExcluirTrancamento(int? pacoteCompraId)
        {

            if (pacoteCompraId != null)
            {
                var pacoteCompra = _pacoteCompraService.BuscarPorId(pacoteCompraId.Value);
                List<Aula> aulasAntigas = new List<Aula>();
                if (pacoteCompra != null)
                {
                    var chamadas = pacoteCompra.Chamadas;

                    foreach (var chamada in chamadas.Where(c => c.Presenca == null).ToList())
                    {
                        aulasAntigas.Add(chamada.Aula);
                        chamadas[chamadas.IndexOf(chamada)].AulaId = null;
                    }

                    var dataAula = chamadas.OrderByDescending(c => c.Aula.Data).FirstOrDefault(c => c.Presenca != null).Aula.Data;

                    foreach (var chamada in chamadas.Where(c => c.AulaId == null).ToList())
                    {
                        if (dataAula != null)
                        {
                            Feriado feriado = null;

                            do
                            {
                                dataAula = dataAula.AddDays(7);
                            } while (dataAula <= DateTime.Now);

                            do
                            {
                                feriado = _feriadoService.BuscarPorData(dataAula);
                                if (feriado != null)
                                {
                                    dataAula = dataAula.AddDays(7);
                                }
                            } while (feriado != null);

                            var aula = _aulaService.BuscarPorDiaHora(dataAula);
                            if (aula == null)
                            {

                                aula = new Aula()
                                {
                                    CursoId = chamada.PacoteCompra.Matricula.CursoId,
                                    ProfessorId = chamada.PacoteCompra.Matricula.DispSala.Professor.Id.Value,
                                    SalaId = chamada.PacoteCompra.Matricula.DispSala.Sala.Id.Value,
                                    Data = dataAula,
                                    DataLimite = dataAula.AddDays(3)
                                };
                                _aulaService.Cadastrar(aula);
                            }
                            chamada.AulaId = aula.Id.Value;
                            _chamadaService.Alterar(chamada);
                            dataAula = dataAula.AddDays(7);
                        }
                    }

                    foreach (var aulaAntiga in aulasAntigas)
                    {
                        if (aulaAntiga != null)
                        {
                            var aula = _aulaService.BuscarPorId(aulaAntiga.Id.Value);
                            if (aula.Chamadas == null)
                            {
                                aula.Chamadas = new List<Chamada>();
                            }
                            if (aula.Demostrativas == null)
                            {
                                aula.Demostrativas = new List<Demostrativa>();
                            }
                            if (aula.Chamadas.Count == 0 && aula.Demostrativas.Count == 0)
                            {
                                _aulaService.Excluir(aula.Id.Value);
                            }
                        }
                    }
                    _trancamentoService.Excluir(pacoteCompra.Trancamento.Id.Value);
                    TempData["Success"] = "Cancelamento do trancamento feito com sucesso. Acesse o calendário para conferir as aulas.";
                    return RedirectToAction("Aluno", "Aluno", new { alunoId = pacoteCompra.Matricula.AlunoId });
                }
            }

            TempData["Error"] = "Pacote de aula não encontrado";
            return RedirectToAction("Index", "Aluno");
        }
    }
}