using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanMusicSchoolManager.Data;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.Models.ViewModels;
using HumanMusicSchoolManager.ServicesInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HumanMusicSchoolManager.Controllers
{

    [Authorize]
    public class RelatorioController : Controller
    {
        private readonly IRelatorioService _relatorioSerevice;
        private readonly ApplicationDbContext _context;

        public RelatorioController(IRelatorioService relatorioService, ApplicationDbContext context)
        {
            this._relatorioSerevice = relatorioService;
            this._context = context;
        }

        public IActionResult Disponibilidade(int? professorId)
        {
            var professores = _relatorioSerevice.Professores();
            var dispSalas = new List<DispSala>();
            if (professorId == null)
            {
                var totalDispSala = new List<DispSala>();
                foreach (var professor in professores)
                {
                    foreach (var dispSala in professor.DispSalas.Where(ds => ds.Ativo == true).ToList())
                    {
                        totalDispSala.Add(dispSala);
                    }
                }

                dispSalas = totalDispSala;
            }
            else
            {
                dispSalas = professores.First(p => p.Id == professorId.Value).DispSalas;
            }

            var total = new RelatorioGeralProfessoresViewModel
            {
                DispSalas = dispSalas
            };

            ViewBag.Professores = professores.OrderBy(p => p.Nome);
            ViewBag.Professor = professorId == null ? "Total" : professores.First(p => p.Id == professorId).Nome;

            return View(total);
        }

        public IActionResult Alunos()
        {
            return View(new RelatorioAlunosViewModel { Alunos = _relatorioSerevice.Alunos() });
        }

        public IActionResult PacoteCompra(DateTime? inicial, DateTime? final)
        {
            if (inicial == null)
            {
                inicial = NowHorarioBrasilia.GetNow()
                    .AddDays(-NowHorarioBrasilia.GetNow().Day + 1)
                    .AddMonths(-NowHorarioBrasilia.GetNow().Month + 1);
            }

            if (final == null)
            {
                final = NowHorarioBrasilia.GetNow()
                    .AddDays(-NowHorarioBrasilia.GetNow().Day + 1)
                    .AddMonths(1)
                    .AddDays(-1)
                    .AddMonths(-NowHorarioBrasilia.GetNow().Month + 1)
                    .AddYears(1)
                    .AddMonths(-1);
            }

            var relatorioPacoteCompraViewModel = new RelatorioPacoteCompraViewModel()
            {
                PacoteComprasAtivos = _relatorioSerevice.PacotesAtivos(),
                PacoteContratados = _relatorioSerevice.PacotesContratados(inicial.Value, final.Value),
                Inicial = inicial.Value,
                Final = final.Value
            };


            return View(relatorioPacoteCompraViewModel);
        }

        public IActionResult MatriculasCanceladas(DateTime? inicial, DateTime? final)
        {
            if (inicial == null)
            {
                inicial = NowHorarioBrasilia.GetNow().AddDays(-NowHorarioBrasilia.GetNow().Day + 1);
            }

            if (final == null)
            {
                final = NowHorarioBrasilia.GetNow().AddDays(-NowHorarioBrasilia.GetNow().Day + 1).AddMonths(1).AddDays(-1);
                
            }

            var inicialAnterior = inicial.Value.AddMonths(-1).AddDays(-inicial.Value.AddMonths(-1).Day + 1);
            var finalAnterior = final.Value.AddMonths(-1).AddDays(-final.Value.AddMonths(-1).Day + 1).AddMonths(1).AddDays(-1);
            var matriculas = _relatorioSerevice.MatriculasCanceladas(inicial.Value, final.Value);
            var totalAnterior = _relatorioSerevice.MatriculasCanceladas(inicialAnterior, finalAnterior).Count;
            var positivo = matriculas.Count >= totalAnterior ? true : false;
            float porcentagem = 0;

            if (positivo)
            {
                porcentagem = (float) (matriculas.Count - totalAnterior) / totalAnterior * 100;
            }
            else
            {
                porcentagem = (float) (totalAnterior - matriculas.Count) / totalAnterior * 100;
            }

            ViewBag.Inicial = inicial.Value;
            ViewBag.Final = final.Value;
            ViewBag.Anterior = totalAnterior;
            ViewBag.Positivo = positivo;
            ViewBag.Porcentagem = porcentagem.ToString("#.##");
            return View(_relatorioSerevice.MatriculasCanceladas(inicial.Value, final.Value));
        }

        public IActionResult MatriculasNovas(DateTime? inicial, DateTime? final)
        {
            if (inicial == null)
            {
                inicial = NowHorarioBrasilia.GetNow().AddDays(-NowHorarioBrasilia.GetNow().Day + 1);
            }

            if (final == null)
            {
                final = NowHorarioBrasilia.GetNow().AddDays(-NowHorarioBrasilia.GetNow().Day + 1).AddMonths(1).AddDays(-1);
            }


            var inicialAnterior = inicial.Value.AddMonths(-1).AddDays(-inicial.Value.AddMonths(-1).Day + 1);
            var finalAnterior = final.Value.AddMonths(-1).AddDays(-final.Value.AddMonths(-1).Day + 1).AddMonths(1).AddDays(-1);
            var matriculas = _relatorioSerevice.MatriculasNovas(inicial.Value, final.Value);
            var totalAnterior = _relatorioSerevice.MatriculasNovas(inicialAnterior, finalAnterior).Count;
            var positivo = matriculas.Count >= totalAnterior ? true : false;
            float porcentagem = 0;

            if (positivo)
            {
                porcentagem = (float)(matriculas.Count - totalAnterior) / totalAnterior * 100;
            }
            else
            {
                porcentagem = (float)(totalAnterior - matriculas.Count) / totalAnterior * 100;
            }

            ViewBag.Inicial = inicial.Value;
            ViewBag.Final = final.Value;
            ViewBag.Anterior = totalAnterior;
            ViewBag.Positivo = positivo;
            ViewBag.Porcentagem = porcentagem.ToString("#.##");
            return View(_relatorioSerevice.MatriculasNovas(inicial.Value, final.Value));
        }

        public IActionResult Aniversariantes(int? mes)
        {
            if (mes == null)
            {
                mes = NowHorarioBrasilia.GetNow().AddDays(-NowHorarioBrasilia.GetNow().Day + 1).Month;
            }

            ViewBag.Mes = mes.Value;

            var alunos = _context.Alunos
                .Where(a =>
                    a.Matriculas.Any(m => m.EncerramentoMatricula == null)
                    && (a.DataNascimento.Month == mes)
                )
                .ToList();

            var funcionarios = _context.Funcionarios.Where(f => f.Ativo && f.DataNascimento.Month == mes).ToList();
            var responsaveis = _context.RespsFinanceiro
                .Where(r =>
                    r.Matriculas.Any(m => m.EncerramentoMatricula == null)
                    && (r.DataNascimento.Month == mes)
                ).ToList();

            var professores = _context.Professores.Where(p => p.Ativo && p.DataNascimento.Month == mes).ToList();

            var aniversariantes = new Dictionary<string, object>
            {
                { "Alunos", alunos },
                { "Responsaveis", responsaveis },
                { "Funcionarios", funcionarios },
                { "Professores", professores }
            };

            return View(aniversariantes);
        }

        public IActionResult Demonstrativas(DateTime? inicial, DateTime? final)
        {
            if (inicial == null)
            {
                inicial = NowHorarioBrasilia.GetNow().AddDays(-NowHorarioBrasilia.GetNow().Day + 1);
            }

            if (final == null)
            {
                final = NowHorarioBrasilia.GetNow().AddDays(-NowHorarioBrasilia.GetNow().Day + 1).AddMonths(1).AddDays(-1);
            }
            var inicialAnterior = inicial.Value.AddMonths(-1).AddDays(-inicial.Value.AddMonths(-1).Day + 1);
            var finalAnterior = final.Value.AddMonths(-1).AddDays(-final.Value.AddMonths(-1).Day + 1).AddMonths(1).AddDays(-1);
            var demonstrativas = _relatorioSerevice.Demostrativas(inicial.Value, final.Value);
            var totalAnterior = _relatorioSerevice.Demostrativas(inicialAnterior, finalAnterior).Count;
            var positivo = demonstrativas.Count >= totalAnterior ? true : false;
            float porcentagem = 0;

            if (positivo)
            {
                porcentagem = (float) (demonstrativas.Count - totalAnterior) / totalAnterior * 100;
            }
            else
            {
                porcentagem = (float) (totalAnterior - demonstrativas.Count) / totalAnterior * 100;
            }

            ViewBag.Inicial = inicial.Value;
            ViewBag.Final = final.Value;
            ViewBag.Anterior = totalAnterior;
            ViewBag.Positivo = positivo;
            ViewBag.Porcentagem = porcentagem.ToString("#.##");
            return View(demonstrativas);
        }

        public IActionResult MatriculasAtivas()
        {

            var matriculas = _context.Matriculas.Where(m => m.DispSalaId != null)
            .Include(m => m.Curso)
            .ToList();

            var cursos = new Dictionary<string, int>();
            foreach (var curso in matriculas.Select(m => m.Curso).Distinct().ToList())
            {
                cursos.Add(curso.Nome, matriculas.Where(m => m.CursoId == curso.Id).ToList().Count);
            }
            cursos.Add("Total", matriculas.Count);

            cursos.OrderBy(c => c.Value);
            return View(cursos);
        }

        public IActionResult MatriculasSemAula()
        {
            var matriculas = _context.Matriculas
                             .Where(m => m.EncerramentoMatricula == null && !m.PacoteCompras.Any(pc => pc.Chamadas.Any(c => c.Presenca == null)))
                             .ToList();

            return View(matriculas);
        }
    }
 
}