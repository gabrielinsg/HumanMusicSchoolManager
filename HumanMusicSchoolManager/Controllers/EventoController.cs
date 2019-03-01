using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.ServicesInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HumanMusicSchoolManager.Controllers
{
    [Authorize(Roles = "Admin, Coordenacao")]
    public class EventoController : Controller
    {
        private readonly IEventoService _eventoService;

        public EventoController(IEventoService eventoService)
        {
            this._eventoService = eventoService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Form(int? eventoId)
        {
            if (eventoId == null)
            {
                return View();
            }
            else
            {
                return View(_eventoService.BuscarPorId(eventoId.Value));
            }
        }

        [HttpPost]
        public IActionResult Form(Evento evento)
        {

            if (evento.DataFinal != null && evento.DataFinal.Value.Date < evento.DataInicial.Date)
            {
                ModelState.AddModelError("DataFinal", "Data final deve ser maior que Data inicial");
            }

            if (evento.Id == null)
            {
                if (ModelState.IsValid)
                {
                    _eventoService.Cadastrar(evento);
                    TempData["Success"] = "Evento cadastrado com sucesso!";
                    return RedirectToAction("Form");
                }
                else
                {
                    return View(evento);
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    _eventoService.Alterar(evento);
                    TempData["Success"] = "Evento alterado com sucesso!";
                    return RedirectToAction("Form");
                }
                else
                {
                    return View(evento);
                }
            }
        }

        public IActionResult Excluir(int? eventoId)
        {
            if (eventoId != null)
            {
                _eventoService.Excluir(eventoId.Value);
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public JsonResult BuscarTodos()
        {
            var eventos = _eventoService.BuscarTodos();
            var eventosJson = new List<EventoJson>();
            foreach (var evento in eventos)
            {
                var eventoJson = new EventoJson()
                {
                    Title = evento.Nome,
                    Start = evento.DataInicial.ToString("yyyy-MM-dd"),
                    Url = "Evento/Form?eventoId=" + evento.Id.Value,
                    Color = "#28a745"
                };
                if (evento.DataFinal != null)
                {
                    eventoJson.End = evento.DataFinal.Value.AddDays(1).ToString("yyyy-MM-dd");
                }
                eventosJson.Add(eventoJson);
            }
            return Json(eventosJson);
        }
    }

    internal class EventoJson
    {

        public string Title { get; set; }
        public string Start { get; set; }
        public string Url { get; set; }
        public string Color { get; set; }
        public string End { get; set; }
    }
}
