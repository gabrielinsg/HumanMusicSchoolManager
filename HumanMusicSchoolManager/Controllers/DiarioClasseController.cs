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
            return View(new MatriculaViewModel(_matriculaService.BuscarPorProfessor(professorId.Value).Where(m => m.Ativo == true).ToList(), _diarioClasseService));
        }

        public IActionResult Form(int? matriculaId, int? diarioId)
        {

            if (diarioId != null)
            {
                var diario = _diarioClasseService.BuscarPorId(diarioId.Value);
                ViewBag.Historico = _diarioClasseService.BuscarAlguns(diario.Matricula);
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
                ViewBag.Historico = _diarioClasseService.BuscarAlguns(diario.Matricula);
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
                    //return RedirectToAction("index", routeValues: new { professorId = _matriculaService.BuscarPorId(diario.MatriculaId).ProfessorId });
                    return RedirectToAction("form", new { matriculaId = diario.MatriculaId});
                }
                else
                {
                    diario = new DiarioClasse()
                    {
                        Matricula = _matriculaService.BuscarPorId(diario.MatriculaId),
                        Data = DateTime.Now,
                        Presenca = true
                    };
                    ViewBag.Historico = _diarioClasseService.BuscarAlguns(diario.Matricula);
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
                    ViewBag.Historico = _diarioClasseService.BuscarAlguns(diario.Matricula);
                    return View("form", diario);
                }
            }

        }

        public IActionResult Historico(int? matriculaId)
        {
            if (matriculaId != null)
            {
                var matricula = _matriculaService.BuscarPorId(matriculaId.Value);
                ViewBag.Matricula = matricula;
                return View(new DiarioClasseViewModel(_diarioClasseService.BuscarPorAluno(matricula)));
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
                var matriculaId = _diarioClasseService.BuscarMatricula(diarioId.Value).Id;
                _diarioClasseService.Excluir(diarioId.Value);
                return RedirectToAction("historico", new { matriculaId = matriculaId });
            }
            else
            {
                return RedirectToAction(controllerName: "home", actionName: "index");
            }
        }
    }
}