using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanMusicSchoolManager.Models.ViewModels;
using HumanMusicSchoolManager.Services;
using HumanMusicSchoolManager.ServicesInterface;
using Microsoft.AspNetCore.Mvc;

namespace HumanMusicSchoolManager.Controllers
{
    public class DemostrativaController : Controller
    {
        private readonly IDemostrativaService _demostrativaService;
        private readonly IChamadaService _chamadaService;
        private readonly IDispSalaService _dispSalaService;
        private readonly ICursoService _cursoService;
        private readonly IAulaService _aulaService;
        private readonly IFeriadoService _feriadoService;
        private readonly ICandidatoService _candidatoService;

        public DemostrativaController(IDemostrativaService demostrativaService,
            IChamadaService chamadaService,
            IDispSalaService dispSalaService,
            ICursoService cursoService,
            IAulaService aulaService,
            IFeriadoService feriadoService,
            ICandidatoService candidatoService)
        {
            this._chamadaService = chamadaService;
            this._dispSalaService = dispSalaService;
            this._cursoService = cursoService;
            this._aulaService = aulaService;
            this._feriadoService = feriadoService;
            this._candidatoService = candidatoService;
        }

        [HttpGet]
        public IActionResult Form(int? demostrativaId, int? cursoId, int? dispSalaId, int? candidatoId)
        {
            if (candidatoId != null)
            {
                var demostrativaViewModel = new DemostrativaViewModel
                {
                    Candidato = _candidatoService.BuscarPorId(candidatoId.Value),
                    Cursos = _cursoService.BuscarTodos(),
                    DispSalas = _dispSalaService.HorariosDisponiveis()
                };

                if (dispSalaId != null)
                {
                    demostrativaViewModel.DispSala = _dispSalaService.BuscarPorId(dispSalaId.Value);
                }
                if (cursoId != null)
                {
                    demostrativaViewModel.Curso = _cursoService.BuscarPorId(cursoId.Value);
                }
                if (demostrativaId != null)
                {
                    demostrativaViewModel.Demostrativa = _demostrativaService.BuscarPorId(demostrativaId.Value);
                }

                return View(demostrativaViewModel);
            }
            else
            {
                TempData["Error"] = "Candidato não localizado";
                return RedirectToAction("Index", "Candidato");
            }
        }

        //[HttpPost]
        //public IActionResult Form(DemostrativaViewModel demostrativaViewModel)
        //{
        //    foreach (var model in ModelState)
        //    {
        //        ModelState.Remove(model.Key);
        //    }
        //    demostrativaViewModel.DispSala = _dispSalaService.BuscarPorId(demostrativaViewModel.DispSala.Id.Value);
        //    demostrativaViewModel.Chamada = _chamadaService.BuscarPorId(demostrativaViewModel.Chamada.Id);
        //    if ((DayOfWeek)demostrativaViewModel.DispSala.Dia != demostrativaViewModel.DiaAula.DayOfWeek)
        //    {
        //        var dia = demostrativaViewModel.DispSala.Dia.GetType()
        //            .GetMember(demostrativaViewModel.DispSala.Dia.ToString())
        //            .First()
        //            .GetCustomAttribute<DisplayAttribute>()
        //            .Name;
        //        ModelState.AddModelError("DiaAula", "A aula deve ser em uma " + dia);
        //    }

        //    if (demostrativaViewModel.DiaAula == null)
        //    {
        //        ModelState.AddModelError("DiaAula", "Dia de aula deve ser preenchido");
        //    }

        //    demostrativaViewModel.DiaAula = demostrativaViewModel.DiaAula.AddHours((double)demostrativaViewModel.DispSala.Hora);

        //    var feriado = _feriadoService.BuscarPorData(demostrativaViewModel.DiaAula);

        //    if (feriado != null)
        //    {
        //        ModelState.AddModelError("DiaAula", "Não é possível agendar para este dia - " + feriado.Nome);
        //    }

        //    if (demostrativaViewModel.DiaAula < DateTime.Now)
        //    {
        //        ModelState.AddModelError("DiaAula", "Dia da aula não pode ser menor que hoje");
        //    }

        //    if (demostrativaViewModel.Reposicao.Motivo == null)
        //    {
        //        ModelState.AddModelError("Reposicao.Motivo", "O motivo deve ser preenchido");
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        demostrativaViewModel.Reposicao.ChamadaId = demostrativaViewModel.Chamada.Id;
        //        demostrativaViewModel.Reposicao.DispSalaId = demostrativaViewModel.DispSala.Id.Value;
        //        if (demostrativaViewModel.Reposicao.Id == null)
        //        {
        //            _reposicaoService.Cadastrar(demostrativaViewModel.Reposicao);
        //        }
        //        else
        //        {
        //            _reposicaoService.Alterar(demostrativaViewModel.Reposicao);
        //        }

        //        var aula = _aulaService.BuscarPorDiaHora(demostrativaViewModel.DiaAula);
        //        if (aula == null)
        //        {

        //            aula = new Aula()
        //            {
        //                CursoId = demostrativaViewModel.Chamada.PacoteCompra.Matricula.CursoId,
        //                ProfessorId = demostrativaViewModel.DispSala.Professor.Id.Value,
        //                SalaId = demostrativaViewModel.DispSala.Sala.Id.Value,
        //                Data = demostrativaViewModel.DiaAula,
        //                DataLimite = demostrativaViewModel.DiaAula.AddDays(3)
        //            };
        //            _aulaService.Cadastrar(aula);
        //        }

        //        var aulaAntiga = demostrativaViewModel.Chamada.Aula;
        //        demostrativaViewModel.Chamada.Aula = aula;
        //        _chamadaService.Alterar(demostrativaViewModel.Chamada);
        //        aulaAntiga = _aulaService.BuscarPorId(aulaAntiga.Id);
        //        if (aulaAntiga.Chamadas.Count == 0)
        //        {
        //            _aulaService.Excluir(aulaAntiga.Id);
        //        }

        //        return RedirectToAction("PacoteCompra", "PacoteCompra", new { pacoteCompraId = demostrativaViewModel.Chamada.PacoteCompraId });
        //    }
        //    else
        //    {
        //        demostrativaViewModel.DispSalas = _dispSalaService.HorariosDisponiveis();
        //        demostrativaViewModel.Cursos = _cursoService.BuscarTodos();
        //        if (demostrativaViewModel.Reposicao.Id != null)
        //        {
        //            demostrativaViewModel.Reposicao = _reposicaoService.BuscarPorId(demostrativaViewModel.Reposicao.Id.Value);
        //        }
        //        if (demostrativaViewModel.DispSala.Id != null)
        //        {
        //            demostrativaViewModel.DispSala = _dispSalaService.BuscarPorId(demostrativaViewModel.DispSala.Id.Value);
        //        }
        //        return View(demostrativaViewModel);
        //    }
        //}
    }
}