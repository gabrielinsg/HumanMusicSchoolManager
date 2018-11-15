using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.Models.ViewModels;
using HumanMusicSchoolManager.Services;
using HumanMusicSchoolManager.ServicesInterface;
using Microsoft.AspNetCore.Mvc;

namespace HumanMusicSchoolManager.Controllers
{
    public class SalaController : Controller
    {
        private readonly ISalaService _salaService;
        private readonly ICursoService _cursoService;
        private readonly IProfessorService _professorService;
        private readonly IFeriadoService _feriadoService;
        private readonly IEventoService _eventoService;
        private readonly IAulaService _aulaService;

        public SalaController(ISalaService salaService, 
            ICursoService cursoService, 
            IProfessorService professorService,
            IFeriadoService feriadoService,
            IEventoService eventoService,
            IAulaService aulaService)
        {
            this._salaService = salaService;
            this._cursoService = cursoService;
            this._professorService = professorService;
            this._feriadoService = feriadoService;
            this._eventoService = eventoService;
            this._aulaService = aulaService;
        }

        public IActionResult Index()
        {
            return View(_salaService.BuscarTodos());
        }

        [HttpGet]
        public IActionResult Form(int? salaId)
        {
            if (salaId == null)
            {
                return View();
            }
            else
            {
                var sala = _salaService.BuscarPorId(salaId.Value);
                if (sala != null)
                {
                    return View(sala);
                }
                else
                {
                    return View();
                }
            }
        }

        [HttpPost]
        public IActionResult Form(Sala sala)
        {
            if (sala.Id == null)
            {
                if (ModelState.IsValid)
                {
                    _salaService.Cadastrar(sala);
                    TempData["Success"] = "Sala cadastrada com sucesso!";
                    return RedirectToAction("Form");
                }
                else
                {
                    return View(sala);
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    _salaService.Alterar(sala);
                    TempData["Success"] = "Sala alterada com sucesso!";
                    return RedirectToAction("Form");
                }
                else
                {
                    return View(sala);
                }
            }
        }

        public IActionResult AdicionarCurso(int? cursoId, int? salaId)
        {

            if (salaId != null && cursoId != null)
            {
                _salaService.AdicionarCurso(salaId.Value, cursoId.Value);
            }

            return RedirectToAction(actionName: "SalaCurso", routeValues: new { salaId = salaId });

        }

        public IActionResult RemoverCurso(int? cursoId, int? salaId)
        {

            if (salaId != null && cursoId != null)
            {
                _salaService.RemoverCurso(salaId.Value, cursoId.Value);
            }

            return RedirectToAction(actionName: "SalaCurso", routeValues: new { salaId = salaId });

        }

        public IActionResult SalaCurso(int? salaId)
        {
            if (salaId != null)
            {
                var professor = new SalaCursoViewModel(_salaService.BuscarPorId(salaId.Value), _cursoService.BuscarTodos().Where(c => c.Ativo == true).ToList());
                return View(professor);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public JsonResult BuscarPorNome(string nome)
        {
            var sala = _salaService.BuscarPorNome(nome);
            return Json(sala);
        }

        [HttpGet]
        public IActionResult DispSala(int? salaId, int? dispSalaId)
        {
            if (salaId != null)
            {
                var sala = _salaService.BuscarPorId(salaId.Value);

                var cursos = sala.Cursos.Select(c => c.Curso).ToList();

                var professores = new List<Professor>();

                foreach (var curso in cursos)
                {
                    var professors = curso.Professores.Select(p => p.Professor).ToList();
                    professores.AddRange(professors);
                }
                professores = professores.Distinct().ToList();

                if (dispSalaId == null)
                {
                    return View(new DispSalaViewModel(sala, professores));
                }
                else
                {
                    var dispSala = sala.DispSalas.SingleOrDefault(ds => ds.Id == dispSalaId.Value);
                    return View(new DispSalaViewModel(sala, dispSala, professores));
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult DispSala(DispSala dispSala, Sala sala)
        {
            sala = _salaService.BuscarPorId(sala.Id.Value);
            if (dispSala.Id == null)
            {
                if (dispSala.Professor.Id != null)
                {
                    dispSala.Professor = _professorService.BuscarPorId(dispSala.Professor.Id.Value);
                }

                sala.DispSalas.Add(dispSala);
                _salaService.Alterar(sala);
                TempData["Success"] = "Horário incluído com sucesso!";
                return RedirectToAction("DispSala", new { salaId = sala.Id });

            }
            else
            {
                if (dispSala.Professor.Id != null)
                {
                    dispSala.Professor = _professorService.BuscarPorId(dispSala.Professor.Id.Value);
                }

                var index = sala.DispSalas.FindIndex(d => d.Id == dispSala.Id);
                sala.DispSalas[index].Dia = dispSala.Dia;
                sala.DispSalas[index].Hora = dispSala.Hora;
                sala.DispSalas[index].Professor = dispSala.Professor;
                _salaService.Alterar(sala);
                TempData["Success"] = "Horário alterado com sucesso!";
                return RedirectToAction("DispSala", new { salaId = sala.Id });

            }
        }

        public IActionResult ExcluirDispSala(int? dispSalaId, int? salaId)
        {
            if (dispSalaId != null && salaId != null)
            {
                var sala = _salaService.BuscarPorId(salaId.Value);
                var dispSala = sala.DispSalas.SingleOrDefault(ds => ds.Id == dispSalaId);
                sala.DispSalas.Remove(dispSala);
                _salaService.Alterar(sala);
                TempData["Success"] = "Horário removido com sucesso!";
                return RedirectToAction("DispSala", new { salaId = salaId });
            }
            else if (salaId != null)
            {
                return RedirectToAction("DispSala", new { salaId = salaId });
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public IActionResult Excluir(int? salaId)
        {
            if (salaId != null)
            {
                _salaService.Excluir(salaId.Value);
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public IActionResult Calendario(int? salaId)
        {

            var sala = _salaService.BuscarPorId(salaId.Value);

            if (sala != null)
            {
                return View(sala);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public JsonResult CalendarioJson(int salaId)
        {

            var aulas = _aulaService.BuscarPorSala(salaId);
            var feriados = _feriadoService.BuscarTodos();
            var eventos = _eventoService.BuscarTodos();
            var calendar = new List<CalendarJson>();

            //Aulas
            foreach (var aula in aulas)
            {
                var start = aula.Data;
                var end = start.AddMinutes(55);
                var color = "";
                if (aula.AulaDada == true)
                {
                    color = "#28a745";
                }
                else if (aula.AulaDada == false && aula.Data < DateTime.Now)
                {
                    color = "#dc3545";
                }
                else
                {
                    color = "#007bff";
                }
                var cal = new CalendarJson()
                {
                    Start = start.ToString("yyyy-MM-ddTHH:mm:ss"),
                    End = end.ToString("yyyy-MM-ddTHH:mm:ss"),
                    Color = color,
                    Title = "Aula - ",
                    Url = "/Aula/Form?aulaId=" + aula.Id
                };

                int ultimo = 0;
                foreach (var chamada in aula.Chamadas)
                {
                    ultimo++;
                    var nome = chamada.PacoteCompra.Matricula.Aluno.Nome.Split(' ');
                    if (ultimo == aula.Chamadas.Count && ultimo != 1)
                    {
                        cal.Title += ", " + nome[0];
                    }
                    else
                    {
                        cal.Title += nome[0];
                    }
                }
                calendar.Add(cal);
            }

            //Feriados
            foreach (var feriado in feriados)
            {
                var cal = new CalendarJson()
                {
                    Title = feriado.Nome,
                    Start = feriado.DataInicial.ToString("yyyy-MM-dd"),
                    Color = "#ffc107"
                };
                if (feriado.DataFinal != null)
                {
                    cal.End = feriado.DataFinal.Value.ToString("yyyy-MM-dd");
                }
                calendar.Add(cal);
            }

            //Eventos
            foreach (var evento in eventos)
            {
                var calendarJson = new CalendarJson()
                {
                    Title = evento.Nome,
                    Start = evento.DataInicial.ToString("yyyy-MM-dd"),
                    Color = "#6c757d"
                };
                if (evento.DataFinal != null)
                {
                    calendarJson.End = evento.DataFinal.Value.ToString("yyyy-MM-dd");
                }
                calendar.Add(calendarJson);
            }

            return Json(calendar);
        }

        internal class CalendarJson
        {

            public string Title { get; set; }
            public string Start { get; set; }
            public string Url { get; set; }
            public string Color { get; set; }
            public string End { get; set; }
        }
    }
}