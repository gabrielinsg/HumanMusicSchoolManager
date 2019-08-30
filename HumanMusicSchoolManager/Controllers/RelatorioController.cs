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

    [Authorize]
    public class RelatorioController : Controller
    {
        private readonly IRelatorioService _relatorioSerevice;

        public RelatorioController(IRelatorioService relatorioService)
        {
            this._relatorioSerevice = relatorioService;
        }

        public IActionResult Disponibilidade()
        {
            return View(_relatorioSerevice.Professores());
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

            ViewBag.Inicial = inicial.Value;
            ViewBag.Final = final.Value;
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

            ViewBag.Inicial = inicial.Value;
            ViewBag.Final = final.Value;
            return View(_relatorioSerevice.MatriculasNovas(inicial.Value, final.Value));
        }

        public IActionResult AlunosAniversariantes(int? mes)
        {
            if (mes == null)
            {
                mes = NowHorarioBrasilia.GetNow().AddDays(-NowHorarioBrasilia.GetNow().Day + 1).Month;
            }

            ViewBag.Mes = mes.Value;
            return View(_relatorioSerevice.AlunosAniversariantes(mes.Value));
        }
    }
 
}