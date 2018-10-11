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
    [Authorize(Roles = "Admin, Coordenacao")]
    public class CursoController : Controller
    {
        private readonly ICursoService _cursoService;

        public CursoController(ICursoService cursoService)
        {
            this._cursoService = cursoService;
        }

        public IActionResult Index()
        {
            return View(_cursoService.BuscarTodos());
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

        [HttpGet]
        public IActionResult Form(int? cursoId)
        {
            if (cursoId == null)
            {
                return View();
            }
            else
            {
                var curso = _cursoService.BuscarPorId(cursoId.Value);
                if (curso != null)
                {
                    return View(curso);
                }
                else
                {
                    return View();
                }
            }
        }

        [HttpPost]
        public IActionResult Form(Curso curso)
        {
            if (curso.Id == null)
            {
                if (ModelState.IsValid)
                {
                    _cursoService.Cadastrar(curso);
                    TempData["Success"] = "Curso cadastrado com sucesso!";
                    return RedirectToAction("Form");
                }
                else
                {
                    return View(curso);
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    _cursoService.Alterar(curso);
                    TempData["Success"] = "Curso alterado com sucesso!";
                    return RedirectToAction("Form");
                }
                else
                {
                    return View(curso);
                }
            }
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

        [HttpPost]
        public JsonResult BuscarPorNome(string nome)
        {
            var curso = _cursoService.BuscarPorNome(nome);
            return Json(curso);
        }

        [HttpPost]
        public JsonResult BuscarPorId(int id)
        {
            var curso = _cursoService.BuscarPorId(id);
            return Json(curso);
        }
    }
}