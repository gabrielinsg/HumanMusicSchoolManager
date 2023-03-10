using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.Models.ViewModels;
using HumanMusicSchoolManager.ServicesInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HumanMusicSchoolManager.Controllers
{
    [Authorize(Roles = "Admin, Vendas, Atendimento")]
    public class CandidatoController : Controller
    {
        private readonly ICandidatoService _candidatoService;

        public CandidatoController(ICandidatoService candidatoService)
        {
            this._candidatoService = candidatoService;
        }

        public IActionResult Index()
        {
            return View(_candidatoService.BuscarTodos());
        }

        [HttpGet]
        public IActionResult Form(int? candidatoId)
        {
            if (candidatoId == null)
            {
                return View();
            }
            else
            {
                var candidato = _candidatoService.BuscarPorId(candidatoId.Value);
                if (candidato != null)
                {
                    return View(candidato);
                }
                else
                {
                    TempData["Error"] = "Aluno não encontrato!";
                    return RedirectToAction("Index");
                }
            }
        }

        [HttpPost]
        public IActionResult Form(Candidato candidato)
        {
            if (candidato.CPF == null)
            {
                ModelState.Remove(ModelState.Keys.FirstOrDefault(k => k.Contains("CPF")))   ;
            }

            if (ModelState.IsValid)
            {
                if (candidato.Id == null)
                {
                    _candidatoService.Cadastrar(candidato);
                    TempData["Success"] = "Candidato Cadastrado com sucesso!";
                }
                else
                {
                    _candidatoService.Alterar(candidato);
                    TempData["Success"] = "Candidato Alterado com sucesso!";
                }
                return RedirectToAction("Candidato", "Candidato", new { candidatoId = candidato.Id.Value });
            }
            else
            {
                return View(candidato);
            }
        }

        [HttpPost]
        public JsonResult BuscarPorNome(string nome)
        {
            var candidato = _candidatoService.BuscarPorNome(nome);
            return Json(candidato);
        }

        public IActionResult Candidato(int? candidatoId)
        {
            if (candidatoId != null)
            {
                var candidato = _candidatoService.BuscarPorId(candidatoId.Value);
                if (candidato != null)
                {
                    var candidatoViewModel = new CandidatoViewModel
                    {
                        Candidato = candidato,
                        Contratou = false
                    };
                    return View(candidatoViewModel);
                }
            }

            TempData["Error"] = "Candidato não encontrado!";
            return RedirectToAction("Index");
        }
    }
}