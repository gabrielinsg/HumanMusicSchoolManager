using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HumanMusicSchoolManager.Extensions;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.Models.ViewModels;
using HumanMusicSchoolManager.ServicesInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HumanMusicSchoolManager.Controllers
{
    [Authorize(Roles = "Admin, Atendimento")]
    public class TrancamentoController : Controller
    {
        private readonly ITrancamentoService _trancamentoService;
        private readonly IPacoteCompraService _pacoteCompraService;
        private readonly IAulaService _aulaService;
        private readonly IFeriadoService _feriadoService;
        private readonly IChamadaService _chamadaService;
        private readonly IPessoaService _pessoaService;
        private readonly IRelatorioMatriculaService _relatorioMatriculaService;
        private readonly IEmailConfigService _emailConfigService;
        private readonly IMatriculaService _matriculaService;

        public TrancamentoController(ITrancamentoService trancamentoService,
            IPacoteCompraService pacoteCompraService,
            IAulaService aulaService,
            IFeriadoService feriadoService,
            IChamadaService chamadaService,
            IPessoaService pessoaService,
            IRelatorioMatriculaService relatorioMatriculaService,
            IEmailConfigService emailConfigService,
            IMatriculaService matriculaService)
        {
            this._trancamentoService = trancamentoService;
            this._pacoteCompraService = pacoteCompraService;
            this._aulaService = aulaService;
            this._feriadoService = feriadoService;
            this._chamadaService = chamadaService;
            this._pessoaService = pessoaService;
            this._relatorioMatriculaService = relatorioMatriculaService;
            this._emailConfigService = emailConfigService;
            this._matriculaService = matriculaService;
        }

        [HttpGet]
        public IActionResult Form(int? pacoteCompraId)
        {
            if (pacoteCompraId != null)
            {
                var pacoteCompra = _pacoteCompraService.BuscarPorId(pacoteCompraId.Value);
                if (pacoteCompra != null)
                {
                    return View(new TrancamentoViewModel(pacoteCompra));
                }
            }
            TempData["Error"] = "Não foi possível localizar o pacote";
            return RedirectToAction("Index", "Aluno");
        }

        [HttpPost]
        public IActionResult Form(Trancamento trancamento)
        {
            var pacoteCompra = _pacoteCompraService.BuscarPorId(trancamento.PacoteCompraId);
            if (trancamento.DataInicial > pacoteCompra.Chamadas.OrderByDescending(c => c.Aula.Data).FirstOrDefault().Aula.Data)
            {
                ModelState.AddModelError("DataInicial", "Data inicial não pode ser maior que a última aula do aluno que é dia " + pacoteCompra.Chamadas.OrderByDescending(c => c.Aula.Data).FirstOrDefault().Aula.Data.ToString("dd/MM/yyyy"));
            }

            if (trancamento.DataInicial >= trancamento.DataFinal)
            {
                ModelState.AddModelError("DataInicial", "Data inicial não pode ser igual ou maior a data final");
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
                    chamada.AulaId = null;
                    _chamadaService.Alterar(chamada);
                    aulaAntiga = _aulaService.BuscarPorId(aulaAntiga.Id.Value);
                    if (aulaAntiga.Chamadas.Count == 0 && aulaAntiga.Demostrativas.Count == 0)
                    {
                        _aulaService.Excluir(aulaAntiga.Id.Value);
                    }
                }

                DateTime? dataAula = null;
                foreach (var chamada in chamadas)
                {
                    
                    if (pacoteCompra.Chamadas.FirstOrDefault(c => c.Presenca != null) != null)
                    {
                        dataAula = pacoteCompra.Chamadas.OrderByDescending(c => c.Aula.Data).FirstOrDefault(c => c.Aula != null).Aula.Data;
                    }
                    if (dataAula == null || dataAula < trancamento.DataFinal)
                    {
                        dataAula = trancamento.DataFinal;
                    }
                    Feriado feriado = null;
                    do
                    {
                        dataAula = dataAula.Value.AddDays(7);
                        feriado = _feriadoService.BuscarPorData(dataAula.Value);
                    } while (feriado != null);


                    var aula = _aulaService.BuscarPorDiaHora(dataAula.Value, chamada.PacoteCompra.Matricula.DispSala);

                    if (aula == null)
                    {

                        aula = new Aula()
                        {
                            CursoId = chamada.PacoteCompra.Matricula.CursoId,
                            ProfessorId = chamada.PacoteCompra.Matricula.DispSala.Professor.Id.Value,
                            SalaId = chamada.PacoteCompra.Matricula.DispSala.Sala.Id.Value,
                            Data = dataAula.Value,
                            DataLimite = dataAula.Value.AddDays(3)
                        };
                        _aulaService.Cadastrar(aula);
                    }

                    chamada.AulaId = aula.Id.Value;
                    _chamadaService.Alterar(chamada);
                    dataAula.Value.AddDays(7);                    
                }
                trancamento.Data = NowHorarioBrasilia.GetNow();
                _trancamentoService.Cadastrar(trancamento);


                //Relatório Matrícula
                var relatorioMatricula = new RelatorioMatricula
                {
                    PessoaId = _pessoaService.BusacarPorUserName(User.Identity.Name).Id.Value,
                    MatriculaId = pacoteCompra.Matricula.Id.Value,
                    Data = NowHorarioBrasilia.GetNow()
                };
                string descricao = "Trancamento feito do dia " + trancamento.DataInicial.ToString("dd/MM/yyyy") + " até dia " +
                    trancamento.DataFinal.ToString("dd/MM/yyyy");
                relatorioMatricula.Descricao = descricao;

                _relatorioMatriculaService.Cadastrar(relatorioMatricula);

                //Enviar email
                var envioEmail = new EnvioEmailExtencions(_emailConfigService);
                var matricula = _matriculaService.BuscarPorId(trancamento.PacoteCompra.MatriculaId);
                string corpo = matricula.Aluno.Nome + " fez trancamento do dia " +
                    trancamento.DataInicial.ToString("dd/MM/yyyy") + " até dia " +
                    trancamento.DataFinal.ToString("dd/MM/yyyy");
                envioEmail.EnviarEmail("Aviso Trancamento", corpo, matricula.DispSala.Professor.Email);

                TempData["Success"] = "Trancamento realizado com sucesso.";

                return RedirectToAction("Aluno", "Aluno", new { alunoId = pacoteCompra.Matricula.AlunoId });
            }
            else
            {


                return View(new TrancamentoViewModel(trancamento, pacoteCompra));
            }
        }

        public IActionResult ExcluirTrancamento(int? pacoteCompraId)
        {

            if (pacoteCompraId != null)
            {
                var pacoteCompra = _pacoteCompraService.BuscarPorId(pacoteCompraId.Value);

                RefazerAulas(pacoteCompra);

                if (pacoteCompra.Trancamento != null)
                {
                    _trancamentoService.Excluir(pacoteCompra.Trancamento.Id.Value);
                }

                var relatorioMatricula = new RelatorioMatricula
                {
                    PessoaId = _pessoaService.BusacarPorUserName(User.Identity.Name).Id.Value,
                    MatriculaId = pacoteCompra.Matricula.Id.Value,
                    Data = NowHorarioBrasilia.GetNow()
                };
                string descricao = "Trancamento cancelado";
                relatorioMatricula.Descricao = descricao;

                _relatorioMatriculaService.Cadastrar(relatorioMatricula);

                TempData["Success"] = "Trancamento Excluido com sucesso. Favor verificar o calendário";
                return RedirectToAction("Aluno", "Aluno", new { alunoId = pacoteCompra.Matricula.AlunoId });
            }

            TempData["Error"] = "Pacote de aula não encontrado";
            return RedirectToAction("Index", "Aluno");
        }

        private void RefazerAulas(PacoteCompra pacoteCompra)
        {
            if (pacoteCompra != null)
            {
                var chamadas = pacoteCompra.Chamadas.Where(c => c.Presenca == null).ToList();

                //tirar as aulas
                foreach (var chamada in chamadas)
                {
                    var aulaAntiga = chamada.Aula;
                    chamada.AulaId = null;
                    _chamadaService.Alterar(chamada);
                    aulaAntiga = _aulaService.BuscarPorId(aulaAntiga.Id.Value);
                    if (aulaAntiga.Chamadas.Count == 0 && aulaAntiga.Demostrativas.Count == 0)
                    {
                        _chamadaService.Excluir(aulaAntiga.Id.Value);
                    }
                }

                DateTime aula = DateTime.Now.Date;
                while(aula.DayOfWeek != (DayOfWeek)pacoteCompra.Matricula.DispSala.Dia)
                {
                    aula = aula.AddDays(1);
                }
                aula = aula.AddHours(pacoteCompra.Matricula.DispSala.Hora);
                foreach (var chamada in chamadas)
                {
                    Feriado feriado = null;
                    do
                    {
                        feriado = _feriadoService.BuscarPorData(aula);
                        if (feriado != null)
                        {
                            aula = aula.AddDays(7);
                        }
                    } while (feriado != null);



                    var novaAula = _aulaService.BuscarPorDiaHora(aula, chamada.PacoteCompra.Matricula.DispSala);

                    if (novaAula == null)
                    {
                        novaAula = new Aula()
                        {
                            CursoId = chamada.PacoteCompra.Matricula.CursoId,
                            ProfessorId = chamada.PacoteCompra.Matricula.DispSala.Professor.Id.Value,
                            SalaId = chamada.PacoteCompra.Matricula.DispSala.Sala.Id.Value,
                            Data = aula,
                            DataLimite = aula.AddDays(3)
                        };
                        _aulaService.Cadastrar(novaAula);
                    }
                    chamada.AulaId = novaAula.Id.Value;
                    _chamadaService.Alterar(chamada);
                    aula = aula.AddDays(7);
                }  
            }
        }
    }
}