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
    [Authorize]
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

            var professor = _professorService.BuscarPorId(_pessoaService.BusacarPorUserName(User.Identity.Name).Id.Value);

            if(professor != null)
            {
                ViewBag.user = 0;
                return View(professor);
            }
            else
            {
                if (professorId != null)
                {
                    professor = _professorService.BuscarPorId(professorId.Value);
                    if (professor != null)
                    {
                        ViewBag.user = 1;
                        return View(professor);
                    }
                }

                return RedirectToAction("Index", "Home");

            }
        }

        public JsonResult CalendarioJson(int professorId, int user)
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
                else if (aula.AulaDada == false && aula.Data < NowHorarioBrasilia.GetNow())
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
                if (user == 1)
                {
                    cal.Url = "/Aula/Aula?aulaId=" + aula.Id;
                }
                else
                {
                    cal.Url = "/Aula/Form?aulaId=" + aula.Id;
                }

                
                if (aula.Chamadas.Count  > 1)
                {
                    var nomes = new List<string>();

                    foreach (var chamada in aula.Chamadas)
                    {
                        nomes.Add(chamada.PacoteCompra.Matricula.Aluno.Nome.Split(' ')[0]);
                    }

                    cal.Title += CarregarNomes(nomes);
                }
                else if (aula.Chamadas.Count > 0)
                {
                    cal.Title += " " + aula.Chamadas.FirstOrDefault().PacoteCompra.Matricula.Aluno.Nome.Split(' ')[0];
                }
                    
                
                if (aula.Chamadas.Count > 0 && aula.Demostrativas.Count > 0)
                {
                    cal.Title += ", ";
                }
                
                if (aula.Demostrativas.Count > 1)
                {
                    var nomes = new List<string>();

                    foreach (var demo in aula.Demostrativas)
                    {
                        nomes.Add("(D)"+demo.Candidato.Nome.Split(' ')[0]);
                    }

                    cal.Title += CarregarNomes(nomes);
                }
                else if (aula.Demostrativas.Count > 0)
                {
                    cal.Title += " (D)" + aula.Demostrativas.FirstOrDefault().Candidato.Nome.Split(' ')[0];
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

            var professorPorUsuario = _professorService.BuscarPorId(_pessoaService.BusacarPorUserName(User.Identity.Name).Id.Value);
            if (professorPorUsuario != null)
            {
                professorId = professorPorUsuario.Id.Value;
            }

            if (professorId != null)
            {
                var professor = _professorService.BuscarPorId(professorId.Value);
                if (professor != null)
                {

                    if (inicial == null)
                    {
                        inicial = NowHorarioBrasilia.GetNow().AddDays(- NowHorarioBrasilia.GetNow().Day + 1);
                    }

                    if (final == null)
                    {
                        final = NowHorarioBrasilia.GetNow().AddDays(-NowHorarioBrasilia.GetNow().Day + 1).AddMonths(1).AddDays(-1);
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
                    inicial = NowHorarioBrasilia.GetNow().AddDays(-NowHorarioBrasilia.GetNow().Day + 1);
                }

                if (final == null)
                {
                    final = NowHorarioBrasilia.GetNow().AddDays(-NowHorarioBrasilia.GetNow().Day + 1).AddMonths(1).AddDays(-1);
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

        private string CarregarNomes(List<string> nomes)
        {
            string title = "";

            for (var i = 0; i < nomes.Count; i++)
            {
                if (i == nomes.Count - 1)
                {
                    title += " " + nomes[i];
                }
                else
                {
                    title += " " + nomes[i] + ",";
                }
            }

            return title;
        }

        public IActionResult RelatorioProfessor(int? professorId, DateTime? inicial, DateTime? final)
        {
            var professorPorUsuario = _professorService.BuscarPorId(_pessoaService.BusacarPorUserName(User.Identity.Name).Id.Value);
            if (professorPorUsuario != null)
            {
                professorId = professorPorUsuario.Id.Value;
            }

            if (professorId != null)
            {
                var professor = _professorService.BuscarPorId(professorId.Value);
                if (professor != null)
                {

                    if (inicial == null)
                    {
                        inicial = NowHorarioBrasilia.GetNow().AddDays(-NowHorarioBrasilia.GetNow().Day + 1);
                    }

                    if (final == null)
                    {
                        final = NowHorarioBrasilia.GetNow().AddDays(-NowHorarioBrasilia.GetNow().Day + 1).AddMonths(1).AddDays(-1);
                    }

                    var professorCompletoViewModel = _professorService.RelatorioCompleto(professor.Id.Value, inicial.Value, final.Value);
                    professorCompletoViewModel.Inicial = inicial.Value;
                    professorCompletoViewModel.Final = final.Value;


                    return View(professorCompletoViewModel);
                }
            }

            TempData["Error"] = "Professor não encontrado";
            return RedirectToAction("Index", "Professor");
        }

    }
}