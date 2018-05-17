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
    [Authorize(Roles = "Admin, Coordenacao, Professor")]
    public class MatriculaController : Controller
    {
        private readonly IAlunoService _alunoService;
        private readonly IMatriculaService _matriculaService;
        private readonly IProfessorService _professorService;
        private readonly ICursoService _cursoService;

        public MatriculaController(
            IAlunoService alunoService,
            IMatriculaService matriculaServices,
            IProfessorService professorService,
            ICursoService cursoService)
        {
            this._alunoService = alunoService;
            this._matriculaService = matriculaServices;
            this._professorService = professorService;
            this._cursoService = cursoService;
        }

        public IActionResult Index(int? matriculaId)
        {

            if (matriculaId == null)
            {
                return RedirectToAction(controllerName: "Aluno", actionName: "Index");
            }
            else
            {
                var matricula = _matriculaService.BuscarPorId(matriculaId.Value);

                return View(matricula);
            }
        }

        public IActionResult Form(int? alunoId, int? matriculaId)
        {

            if (matriculaId != null)
            {
                return View(_matriculaService.BuscarPorId(matriculaId.Value));
            }

            if (alunoId == null)
            {
                return RedirectToAction(controllerName: "Aluno", actionName: "Index");
            }
            else
            {
                var matricula = new Matricula()
                {
                    Aluno = _alunoService.BuscarPorId(alunoId.Value),
                    Ativo = true,
                    DataMatricula = DateTime.Now,
                };
                return View(matricula);
            }
        }

        public IActionResult Cadastrar(Matricula matricula)
        {
            if (ModelState.IsValid)
            {
                if (matricula != null)
                {
                    if (matricula.Id == null)
                    {                    
                        _matriculaService.Cadastrar(matricula);
                    }
                    else
                    {
                        _matriculaService.Alterar(matricula);
                    }
                }
                return RedirectToAction(controllerName: "Aluno", actionName: "Index");
            }
            else
            {
                if (matricula.Id != null)
                {
                    return RedirectToAction("Form", new { matriculaId = matricula.Id });
                }
                else
                {
                    return RedirectToAction("Form", new { alunoId = matricula.AlunoId});
                }
            }

        }

        public IActionResult Alterar(Matricula matricula)
        {
            if (matricula != null)
            {
                _matriculaService.Alterar(matricula);
            }

            return RedirectToAction(controllerName: "Aluno", actionName: "Index");
        }

        public JsonResult BuscarCurso()
        {

            var cursos = _cursoService.BuscarTodos().Where(c => c.Ativo == true).ToList();

            return Json(cursos); 
        }

        public IActionResult BuscarProfessor(int? cursoId)
        {
            var professores = new List<Professor>();
            if (cursoId != null)
            {
                professores = _professorService.BuscarProfessorPorCurso(cursoId.Value).Where(p => p.Ativo == true).ToList();
            }

            return Json(professores);
        }
    }
}