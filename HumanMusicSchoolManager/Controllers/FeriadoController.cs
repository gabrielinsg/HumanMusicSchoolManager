using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.ServicesInterface;
using Microsoft.AspNetCore.Mvc;

namespace HumanMusicSchoolManager.Controllers
{
    public class FeriadoController : Controller
    {
        private readonly IFeriadoService _feriadoService;

        public FeriadoController(IFeriadoService feriadoService)
        {
            this._feriadoService = feriadoService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Form(int? feriadoId)
        {
            if (feriadoId == null)
            {
                return View();
            }
            else
            {
                return View(_feriadoService.BuscarPorId(feriadoId.Value));
            }
        }

        [HttpPost]
        public IActionResult Form(Feriado feriado)
        {

            if (feriado.DataFinal != null && feriado.DataFinal.Value.Date < feriado.DataInicial.Date)
            {
                ModelState.AddModelError("DataFinal", "Data final deve ser maior que Data inicial");
            }

            if (feriado.Id == null)
            {
                if (ModelState.IsValid)
                {
                    _feriadoService.Cadastrar(feriado);
                    TempData["Success"] = "Feriado cadastrado com sucesso!";
                    return RedirectToAction("Form");
                }
                else
                {
                    return View(feriado);
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    _feriadoService.Alterar(feriado);
                    TempData["Success"] = "Feriado alterado com sucesso!";
                    return RedirectToAction("Form");
                }
                else
                {
                    return View(feriado);
                }
            }
        }

        public IActionResult Excluir(int? feriadoId)
        {
            if (feriadoId != null)
            {
                _feriadoService.Excluir(feriadoId.Value);
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public JsonResult BuscarTodos()
        {
            var feriados = _feriadoService.BuscarTodos();
            var feriadosJson = new List<FeriadoJson>();
            foreach (var feriado in feriados)
            {
                var feriadoJson = new FeriadoJson()
                {
                    Title = feriado.Nome,
                    Start = feriado.DataInicial.ToString("yyyy-MM-dd"),
                    Url = "Feriado/Form?feriadoId=" + feriado.Id.Value,
                    Color = "#ffc107"
                };
                if (feriado.DataFinal != null)
                {
                    feriadoJson.End = feriado.DataFinal.Value.ToString("yyyy-MM-dd");
                }
                feriadosJson.Add(feriadoJson);
            }
            return Json(feriadosJson);
        }
    }

    class FeriadoJson
    {
        public string Title { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string Color { get; set; }
        public string Url { get; set; }
    }
}