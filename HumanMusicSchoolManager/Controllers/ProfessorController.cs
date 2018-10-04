using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.Models.ViewModels;
using HumanMusicSchoolManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HumanMusicSchoolManager.Controllers
{
    [Authorize(Roles = "Admin, Secretaria")]
    public class ProfessorController : Controller
    {
        private readonly IProfessorService _professorService;
        private readonly ICursoService _cursoService;

        public ProfessorController(IProfessorService professorService, ICursoService cursoService)
        {
            this._professorService = professorService;
            this._cursoService = cursoService;
        }

        [Authorize(Roles = "Coordenacao, Atendimento, Diretoria, Secretaria, Admin")]
        public IActionResult Index()
        {
            ProfessorViewModel professores = new ProfessorViewModel(_professorService.BuscarTodos(), _cursoService.BuscarTodos());
            return View(professores);
        }

        public IActionResult Form()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar(Professor professor)
        {
            if (professor == null)
            {
                return RedirectToAction("Form");
            }
            else
            {
                professor.Ativo = true;
                _professorService.Cadastrar(professor);
                return RedirectToAction("Index");
            }
        }

        public IActionResult Excluir(int? professorId)
        {
            if (professorId != null)
            {
                var professor = _professorService.BuscarPorId(professorId.Value);
                _professorService.Excluir(professor);
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Coordenacao, Atendimento, Diretoria, Secretaria, Admin")]
        public IActionResult Professor(int? professorId)
        {
            if (professorId != null)
            {
                var professor = _professorService.BuscarPorId(professorId.Value);
                return View(professor);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Alterar(Professor professor)
        {
            _professorService.Alterar(professor);
            return RedirectToAction("Index");
        }

        public IActionResult ProfessorCurso(int? professorId)
        {
            if (professorId != null)
            {
                var professor = new ProfessorCursoViewModel(_professorService.BuscarPorId(professorId.Value), _cursoService.BuscarTodos().Where(c => c.Ativo == true).ToList());
                return View(professor);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public IActionResult AdicionarCurso(int? cursoId, int? professorId)
        {

            if (professorId != null && cursoId != null)
            {
                _professorService.AdicionarCurso(professorId.Value, cursoId.Value);
            }

            return RedirectToAction(actionName: "ProfessorCurso", routeValues: new { professorId = professorId });

        }

        public IActionResult RemoverCurso(int? cursoId, int? professorId)
        {

            if (professorId != null && cursoId != null)
            {
                _professorService.RemoverCurso(professorId.Value, cursoId.Value);
            }

            return RedirectToAction(actionName: "ProfessorCurso", routeValues: new { professorId = professorId });

        }
    }
}