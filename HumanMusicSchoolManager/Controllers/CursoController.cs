using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.Models.ViewModels;
using HumanMusicSchoolManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace HumanMusicSchoolManager.Controllers
{
    public class CursoController : Controller
    {
        private readonly ICursoService _cursoService;

        public CursoController(ICursoService cursoService)
        {
            this._cursoService = cursoService;
        }

        public IActionResult Index()
        {
            CursoViewModel cursos = new CursoViewModel(_cursoService.BuscarTodos());
            return View(cursos);
        }

        public IActionResult Curso(int? cursoId)
        {
            if (cursoId != null)
            {
                var curso = _cursoService.BuscarPorId(cursoId.Value);
                return View(curso);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public IActionResult Alterar(Curso curso)
        {
            _cursoService.Alterar(curso);
            return RedirectToAction("Index");
        }

        public IActionResult Cadastrar(Curso curso)
        {
            if (curso == null)
            {
                return RedirectToAction("Form");
            }
            else
            {
                _cursoService.Cadastrar(curso);
                return RedirectToAction("Index");
            }
        }

        public IActionResult Form()
        {
            return View();
        }

        public IActionResult Excluir(int? cursoId)
        {
            if (cursoId != null)
            {
                var professor = _cursoService.BuscarPorId(cursoId.Value);
                _cursoService.Excluir(professor);
            }
            return RedirectToAction("Index");
        }
    }
}