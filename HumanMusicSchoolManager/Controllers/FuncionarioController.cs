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
            return View(_funcionarioService.BuscarTodos());
        }

        [HttpGet]
        public IActionResult Form(int? funcionarioId)
        {
            if (funcionarioId == null)
            {
                return View();
            }
            else
            {
                var funcionario = _funcionarioService.BuscarPorId(funcionarioId.Value);
                if (funcionario != null)
                {
                    return View(funcionario);
                }
                else
                {
                    return View();
                }
            }
        }

        [HttpPost]
        public IActionResult Form(Funcionario funcionario)
        {
            if (funcionario.Id == null)
            {
                if (ModelState.IsValid)
                {
                    var cpfUnico = _funcionarioService.BuscarPorCPF(funcionario.CPF);
                    if (cpfUnico == null)
                    {
                        _funcionarioService.Cadastrar(funcionario);

                        TempData["Success"] = "Funcionário Cadastrado com sucesso!";

                        return RedirectToAction("Form");
                    }
                    else
                    {
                        ModelState.AddModelError("CPF", "CPF já cadastrado para outro funcionário");
                        return View(funcionario);
                    }

                }
                else
                {
                    return View(funcionario);
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    _funcionarioService.Alterar(funcionario);

                    TempData["Success"] = "Funcionário Alterado com sucesso!";

                    return RedirectToAction("Form");

                }
                else
                {
                    return View(funcionario);
                }
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
        public JsonResult BuscarPorNome(string nome)
        {
            var funcionario = _funcionarioService.BuscarPorNome(nome);
            return Json(funcionario);
        }
    }
}