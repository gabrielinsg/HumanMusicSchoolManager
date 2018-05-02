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
            var alunos = new AlunoViewModel(_alunoService.BuscarTodos(), _cursoService.BuscarTodos());
            return View(alunos);
        }

        public IActionResult Form()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar(Aluno aluno)
        {
            if (aluno != null)
            {
                _alunoService.Cadastrar(aluno);
            }

            return RedirectToAction("Index");
        }

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

        public IActionResult Alterar(Aluno aluno)
        {
            if (aluno != null)
            {
                _alunoService.Alterar(aluno);
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction(actionName: "Aluno", routeValues: new { alunoId = aluno.Id });
            }
        }
    }
}