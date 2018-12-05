﻿using System;
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
    public class DemostrativaController : Controller
    {
        private readonly IDemostrativaService _demostrativaService;
        private readonly IChamadaService _chamadaService;
        private readonly IDispSalaService _dispSalaService;
        private readonly ICursoService _cursoService;
        private readonly IAulaService _aulaService;
        private readonly IFeriadoService _feriadoService;
        private readonly ICandidatoService _candidatoService;
        private readonly IPessoaService _pessoaService;

        public DemostrativaController(IDemostrativaService demostrativaService,
            IChamadaService chamadaService,
            IDispSalaService dispSalaService,
            ICursoService cursoService,
            IAulaService aulaService,
            IFeriadoService feriadoService,
            ICandidatoService candidatoService,
            IPessoaService pessoaService)
        {
            this._demostrativaService = demostrativaService;
            this._chamadaService = chamadaService;
            this._dispSalaService = dispSalaService;
            this._cursoService = cursoService;
            this._aulaService = aulaService;
            this._feriadoService = feriadoService;
            this._candidatoService = candidatoService;
            this._pessoaService = pessoaService;
        }

        [HttpGet]
        public IActionResult Form(int? cursoId, int? dispSalaId, int? candidatoId)
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

                demostrativaViewModel.DiaAula = DateTime.Now;

                return View(demostrativaViewModel);
            }
            else
            {
                TempData["Error"] = "Candidato não localizado";
                return RedirectToAction("Index", "Candidato");
            }
        }

        [HttpPost]
        public IActionResult Form(DemostrativaViewModel demostrativaViewModel)
        {
            foreach (var model in ModelState)
            {
                ModelState.Remove(model.Key);
            }
            demostrativaViewModel.DispSala = _dispSalaService.BuscarPorId(demostrativaViewModel.DispSala.Id.Value);
            demostrativaViewModel.Candidato = _candidatoService.BuscarPorId(demostrativaViewModel.Candidato.Id.Value);
            if ((DayOfWeek)demostrativaViewModel.DispSala.Dia != demostrativaViewModel.DiaAula.DayOfWeek)
            {
                var dia = demostrativaViewModel.DispSala.Dia.GetType()
                    .GetMember(demostrativaViewModel.DispSala.Dia.ToString())
                    .First()
                    .GetCustomAttribute<DisplayAttribute>()
                    .Name;
                ModelState.AddModelError("DiaAula", "A aula deve ser em uma " + dia);
            }

            if (demostrativaViewModel.DiaAula == null)
            {
                ModelState.AddModelError("DiaAula", "Dia de aula deve ser preenchido");
            }

            demostrativaViewModel.DiaAula = demostrativaViewModel.DiaAula.AddHours((double)demostrativaViewModel.DispSala.Hora);

            var feriado = _feriadoService.BuscarPorData(demostrativaViewModel.DiaAula);

            if (feriado != null)
            {
                ModelState.AddModelError("DiaAula", "Não é possível agendar para este dia - " + feriado.Nome);
            }

            if (demostrativaViewModel.DiaAula < DateTime.Now)
            {
                ModelState.AddModelError("DiaAula", "Dia da aula não pode ser menor que hoje");
            }


            if (ModelState.IsValid)
            {

                demostrativaViewModel.Demostrativa.DispSalaId = demostrativaViewModel.DispSala.Id.Value;
                demostrativaViewModel.Demostrativa.CursoId = demostrativaViewModel.Curso.Id.Value;
                demostrativaViewModel.Demostrativa.CandidatoId = demostrativaViewModel.Candidato.Id.Value;
                demostrativaViewModel.Demostrativa.PessoaId = _pessoaService.GetUser(User.Identity.Name).Id.Value;

                var aula = _aulaService.BuscarPorDiaHora(demostrativaViewModel.DiaAula);
                if (aula == null)
                {

                    aula = new Aula()
                    {
                        CursoId = demostrativaViewModel.Curso.Id.Value,
                        ProfessorId = demostrativaViewModel.DispSala.Professor.Id.Value,
                        SalaId = demostrativaViewModel.DispSala.Sala.Id.Value,
                        Data = demostrativaViewModel.DiaAula,
                        DataLimite = demostrativaViewModel.DiaAula.AddDays(3)
                    };
                    _aulaService.Cadastrar(aula);
                }


                demostrativaViewModel.Demostrativa.Aula = aula;
                _demostrativaService.Cadastrar(demostrativaViewModel.Demostrativa);



                return RedirectToAction("Candidato", "Candidato", new { candidatoId = demostrativaViewModel.Candidato.Id.Value });
            }
            else
            {
                demostrativaViewModel.DispSalas = _dispSalaService.HorariosDisponiveis();
                demostrativaViewModel.Cursos = _cursoService.BuscarTodos();
                demostrativaViewModel.Candidato = _candidatoService.BuscarPorId(demostrativaViewModel.Candidato.Id.Value);
                if (demostrativaViewModel.Demostrativa.Id != null)
                {
                    demostrativaViewModel.Demostrativa = _demostrativaService.BuscarPorId(demostrativaViewModel.Demostrativa.Id.Value);
                }
                if (demostrativaViewModel.DispSala.Id != null)
                {
                    demostrativaViewModel.DispSala = _dispSalaService.BuscarPorId(demostrativaViewModel.DispSala.Id.Value);
                }
                return View(demostrativaViewModel);
            }
        }

        [HttpPost]
        public IActionResult FinalizarDemostrativa(Demostrativa demostrativa, bool Contratou)
        {

            demostrativa.DispSalaId = null;
            demostrativa.Contratou = Contratou;
            var aula = _aulaService.BuscarPorId(demostrativa.AulaId.Value);
            if (aula.AulaDada == false)
            {
                demostrativa.AulaId = null;
            }
            _demostrativaService.Alterar(demostrativa);
            TempData["Success"] = "Demostrativa alterada com sucesso";
            if (demostrativa.Contratou == true)
            {
                return RedirectToAction("Form", "Aluno");
            }
            else
            {
                return RedirectToAction("Candidato", "Candidato", new { candidatoId = demostrativa.CandidatoId });
            }

        }
    }
}