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

        [HttpGet]
        public IActionResult Form(int? professorId)
        {
            if(professorId == null)
            {
                return View();
            }
            else
            {
                var professor = _professorService.BuscarPorId(professorId.Value);
                if (professor == null)
                {
                    return View();
                }
                else
                {
                    return View(professor);
                }
            }
        }

        [HttpPost]
        public IActionResult Form(Professor professor)
        {
            if (professor.Id == null)
            {
                if (ModelState.IsValid)
                {
                    var cpfUnico = _professorService.BuscarPorCPF(professor.CPF);
                    if (cpfUnico == null)
                    {
                        _professorService.Cadastrar(professor);

                        TempData["Success"] = "Professor Cadastrado com sucesso!";

                        return RedirectToAction("Form");
                    }
                    else
                    {
                        ModelState.AddModelError("CPF", "CPF já cadastrado para um Professor");
                        return View(professor);
                    }
                    
                }
                else
                {
                    return View(professor);
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    _professorService.Alterar(professor);

                    TempData["Success"] = "Professor Alterado com sucesso!";

                    return RedirectToAction("Form");

                }
                else
                {
                    return View(professor);
                }
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