using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.Services;
using HumanMusicSchoolManager.ServicesInterface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HumanMusicSchoolManager.Controllers
{
    public class AulaController : Controller
    {
        private readonly IAulaService _aulaService;
        private readonly IProfessorService _professorService;
        private readonly ICursoService _cursoService;
        private readonly ISalaService _salaService;
        private readonly IChamadaService _chamadaService;
        private readonly IMatriculaService _matriculaService;
        private readonly IReposicaoService _reposicaoService;
        private readonly IDemostrativaService _demostrativaService;

        public AulaController(IAulaService aulaService,
            IProfessorService professorService,
            ICursoService cursoService,
            ISalaService salaService,
            IChamadaService chamadaService,
            IMatriculaService matriculaService,
            IReposicaoService reposicaoService,
            IDemostrativaService demostrativaService)
        {
            this._aulaService = aulaService;
            this._professorService = professorService;
            this._cursoService = cursoService;
            this._salaService = salaService;
            this._chamadaService = chamadaService;
            this._matriculaService = matriculaService;
            this._reposicaoService = reposicaoService;
            this._demostrativaService = demostrativaService;
        }

        [HttpGet]
        public IActionResult Form(int? aulaId)
        {
            if (aulaId != null)
            {
                var aula = _aulaService.BuscarPorId(aulaId.Value);

                if (aula != null)
                {

                    if (aula.DataLimite == null || aula.DataLimite > NowHorarioBrasilia.GetNow())
                    {
                        return View(aula);
                    }
                    else
                    {
                        TempData["Error"] = "Data limite para lançamento dessa aula expirou. Favor entrar em contato com a coordenação!";
                        return RedirectToAction("Calendario", "Professor");
                    }

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

        [HttpPost]
        public IActionResult Form(Aula aula, List<int> Estrelas)
        {

            foreach (var model in ModelState.Where(m => m.Key.Contains(".PacoteCompra.")).ToList())
            {
                ModelState.Remove(model.Key);
            }

            if (aula.DescAtividades == null)
            {
                ModelState.AddModelError("DescAtividades", "Descrição de atividades obrigatória");
            }
            else if (aula.DescAtividades.Length < 15)
            {
                ModelState.AddModelError("DescAtividades", "Descrição não atingiu o mínimo de caracteres exigidos.");
            }

            aula.AulaDada = true;
            var matriculas = new List<Matricula>();
            var pacoteCompras = new List<PacoteCompra>();

            if(aula.Chamadas != null)
            {
                foreach (var chamada in aula.Chamadas)
                {
                    var modulo = chamada.PacoteCompra.Matricula.Modulo;
                    var estrelas = chamada.PacoteCompra.Matricula.Estrelas;
                    var matricula = _matriculaService.BuscarPorId(chamada.PacoteCompra.MatriculaId);
                    chamada.PacoteCompra = null;
                    matricula.Modulo = modulo;
                    matricula.Estrelas = estrelas;
                    matriculas.Add(matricula);
                }
            }
                
            if (ModelState.IsValid)
            {
                foreach (var matricula in matriculas)
                {
                    _matriculaService.Alterar(matricula);
                }
                if (aula.Chamadas != null)
                {
                    foreach (var chamada in aula.Chamadas)
                    {
                        var reposicao = _reposicaoService.BuscarPorChamada(chamada.Id.Value);
                        _chamadaService.Alterar(chamada);
                        if (reposicao != null)
                        {
                            reposicao.DispSalaId = null;
                            _reposicaoService.Alterar(reposicao);
                        }
                    }
                }
                if (aula.Demostrativas != null)
                {
                    var i = 0;
                    foreach (var demostrativa in aula.Demostrativas)
                    {
                        demostrativa.Estrelas = Estrelas[i];
                        _demostrativaService.Alterar(demostrativa);
                        i++;
                    }
                }
               
                _aulaService.Alterar(aula);
                return RedirectToAction("Calendario", "Professor", new { professorId = aula.ProfessorId });
            }
            else
            {
                aula.Professor = _professorService.BuscarPorId(aula.ProfessorId);
                aula.Curso = _cursoService.BuscarPorId(aula.CursoId);
                aula.Sala = _salaService.BuscarPorId(aula.SalaId);
                aula.Chamadas = _aulaService.BuscarPorId(aula.Id.Value).Chamadas;
                aula.Demostrativas = _aulaService.BuscarPorId(aula.Id.Value).Demostrativas;
                return View(aula);
            }
        }

        [HttpGet]
        public IActionResult Aula(int? aulaId)
        {
            if (aulaId != null)
            {
                var aula = _aulaService.BuscarPorId(aulaId.Value);
                if (aula != null)
                {
                    var professores = _professorService.BuscarProfessorPorCurso(aula.CursoId);
                    var listProfessores = new List<SelectListItem>();
                    foreach (var professor in professores)
                    {
                        var selectListItem = new SelectListItem
                        {
                            Value = professor.Id.ToString(),
                            Text = professor.Nome
                        };
                        listProfessores.Add(selectListItem);
                    }
                    ViewBag.professores = listProfessores;

                    var salas = _salaService.BuscarTodos();
                    var listSalas = new List<SelectListItem>();
                    foreach (var sala in salas)
                    {
                        var selectListItem = new SelectListItem
                        {
                            Value = sala.Id.ToString(),
                            Text = sala.Nome
                        };
                        listSalas.Add(selectListItem);
                    }
                    ViewBag.salas = listSalas;

                    return View(aula);
                }
            }

            return RedirectToAction("Index", "Aluno");
        }

        [HttpPost]
        public IActionResult Aula(Aula aula, int? Hora)
        {

            if (Hora == null)
            {
                ModelState.AddModelError("Hora", "Hora deve ser preenchida!");
            }

            if (ModelState.IsValid)
            {
                aula.Data.AddHours(-aula.Data.Hour);
                aula.Data.AddHours(Hora.Value);
                aula.DataLimite.AddHours(23);

                foreach (var chamada in aula.Chamadas)
                {
                    if (aula.AulaDada == false)
                    {
                        chamada.Presenca = null;
                    }
                    _chamadaService.Alterar(chamada);
                }

                _aulaService.Alterar(aula);
                TempData["Success"] = "Aula alterado com sucesso";
                return RedirectToAction("Index", "Aula");
            }
            else
            {
                var professores = _professorService.BuscarProfessorPorCurso(aula.CursoId);
                var listProfessores = new List<SelectListItem>();
                foreach (var professor in professores)
                {
                    var selectListItem = new SelectListItem
                    {
                        Value = professor.Id.ToString(),
                        Text = professor.Nome
                    };
                    listProfessores.Add(selectListItem);
                }

                var salas = _salaService.BuscarTodos();
                var listSalas = new List<SelectListItem>();
                foreach (var sala in salas)
                {
                    var selectListItem = new SelectListItem
                    {
                        Value = sala.Id.ToString(),
                        Text = sala.Nome
                    };
                    listSalas.Add(selectListItem);
                }
                ViewBag.salas = listSalas;

                ViewBag.professores = listProfessores;
                return View(aula);
            }
        }
    }
}