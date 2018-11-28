using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.Models.ViewModels;
using HumanMusicSchoolManager.Services;
using HumanMusicSchoolManager.ServicesInterface;
using Microsoft.AspNetCore.Mvc;

namespace HumanMusicSchoolManager.Controllers
{
    public class ReposicaoController : Controller
    {
        private readonly IReposicaoService _reposicaoService;
        private readonly IChamadaService _chamadaService;
        private readonly IDispSalaService _dispSalaService;
        private readonly ICursoService _cursoService;
        private readonly IAulaService _aulaService;
        private readonly IFeriadoService _feriadoService;

        public ReposicaoController(IReposicaoService reposicaoService,
            IChamadaService chamadaService,
            IDispSalaService dispSalaService,
            ICursoService cursoService,
            IAulaService aulaService,
            IFeriadoService feriadoService)
        {
            this._reposicaoService = reposicaoService;
            this._chamadaService = chamadaService;
            this._dispSalaService = dispSalaService;
            this._cursoService = cursoService;
            this._aulaService = aulaService;
            this._feriadoService = feriadoService;
        }

        [HttpGet]
        public IActionResult Form(int? chamadaId, int? dispSalaId)
        {
            if (chamadaId != null)
            {
                var reposicaoViewModel = new ReposicaoViewModel
                {
                    Chamada = _chamadaService.BuscarPorId(chamadaId.Value),
                    DispSalas = _dispSalaService.HorariosDisponiveis(),
                    Cursos = _cursoService.BuscarTodos()
                };

                if (dispSalaId != null)
                {
                    reposicaoViewModel.DispSala = _dispSalaService.BuscarPorId(dispSalaId.Value);
                }

                if (reposicaoViewModel.Chamada.Reposicao != null)
                {
                    reposicaoViewModel.Reposicao = reposicaoViewModel.Chamada.Reposicao;
                    reposicaoViewModel.DiaAula = reposicaoViewModel.Chamada.Aula.Data;
                }
                else
                {
                    reposicaoViewModel.DiaAula = DateTime.Now;
                }

                return View(reposicaoViewModel);
            }
            else
            {
                TempData["Error"] = "Chamada não encontrada";
                return RedirectToAction("Index", "Aluno");
            }
        }

        [HttpPost]
        public IActionResult Form(ReposicaoViewModel reposicaoViewModel)
        {
            foreach (var model in ModelState)
            {
                ModelState.Remove(model.Key);
            }
            reposicaoViewModel.DispSala = _dispSalaService.BuscarPorId(reposicaoViewModel.DispSala.Id.Value);
            reposicaoViewModel.Chamada = _chamadaService.BuscarPorId(reposicaoViewModel.Chamada.Id);
            if ((DayOfWeek)reposicaoViewModel.DispSala.Dia != reposicaoViewModel.DiaAula.DayOfWeek)
            {
                var dia = reposicaoViewModel.DispSala.Dia.GetType()
                    .GetMember(reposicaoViewModel.DispSala.Dia.ToString())
                    .First()
                    .GetCustomAttribute<DisplayAttribute>()
                    .Name;
                ModelState.AddModelError("DiaAula", "A aula deve ser em uma " + dia);
            }

            if (reposicaoViewModel.DiaAula == null)
            {
                ModelState.AddModelError("DiaAula", "Dia de aula deve ser preenchido");
            }

            reposicaoViewModel.DiaAula = reposicaoViewModel.DiaAula.AddHours((double)reposicaoViewModel.DispSala.Hora);

            var feriado = _feriadoService.BuscarPorData(reposicaoViewModel.DiaAula);

            if (feriado != null)
            {
                ModelState.AddModelError("DiaAula", "Não é possível agendar para este dia - "+feriado.Nome);
            }

            if (reposicaoViewModel.DiaAula < DateTime.Now)
            {
                ModelState.AddModelError("DiaAula", "Dia da aula não pode ser menor que hoje");
            }

            if (reposicaoViewModel.Reposicao.Motivo == null)
            {
                ModelState.AddModelError("Reposicao.Motivo", "O motivo deve ser preenchido");
            }

            if (ModelState.IsValid)
            {
                reposicaoViewModel.Reposicao.ChamadaId = reposicaoViewModel.Chamada.Id;
                reposicaoViewModel.Reposicao.DispSalaId = reposicaoViewModel.DispSala.Id.Value;
                if (reposicaoViewModel.Reposicao.Id == null)
                {
                    _reposicaoService.Cadastrar(reposicaoViewModel.Reposicao);
                }
                else
                {
                    _reposicaoService.Alterar(reposicaoViewModel.Reposicao);
                }

                var aula = _aulaService.BuscarPorDiaHora(reposicaoViewModel.DiaAula);
                if (aula == null)
                {

                    aula = new Aula()
                    {
                        CursoId = reposicaoViewModel.Chamada.PacoteCompra.Matricula.CursoId,
                        ProfessorId = reposicaoViewModel.DispSala.Professor.Id.Value,
                        SalaId = reposicaoViewModel.DispSala.Sala.Id.Value,
                        Data = reposicaoViewModel.DiaAula,
                        DataLimite = reposicaoViewModel.DiaAula.AddDays(3)
                    };
                    _aulaService.Cadastrar(aula);
                }

                var aulaAntiga = reposicaoViewModel.Chamada.Aula;
                reposicaoViewModel.Chamada.Aula = aula;
                _chamadaService.Alterar(reposicaoViewModel.Chamada);
                aulaAntiga = _aulaService.BuscarPorId(aulaAntiga.Id.Value);
                if (aulaAntiga.Chamadas.Count == 0 && aulaAntiga.Demostrativas.Count == 0)
                {
                    _aulaService.Excluir(aulaAntiga.Id.Value);
                }

                return RedirectToAction("PacoteCompra", "PacoteCompra", new { pacoteCompraId = reposicaoViewModel.Chamada.PacoteCompraId });
            }
            else
            {
                reposicaoViewModel.DispSalas = _dispSalaService.HorariosDisponiveis();
                reposicaoViewModel.Cursos = _cursoService.BuscarTodos();
                if (reposicaoViewModel.Reposicao.Id != null)
                {
                    reposicaoViewModel.Reposicao = _reposicaoService.BuscarPorId(reposicaoViewModel.Reposicao.Id.Value);
                }
                if (reposicaoViewModel.DispSala.Id != null)
                {
                    reposicaoViewModel.DispSala = _dispSalaService.BuscarPorId(reposicaoViewModel.DispSala.Id.Value);
                }
                return View(reposicaoViewModel);
            }
        }
    }
}