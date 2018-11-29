using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using HumanMusicSchoolManager.Models;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.Models.ViewModels;
using HumanMusicSchoolManager.Services;
using HumanMusicSchoolManager.ServicesInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HumanMusicSchoolManager.Controllers
{
    public class MatriculaController : Controller
    {
        private readonly IMatriculaService _matriculaService;
        private readonly IAlunoService _alunoService;
        private readonly ISalaService _salaService;
        private readonly IDispSalaService _dispSalaService;
        private readonly ICursoService _cursoService;
        private readonly IRespFinanceiroService _respFinanceiroService;
        private readonly ITaxaMatriculaService _taxaMatriculaService;
        private readonly IPessoaService _pessoaService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFinanceiroService _financeiroService;

        public MatriculaController(IMatriculaService matriculaService,
            IAlunoService alunoService,
            ISalaService salaService,
            IDispSalaService dispSalaService,
            ICursoService cursoService,
            IRespFinanceiroService respFinanceiroService,
            ITaxaMatriculaService taxaMatriculaService,
            IPessoaService pessoaService,
            IFinanceiroService financeiroService,
            UserManager<ApplicationUser> userManager)
        {
            this._matriculaService = matriculaService;
            this._alunoService = alunoService;
            this._salaService = salaService;
            this._dispSalaService = dispSalaService;
            this._cursoService = cursoService;
            this._respFinanceiroService = respFinanceiroService;
            this._taxaMatriculaService = taxaMatriculaService;
            this._pessoaService = pessoaService;
            this._userManager = userManager;
            this._financeiroService = financeiroService;
        }

        [HttpGet]
        public IActionResult Form(int? matriculaId, int? alunoId, int? dispSalaId, int? cursoId, int? respFinanceiroId, string collapse)
        {
            ViewBag.Collapse = collapse;
            if (matriculaId == null)
            {
                if (alunoId != null)
                {
                    var aluno = _alunoService.BuscarPorId(alunoId.Value);
                    var matricula = new MatriculaViewModel()
                    {
                        Aluno = aluno,
                        DispSalas = _dispSalaService.HorariosDisponiveis(),
                        Cursos = _cursoService.BuscarTodos(),
                        TaxasMatricula = _taxaMatriculaService.BuscarTodos()
                    };
                    if (dispSalaId != null)
                    {
                        matricula.DispSala = _dispSalaService.BuscarPorId(dispSalaId.Value);
                    }
                    if (cursoId != null)
                    {
                        matricula.Curso = _cursoService.BuscarPorId(cursoId.Value);
                    }
                    if (respFinanceiroId != null)
                    {
                        matricula.RespFinanceiro = _respFinanceiroService.BuscarPorId(respFinanceiroId.Value);
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
            foreach (var model in ModelState.Where(m => !m.Key.StartsWith("Financeiro") || m.Key.EndsWith("Pessoa") || m.Key.EndsWith("Aluno")).ToList())
            {
                ModelState.Remove(model.Key);
            }

            var pessoa = _pessoaService.GetUser(User.Identity.Name);

            if(pessoa == null)
            {
                ModelState.AddModelError("Pessoa", "Não existe pessoa para a matricula");
            }
            else
            {
                foreach (var financeiro in matriculaViewModel.Financeiros)
                {
                    financeiro.Pessoa = pessoa;
                }
            }

            foreach (var financeiro in matriculaViewModel.Financeiros)
            {
                financeiro.UltimaAlteracao = DateTime.Now;
            }

            if (matriculaViewModel.Aluno.Id == null)
            {
                ModelState.AddModelError("Aluno", "Aluno não selecionado!");
            }
            else
            {
                matriculaViewModel.Aluno = _alunoService.BuscarPorId(matriculaViewModel.Aluno.Id.Value);
                matriculaViewModel.Matricula.Aluno = _alunoService.BuscarPorId(matriculaViewModel.Aluno.Id.Value);
                foreach (var financeiro in matriculaViewModel.Financeiros)
                {
                    financeiro.Aluno = _alunoService.BuscarPorId(matriculaViewModel.Aluno.Id.Value);
                }
            }

            if (matriculaViewModel.Curso.Id == null)
            {
                ModelState.AddModelError("Curso", "Curso deve ser selecionado!");
            }
            else
            {
                matriculaViewModel.Curso = _cursoService.BuscarPorId(matriculaViewModel.Curso.Id.Value);
                matriculaViewModel.Matricula.Curso = _cursoService.BuscarPorId(matriculaViewModel.Curso.Id.Value);
            }

            if (matriculaViewModel.DispSala.Id == null)
            {
                ModelState.AddModelError("Horário", "Um horário deve ser selecionado!");
            }
            else
            {
                matriculaViewModel.DispSala = _dispSalaService.BuscarPorId(matriculaViewModel.DispSala.Id.Value);
                matriculaViewModel.Matricula.DispSala = _dispSalaService.BuscarPorId(matriculaViewModel.DispSala.Id.Value);
            }

            if (matriculaViewModel.RespFinanceiro.Id == null)
            {
                ModelState.AddModelError("RespFinanceiro", "Um Responsável financeiro deve ser selecionado!");
            }
            else
            {
                matriculaViewModel.RespFinanceiro = _respFinanceiroService.BuscarPorId(matriculaViewModel.RespFinanceiro.Id.Value);
                matriculaViewModel.Matricula.RespFinanceiro = _respFinanceiroService.BuscarPorId(matriculaViewModel.RespFinanceiro.Id.Value);
            }

            matriculaViewModel.TaxasMatricula = _taxaMatriculaService.BuscarTodos();
            matriculaViewModel.DispSalas = _dispSalaService.BuscarTodos();
            matriculaViewModel.Cursos = _cursoService.BuscarTodos();

            if (ModelState.IsValid)
            {
                matriculaViewModel.Matricula.Ativo = true;
                matriculaViewModel.Matricula.DataMatricula = DateTime.Now;
                _matriculaService.Cadastrar(matriculaViewModel.Matricula);
                foreach (var financeiro in matriculaViewModel.Financeiros)
                {
                    _financeiroService.Cadastrar(financeiro);
                }
                return RedirectToAction("Aluno", "Aluno", new { alunoId = matriculaViewModel.Aluno.Id });
            }
            else
            {
                return View(matriculaViewModel);
            }
        }

        [HttpPost]
        public JsonResult BuscarPorNome(string nome)
        {
            var respFinanceiro = _respFinanceiroService.BuscarPorNome(nome);
            return Json(respFinanceiro);
        }

        [HttpPost]
        public IActionResult CadastraRespFinanceiro(MatriculaViewModel matriculaViewModel)
        {
            ViewBag.Collapse = "";
            if (matriculaViewModel.RespFinanceiro.Id == null)
            {
                foreach (var model in ModelState.Where(m => !m.Key.StartsWith("RespFinanceiro")).ToList())
                {
                    ModelState.Remove(model.Key);
                }
                if (ModelState.IsValid)
                {
                    matriculaViewModel.Aluno = _alunoService.BuscarPorId(matriculaViewModel.Aluno.Id.Value);
                    matriculaViewModel.DispSalas = _dispSalaService.HorariosDisponiveis();
                    matriculaViewModel.Cursos = _cursoService.BuscarTodos();
                    matriculaViewModel.TaxasMatricula = _taxaMatriculaService.BuscarTodos();
                    if (matriculaViewModel.DispSala.Id != null)
                    {
                        matriculaViewModel.DispSala = _dispSalaService.BuscarPorId(matriculaViewModel.DispSala.Id.Value);
                    }
                    if (matriculaViewModel.Curso.Id != null)
                    {
                        matriculaViewModel.Curso = _cursoService.BuscarPorId(matriculaViewModel.Curso.Id.Value);
                    }
                    _respFinanceiroService.Cadastrar(matriculaViewModel.RespFinanceiro);
                    return View("Form", matriculaViewModel);
                }
                else
                {
                    ViewBag.Collapse = "respFinanceiro";
                    matriculaViewModel.Aluno = _alunoService.BuscarPorId(matriculaViewModel.Aluno.Id.Value);
                    matriculaViewModel.DispSalas = _dispSalaService.HorariosDisponiveis();
                    matriculaViewModel.Cursos = _cursoService.BuscarTodos();
                    matriculaViewModel.TaxasMatricula = _taxaMatriculaService.BuscarTodos();
                    if (matriculaViewModel.DispSala.Id != null)
                    {
                        matriculaViewModel.DispSala = _dispSalaService.BuscarPorId(matriculaViewModel.DispSala.Id.Value);
                    }
                    if (matriculaViewModel.Curso.Id != null)
                    {
                        matriculaViewModel.Curso = _cursoService.BuscarPorId(matriculaViewModel.Curso.Id.Value);
                    }
                    return View("Form", matriculaViewModel);
                }
            }
            else
            {
                foreach (var model in ModelState.Where(m => !m.Key.StartsWith("RespFinanceiro")).ToList())
                {
                    ModelState.Remove(model.Key);
                }
                if (ModelState.IsValid)
                {
                    matriculaViewModel.Aluno = _alunoService.BuscarPorId(matriculaViewModel.Aluno.Id.Value);
                    matriculaViewModel.DispSalas = _dispSalaService.HorariosDisponiveis();
                    matriculaViewModel.Cursos = _cursoService.BuscarTodos();
                    matriculaViewModel.TaxasMatricula = _taxaMatriculaService.BuscarTodos();
                    if (matriculaViewModel.DispSala.Id != null)
                    {
                        matriculaViewModel.DispSala = _dispSalaService.BuscarPorId(matriculaViewModel.DispSala.Id.Value);
                    }
                    if (matriculaViewModel.Curso.Id != null)
                    {
                        matriculaViewModel.Curso = _cursoService.BuscarPorId(matriculaViewModel.Curso.Id.Value);
                    }
                    _respFinanceiroService.Alterar(matriculaViewModel.RespFinanceiro);
                    return View("Form", matriculaViewModel);
                }
                else
                {
                    ViewBag.Collapse = "respFinanceiro";
                    matriculaViewModel.Aluno = _alunoService.BuscarPorId(matriculaViewModel.Aluno.Id.Value);
                    matriculaViewModel.DispSalas = _dispSalaService.HorariosDisponiveis();
                    matriculaViewModel.Cursos = _cursoService.BuscarTodos();
                    matriculaViewModel.TaxasMatricula = _taxaMatriculaService.BuscarTodos();
                    if (matriculaViewModel.DispSala.Id != null)
                    {
                        matriculaViewModel.DispSala = _dispSalaService.BuscarPorId(matriculaViewModel.DispSala.Id.Value);
                    }
                    if (matriculaViewModel.Curso.Id != null)
                    {
                        matriculaViewModel.Curso = _cursoService.BuscarPorId(matriculaViewModel.Curso.Id.Value);
                    }
                    return View("Form", matriculaViewModel);
                }
            }
        }

        [HttpPost]
        public JsonResult BuscarAlunoPorId(int alunoId)
        {
            var aluno = _alunoService.BuscarPorId(alunoId);
            return Json(alunoId);
        }

        [HttpPost]
        public JsonResult BuscarAlunoRespFinanceiro(int alunoId)
        {
            var aluno = _alunoService.BuscarPorId(alunoId);
            var respFinanceiro = _respFinanceiroService.BuscarPorCPF(aluno.CPF);
            if (respFinanceiro != null)
            {
                return Json(respFinanceiro);
            }
            else
            {
                aluno.Id = null;
                return Json(aluno);
            }
        }

        [HttpPost]
        public JsonResult BuscarRespFinanceiro(int respFinanceiroId)
        {
            return Json(_respFinanceiroService.BuscarPorId(respFinanceiroId));
        }
    }
}