using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    [Authorize(Roles = "Admin, Coordenacao, Professor")]
    public class AlunoController : Controller
    {
        private readonly IAlunoService _alunoService;
        private readonly ICursoService _cursoService;

        public AlunoController(IAlunoService alunoService, ICursoService cursoService)
        {
            this._alunoService = alunoService;
            this._cursoService = cursoService;
        }

        public IActionResult Index()
        {
            return View(_alunoService.BuscarTodos());
        }

        [HttpGet]
        public IActionResult Form(int? alunoId)
        {
            if (alunoId == null)
            {
                return View();
            }
            else
            {
                var aluno = _alunoService.BuscarPorId(alunoId.Value);
                if (aluno != null)
                {
                    return View(aluno);
                }
                else
                {
                    return View();
                }
            }
        }

        [HttpPost]
        public IActionResult Form(Aluno aluno)
        {
            if (aluno.Id == null)
            {
                if (ModelState.IsValid)
                {
                    var cpfUnico = _alunoService.BuscarPorCPF(aluno.CPF);
                    if (cpfUnico == null)
                    {
                        _alunoService.Cadastrar(aluno);
                        TempData["Success"] = "Aluno cadastrado com sucesso!";
                        return RedirectToAction("Form");
                    }
                    else
                    {
                        ModelState.AddModelError("CPF", "CPF já cadastrado para outro aluno");
                        return View(aluno);
                    }
                }
                else
                {
                    return View(aluno);
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    _alunoService.Alterar(aluno);
                    TempData["Success"] = "Aluno alterado com sucesso!";
                    return RedirectToAction("Form");
                }
                else
                {
                    return View(aluno);
                }
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Excluir(int? alunoId)
        {
            if (alunoId != null)
            {
                _alunoService.Excluir(alunoId.Value);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Aluno(int? alunoId)
        {
            if (alunoId == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(_alunoService.BuscarPorId(alunoId.Value));
            }
        }

        [HttpPost]
        public JsonResult BuscarPorNome(string nome)
        {
            var aluno = _alunoService.BuscarPorNome(nome);
            return Json(aluno);
        }
    }
}