using System;
using System.Collections.Generic;
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
    public class ProfessorController : Controller
    {
        private readonly IProfessorService _professorService;
        private readonly ICursoService _cursoService;
        private readonly IPessoaService _pessoaService;
        private readonly IFeriadoService _feriadoService;
        private readonly IEventoService _eventoService;

        public ProfessorController(IProfessorService professorService, 
            ICursoService cursoService, 
            IPessoaService pessoaService,
            IFeriadoService feriadoService,
            IEventoService eventoService)
        {
            this._professorService = professorService;
            this._cursoService = cursoService;
            this._pessoaService = pessoaService;
            this._feriadoService = feriadoService;
            this._eventoService = eventoService;
        }

        [Authorize(Roles = "Coordenacao, Atendimento, Diretoria, Secretaria, Admin")]
        public IActionResult Index()
        {            
            return View(_professorService.BuscarTodos());
        }

        [HttpGet]
        public IActionResult Form(int? professorId)
        {
            if(professorId == null)
            {
                return View();
            }
            else
            {
                var professor = _professorService.BuscarPorId(professorId.Value);
                if (professor != null)
                {
                    return View(professor);

                }
                else
                {
                    return View();
                }
            }
        }

        [HttpPost]
        public IActionResult Form(Professor professor)
        {
            if (professor.Id == null)
            {
                if (ModelState.IsValid)
                {
                    var cpfUnico = _professorService.BuscarPorCPF(professor.CPF);
                    if (cpfUnico == null)
                    {
                        _professorService.Cadastrar(professor);

                        TempData["Success"] = "Professor Cadastrado com sucesso!";

                        return RedirectToAction("Form");
                    }
                    else
                    {
                        ModelState.AddModelError("CPF", "CPF já cadastrado para outro professor");
                        return View(professor);
                    }
                    
                }
                else
                {
                    return View(professor);
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    _professorService.Alterar(professor);

                    TempData["Success"] = "Professor Alterado com sucesso!";

                    return RedirectToAction("Form");

                }
                else
                {
                    return View(professor);
                }
            }
        }

        public IActionResult Excluir(int? professorId)
        {
            if (professorId != null)
            {
                var professor = _professorService.BuscarPorId(professorId.Value);
                _professorService.Excluir(professor);
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Coordenacao, Atendimento, Diretoria, Secretaria, Admin")]
        public IActionResult Professor(int? professorId)
        {
            if (professorId != null)
            {
                var professor = _professorService.BuscarPorId(professorId.Value);
                return View(professor);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public IActionResult ProfessorCurso(int? professorId)
        {
            if (professorId != null)
            {
                var professor = new ProfessorCursoViewModel(_professorService.BuscarPorId(professorId.Value), _cursoService.BuscarTodos().Where(c => c.Ativo == true).ToList());
                return View(professor);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public IActionResult AdicionarCurso(int? cursoId, int? professorId)
        {

            if (professorId != null && cursoId != null)
            {
                _professorService.AdicionarCurso(professorId.Value, cursoId.Value);
            }

            return RedirectToAction(actionName: "ProfessorCurso", routeValues: new { professorId = professorId });

        }

        public IActionResult RemoverCurso(int? cursoId, int? professorId)
        {

            if (professorId != null && cursoId != null)
            {
                _professorService.RemoverCurso(professorId.Value, cursoId.Value);
            }

            return RedirectToAction(actionName: "ProfessorCurso", routeValues: new { professorId = professorId });

        }

        [HttpPost]
        public JsonResult BuscarPorNome(string nome)
        {
            var professor = _professorService.BuscarPorNome(nome);
            return Json(professor);
        }

        public IActionResult Calendario()
        {
            var professor = _professorService.BuscarPorId(_pessoaService.GetUser(User.Identity.Name).Id.Value);
            if (professor != null)
            {
                return View(professor);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public JsonResult CalendarioJson(int professorId)
        {

            var professor = _professorService.BuscarPorId(professorId);
            var feriados = _feriadoService.BuscarTodos();
            var eventos = _eventoService.BuscarTodos();
            var calendar = new List<CalendarJson>();

            //Aulas
            foreach (var aula in professor.Aulas)
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
                    Title = "Aula ",
                    Url = "/Aula/Form?aulaId=" + aula.Id
                };
                foreach (var chamada in aula.Chamadas)
                {
                    var nome = chamada.PacoteCompra.Matricula.Aluno.Nome.Split(' ');
                    cal.Title += nome[0]+" ";
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
                    Url = "Feriado/Form?feriadoId=" + feriado.Id.Value,
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