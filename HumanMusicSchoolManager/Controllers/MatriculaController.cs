using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.Models.ViewModels;
using HumanMusicSchoolManager.Services;
using HumanMusicSchoolManager.ServicesInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HumanMusicSchoolManager.Controllers
{
    public class MatriculaController : Controller
    {
        private readonly IMatriculaService _matriculaService;
        private readonly IAlunoService _alunoService;
        private readonly IPacoteAulaService _pacoteAulaService;
        private readonly ISalaService _salaService;
        private readonly IDispSalaService _dispSalaService;
        private readonly ICursoService _cursoService;

        public MatriculaController(IMatriculaService matriculaService,
            IAlunoService alunoService,
            IPacoteAulaService pacoteAulaService,
            ISalaService salaService,
            IDispSalaService dispSalaService,
            ICursoService cursoService)
        {
            this._matriculaService = matriculaService;
            this._alunoService = alunoService;
            this._pacoteAulaService = pacoteAulaService;
            this._salaService = salaService;
            this._dispSalaService = dispSalaService;
            this._cursoService = cursoService;
        }

        [HttpGet]
        public IActionResult Form(int? matriculaId, int? alunoId, int? dispSalaId, int? cursoId)
        {
            ViewBag.Salas = _salaService.ToString();
            if (matriculaId == null)
            {
                if (alunoId != null)
                {
                    var aluno = _alunoService.BuscarPorId(alunoId.Value);
                    var matricula = new MatriculaViewModel() {
                        Aluno = aluno,
                        DispSalas = _dispSalaService.BuscarTodos(),
                        Cursos = _cursoService.BuscarTodos()
                        
                    };
                    if (dispSalaId != null)
                    {
                        matricula.DispSala = _dispSalaService.BuscarPorId(dispSalaId.Value);
                    }
                    if (cursoId != null)
                    {
                        matricula.Curso = _cursoService.BuscarPorId(cursoId.Value);
                    }
                    return View(matricula);
                }
                else
                {
                    return RedirectToAction(controllerName: "Aluno", actionName: "Index");
                }

            }
            else
            {
                var mat = _matriculaService.BuscarPorId(matriculaId.Value);
                var matricula = new MatriculaViewModel()
                {
                    Matricula = mat,
                    Aluno = mat.Aluno,
                    DispSala = mat.DispSala,
                    DispSalas = _dispSalaService.BuscarTodos(),
                    Cursos = _cursoService.BuscarTodos()
                };
                return View(matricula);
            }
        }

        [HttpPost]
        public IActionResult Form(MatriculaViewModel matriculaViewModel)
        {
            return null;
        }
    }
}