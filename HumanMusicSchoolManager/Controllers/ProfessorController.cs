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

        public IActionResult Calendario(int? professorId)
        {
            Professor professor = null;

            if (professorId == null)
            {
                professor = _professorService.BuscarPorId(_pessoaService.GetUser(User.Identity.Name).Id.Value);
                ViewBag.user = false;
            }
            else
            {
                professor = _professorService.BuscarPorId(professorId.Value);
                ViewBag.user = true;

            }

            if (professor != null)
            {
                return View(professor);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public JsonResult CalendarioJson(int professorId, bool user)
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
                    Title = "Aula - "                    
                };
                if (user == true)
                {
                    cal.Url = "/Aula/Aula?aulaId=" + aula.Id;
                }
                else
                {
                    cal.Url = "/Aula/Form?aulaId=" + aula.Id;
                }

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
                ultimo = 0;
                if (aula.Chamadas.Count > 0 && aula.Demostrativas.Count > 0)
                {
                    cal.Title += ", ";
                }
                foreach (var demostrativa in aula.Demostrativas)
                {
                    ultimo++;
                    var nome = demostrativa.Candidato.Nome.Split(' ');
                    if (ultimo == aula.Demostrativas.Count && ultimo != 1)
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

        public IActionResult FolhaPontoProfessor(int? professorId, DateTime? inicial, DateTime? final)
        {
            if (professorId != null)
            {
                var professor = _professorService.BuscarPorId(professorId.Value);
                if (professor != null)
                {

                    if (inicial == null)
                    {
                        inicial = DateTime.Now.AddDays(- DateTime.Now.Day + 1);
                    }

                    if (final == null)
                    {
                        final = DateTime.Now.AddDays(-DateTime.Now.Day + 1).AddMonths(1).AddDays(-1);
                    }

                    var professorEncontrado = _professorService.BuscarPorIdData(professor.Id.Value, (DateTime)inicial, (DateTime)final);

                    var professorDataViewModel = new ProfessorPorDataViewModel
                    {
                        Professores = new List<Professor> { professorEncontrado },
                        Inicial = (DateTime)inicial,
                        Final = (DateTime)final,
                        Professor = professor
                    };

                    return View(professorDataViewModel);
                }
            }
            else
            {
                if (inicial == null)
                {
                    inicial = DateTime.Now.AddDays(-DateTime.Now.Day + 1);
                }

                if (final == null)
                {
                    final = DateTime.Now.AddDays(-DateTime.Now.Day + 1).AddMonths(1).AddDays(-1);
                }

                var professorDataViewModel = new ProfessorPorDataViewModel
                {
                    Professores = _professorService.BuscarTodosData((DateTime)inicial, (DateTime)final),
                    Inicial = (DateTime)inicial,
                    Final = (DateTime)final
                };

                return View(professorDataViewModel);
            }

            TempData["Error"] = "Professor não encontrado";
            return RedirectToAction("Index", "Professor");
        }
    }
}