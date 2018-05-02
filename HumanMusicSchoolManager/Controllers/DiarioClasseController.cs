using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanMusicSchoolManager.Models;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.Models.ViewModels;
using HumanMusicSchoolManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HumanMusicSchoolManager.Controllers
{
    [Authorize(Roles = "Admin, Coordenacao, Professor")]
    public class DiarioClasseController : Controller
    {
        private readonly IDiarioClasseService _diarioClasseService;
        private readonly IMatriculaService _matriculaService;
        private readonly IProfessorService _professorService;
        private readonly UserManager<ApplicationUser> _userManager;

        public DiarioClasseController(IDiarioClasseService diarioClasseService,
            IMatriculaService matriculaService,
            IProfessorService professorService,
            UserManager<ApplicationUser> userManager)
        {
            _diarioClasseService = diarioClasseService;
            _matriculaService = matriculaService;
            _professorService = professorService;
            _userManager = userManager;
        }


        public IActionResult Index(int? professorId)
        {

            if (professorId == null)
            {
                professorId = _userManager.Users.Where(u => u.UserName == _userManager.GetUserName(User)).FirstOrDefault().PessoaId;
            }

            var professor = _professorService.BuscarPorId(professorId.Value);
            ViewBag.Professor = professor;
            return View(new MatriculaViewModel(_matriculaService.BuscarPorProfessor(professorId.Value).Where(m => m.Ativo == true).ToList()));
        }

        public IActionResult Form(int? matriculaId, int? diarioId)
        {

            if (diarioId != null)
            {
                var diario = _diarioClasseService.BuscarPorId(diarioId.Value);
                ViewBag.Historico = _diarioClasseService.BuscarAlguns(diario.MatriculaId);
                return View(diario);
            }

            if (matriculaId != null)
            {
                var diario = new DiarioClasse()
                {
                    Matricula = _matriculaService.BuscarPorId(matriculaId.Value),
                    Data = DateTime.Now,
                    Presenca = true
                };
                ViewBag.Historico = _diarioClasseService.BuscarAlguns(matriculaId.Value);
                return View(diario);
            }
            else
            {
                return RedirectToAction(controllerName: "home", actionName: "index");
            }
        }

        public IActionResult Cadastrar(DiarioClasse diario)
        {
            if (diario.Id == null)
            {
                if (ModelState.IsValid)
                {
                    _diarioClasseService.Cadastrar(diario);
                    return RedirectToAction("index", routeValues: new { professorId = _matriculaService.BuscarPorId(diario.MatriculaId).ProfessorId });
                }
                else
                {
                    diario = new DiarioClasse()
                    {
                        Matricula = _matriculaService.BuscarPorId(diario.MatriculaId),
                        Data = DateTime.Now,
                        Presenca = true
                    };
                    ViewBag.Historico = _diarioClasseService.BuscarAlguns(diario.Matricula.Id.Value);
                    return View("form", diario);
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    _diarioClasseService.Alterar(diario);
                    return RedirectToAction("index", routeValues: new { professorId = _matriculaService.BuscarPorId(diario.MatriculaId).ProfessorId });
                }
                else
                {
                    diario.Matricula = _matriculaService.BuscarPorId(diario.MatriculaId);
                    ViewBag.Historico = _diarioClasseService.BuscarAlguns(diario.MatriculaId);
                    return View("form", diario);
                }
            }

        }

        public IActionResult Historico(int? matriculaId)
        {
            if (matriculaId != null)
            {
                ViewBag.Matricula = _matriculaService.BuscarPorId(matriculaId.Value);
                return View(new DiarioClasseViewModel(_diarioClasseService.BuscarPorMatricula(matriculaId.Value)));
            }
            else
            {
                return RedirectToAction(controllerName: "home", actionName: "index");
            }
        }

        public IActionResult Excluir(int? diarioId)
        {
            if (diarioId != null)
            {
                var professorId = _diarioClasseService.BuscarPorId(diarioId.Value).Matricula.Professor.Id;
                _diarioClasseService.Excluir(diarioId.Value);
                return RedirectToAction("index", new { professorId = professorId });
            }
            else
            {
                return RedirectToAction(controllerName: "home", actionName: "index");
            }
        }
    }
}