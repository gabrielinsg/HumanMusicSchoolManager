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
    [Authorize(Roles = "Admin")]
    public class FuncionarioController : Controller
    {
        private readonly IFuncionarioService _funcionarioService;

        public FuncionarioController(IFuncionarioService funcionarioService)
        {
            this._funcionarioService = funcionarioService;
        }

        public IActionResult Index()
        {
            return View(new FuncionarioViewModel(_funcionarioService.BuscarTodos()));
        }

        public IActionResult Form()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar(Funcionario funcionario)
        {
            if (!ModelState.IsValid)
            {
                return View("Form", funcionario);
            }
            else
            {
                funcionario.Ativo = true;
                _funcionarioService.Cadastrar(funcionario);
                return RedirectToAction("Index");
            }
        }

        public IActionResult Excluir(int? funcionarioId)
        {
            if (funcionarioId != null)
            {
                _funcionarioService.Excluir(funcionarioId.Value);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Funcionario(int? funcionarioId)
        {
            if (funcionarioId != null)
            {
                var funcionario = _funcionarioService.BuscarPorId(funcionarioId.Value);
                return View(funcionario);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Alterar(Funcionario funcionario)
        {
            _funcionarioService.Alterar(funcionario);
            return RedirectToAction("Index");
        }
    }
}