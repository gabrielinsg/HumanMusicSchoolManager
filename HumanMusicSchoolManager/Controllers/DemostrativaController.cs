using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HumanMusicSchoolManager.Extensions;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.Models.ViewModels;
using HumanMusicSchoolManager.Services;
using HumanMusicSchoolManager.ServicesInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HumanMusicSchoolManager.Controllers
{
    [Authorize(Roles = "Admin, Vendas, Atendimento, Professor")]
    public class DemostrativaController : Controller
    {
        private readonly IDemostrativaService _DemostrativaService;
        private readonly IChamadaService _chamadaService;
        private readonly IDispSalaService _dispSalaService;
        private readonly ICursoService _cursoService;
        private readonly IAulaService _aulaService;
        private readonly IFeriadoService _feriadoService;
        private readonly ICandidatoService _candidatoService;
        private readonly IPessoaService _pessoaService;
        private readonly IEmailConfigService _emailConfigService;
        private readonly IAulaConfigService _aulaConfigService;

        public DemostrativaController(IDemostrativaService DemostrativaService,
            IChamadaService chamadaService,
            IDispSalaService dispSalaService,
            ICursoService cursoService,
            IAulaService aulaService,
            IFeriadoService feriadoService,
            ICandidatoService candidatoService,
            IPessoaService pessoaService,
            IEmailConfigService emailConfigService,
            IAulaConfigService aulaConfigService)
        {
            this._DemostrativaService = DemostrativaService;
            this._chamadaService = chamadaService;
            this._dispSalaService = dispSalaService;
            this._cursoService = cursoService;
            this._aulaService = aulaService;
            this._feriadoService = feriadoService;
            this._candidatoService = candidatoService;
            this._pessoaService = pessoaService;
            this._emailConfigService = emailConfigService;
            this._aulaConfigService = aulaConfigService;
        }

        [HttpGet]
        public IActionResult Form(int? cursoId, int? dispSalaId, int? candidatoId)
        {
            if (candidatoId != null)
            {
                
                var DemostrativaViewModel = new DemostrativaViewModel
                {
                    Candidato = _candidatoService.BuscarPorId(candidatoId.Value),
                    Cursos = _cursoService.BuscarTodos(),
                    DispSalas = _dispSalaService.HorariosDisponiveis()
                };

                if (dispSalaId != null)
                {
                    DemostrativaViewModel.DispSala = _dispSalaService.BuscarPorId(dispSalaId.Value);
                }
                if (cursoId != null)
                {
                    DemostrativaViewModel.Curso = _cursoService.BuscarPorId(cursoId.Value);
                }

                DemostrativaViewModel.DiaAula = NowHorarioBrasilia.GetNow();

                if (dispSalaId == null)
                {
                    ViewBag.Aba = 1;
                } else
                {
                    ViewBag.Aba = 2;
                }

                return View(DemostrativaViewModel);
            }
            else
            {
                TempData["Error"] = "Candidato não localizado";
                return RedirectToAction("Index", "Candidato");
            }
        }

        [HttpPost]
        public IActionResult Form(DemostrativaViewModel DemostrativaViewModel)
        {
            foreach (var model in ModelState)
            {
                ModelState.Remove(model.Key);
            }
            DemostrativaViewModel.DispSala = _dispSalaService.BuscarPorId(DemostrativaViewModel.DispSala.Id.Value);
            DemostrativaViewModel.Candidato = _candidatoService.BuscarPorId(DemostrativaViewModel.Candidato.Id.Value);
            if ((DayOfWeek)DemostrativaViewModel.DispSala.Dia != DemostrativaViewModel.DiaAula.DayOfWeek)
            {
                var dia = DemostrativaViewModel.DispSala.Dia.GetType()
                    .GetMember(DemostrativaViewModel.DispSala.Dia.ToString())
                    .First()
                    .GetCustomAttribute<DisplayAttribute>()
                    .Name;
                ModelState.AddModelError("DiaAula", "A aula deve ser em uma " + dia);
            }

            if (DemostrativaViewModel.DiaAula == null)
            {
                ModelState.AddModelError("DiaAula", "Dia de aula deve ser preenchido");
            }

            DemostrativaViewModel.DiaAula = DemostrativaViewModel.DiaAula.AddHours((double)DemostrativaViewModel.DispSala.Hora);

            var feriado = _feriadoService.BuscarPorData(DemostrativaViewModel.DiaAula);

            if (feriado != null)
            {
                ModelState.AddModelError("DiaAula", "Não é possível agendar para este dia - " + feriado.Nome);
            }

            if (DemostrativaViewModel.DiaAula.Date < NowHorarioBrasilia.GetNow().Date)
            {
                ModelState.AddModelError("DiaAula", "Dia da aula não pode ser menor que hoje");
            }


            if (ModelState.IsValid)
            {

                DemostrativaViewModel.Demostrativa.DispSalaId = DemostrativaViewModel.DispSala.Id.Value;
                DemostrativaViewModel.Demostrativa.CursoId = DemostrativaViewModel.Curso.Id.Value;
                DemostrativaViewModel.Demostrativa.ProfessorId = DemostrativaViewModel.DispSala.Professor.Id;
                DemostrativaViewModel.Demostrativa.Dia = DemostrativaViewModel.DispSala.Dia;
                DemostrativaViewModel.Demostrativa.Hora = DemostrativaViewModel.DispSala.Hora;
                DemostrativaViewModel.Demostrativa.CandidatoId = DemostrativaViewModel.Candidato.Id.Value;
                DemostrativaViewModel.Demostrativa.PessoaId = _pessoaService.BusacarPorUserName(User.Identity.Name).Id.Value;
                DemostrativaViewModel.Demostrativa.Confirmado = Confirmado.NAO;
                DemostrativaViewModel.Demostrativa.Data = DemostrativaViewModel.DiaAula;

                var aula = _aulaService.BuscarPorDiaHora(DemostrativaViewModel.DiaAula, DemostrativaViewModel.DispSala);
                if (aula == null)
                {

                    aula = new Aula()
                    {
                        CursoId = DemostrativaViewModel.Curso.Id.Value,
                        ProfessorId = DemostrativaViewModel.DispSala.Professor.Id.Value,
                        SalaId = DemostrativaViewModel.DispSala.Sala.Id.Value,
                        Data = DemostrativaViewModel.DiaAula,
                        DataLimite = DemostrativaViewModel.DiaAula.AddDays(_aulaConfigService.Buscar().TempoLimiteLancamento)
                    };
                    _aulaService.Cadastrar(aula);
                }


                DemostrativaViewModel.Demostrativa.Aula = aula;
                _DemostrativaService.Cadastrar(DemostrativaViewModel.Demostrativa);

                //Enviar email professor
                string corpo = "Adicionado uma desmonstrativa " +
                    " - " + DemostrativaViewModel.Demostrativa.DispSala.Dia.GetType()
                            .GetMember(DemostrativaViewModel.Demostrativa.DispSala.Dia.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetName() + " às " + DemostrativaViewModel.Demostrativa.DispSala.Hora.ToString("00h'00'") + " para " + DemostrativaViewModel.Demostrativa.Candidato.Nome;

                var email = new EnvioEmailExtencions(_emailConfigService);

                email.EnviarEmail("Atualização na Agenda", corpo, DemostrativaViewModel.Demostrativa.DispSala.Professor.Email);

                return RedirectToAction("Candidato", "Candidato", new { candidatoId = DemostrativaViewModel.Candidato.Id.Value });
            }
            else
            {
                DemostrativaViewModel.DispSalas = _dispSalaService.HorariosDisponiveis();
                DemostrativaViewModel.Cursos = _cursoService.BuscarTodos();
                DemostrativaViewModel.Candidato = _candidatoService.BuscarPorId(DemostrativaViewModel.Candidato.Id.Value);
                if (DemostrativaViewModel.Demostrativa.Id != null)
                {
                    DemostrativaViewModel.Demostrativa = _DemostrativaService.BuscarPorId(DemostrativaViewModel.Demostrativa.Id.Value);
                }
                if (DemostrativaViewModel.DispSala.Id != null)
                {
                    DemostrativaViewModel.DispSala = _dispSalaService.BuscarPorId(DemostrativaViewModel.DispSala.Id.Value);
                }
                ViewBag.Aba = 2;
                return View(DemostrativaViewModel);
            }
        }

        [HttpPost]
        public IActionResult FinalizarDemostrativa(Demostrativa Demostrativa, bool Contratou)
        {
            Demostrativa.DispSalaId = null;
            Demostrativa.Contratou = Contratou;
            if (Demostrativa.AulaId != null)
            {
                var aula = _aulaService.BuscarPorId(Demostrativa.AulaId.Value);
                if (aula.AulaDada == false)
                {
                    Demostrativa.AulaId = null;
                    aula.Demostrativas.Remove(Demostrativa);
                    if (aula.Demostrativas.Count <= 1 && aula.Chamadas.Count == 0)
                    {
                        _aulaService.Excluir(aula.Id.Value);
                    }
                }
            }
            _DemostrativaService.Alterar(Demostrativa);

            TempData["Success"] = "Demostrativa alterada com sucesso";
            if (Demostrativa.Contratou == true)
            {

                return RedirectToAction("Candidato", "Aluno", new { candidatoId = Demostrativa.CandidatoId });
            }
            else
            {
                return RedirectToAction("Candidato", "Candidato", new { candidatoId = Demostrativa.CandidatoId });
            }

        }

        public IActionResult DemostrativasAbertas()
        {
            return View(_DemostrativaService.DemostrativasAbertas());
        }

        public IActionResult AlterarDemostrativa(Demostrativa Demostrativa, string Observacao)
        {

            var demo = _DemostrativaService.BuscarPorId(Demostrativa.Id.Value);
            demo.Confirmado = Demostrativa.Confirmado;
            demo.Observacao = Observacao;
            _DemostrativaService.Alterar(demo);

            return RedirectToAction("Candidato", "Candidato", new { candidatoId = demo.CandidatoId });
        }

        public IActionResult NaoContratou(DateTime? inicial, DateTime? final)
        {
            if (inicial == null)
            {
                inicial = NowHorarioBrasilia.GetNow().AddDays(-NowHorarioBrasilia.GetNow().Day + 1);
            }

            if (final == null)
            {
                final = NowHorarioBrasilia.GetNow().AddDays(-NowHorarioBrasilia.GetNow().Day + 1).AddMonths(1).AddDays(-1);
            }

            ViewBag.Inicial = inicial.Value.Date;
            ViewBag.Final = final.Value.Date;
            return View(_DemostrativaService.DemostrativasNaoContrataram(inicial.Value, final.Value));
        }

        [HttpPost]
        public void AutoSaveObservacao(int id, string conteudo)
        {
            _DemostrativaService.AtualizarObservacao(id, conteudo);
        }

        [HttpPost]
        public void AutoSaveConfimado(int id, Confirmado confirmado)
        {
            _DemostrativaService.AtualizarConfirmado(id, confirmado);
        }

        [HttpPost]
        public void AutoSavePresenca(int id, bool presenca)
        {
            var demonstrativa = _DemostrativaService.BuscarPorId(id);
            demonstrativa.Presenca = presenca;
            _DemostrativaService.Alterar(demonstrativa);
        }
    }
}