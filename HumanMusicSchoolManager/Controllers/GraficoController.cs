using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.Services;
using HumanMusicSchoolManager.ServicesInterface;
using Microsoft.AspNetCore.Mvc;

namespace HumanMusicSchoolManager.Controllers
{
    public class GraficoController : Controller
    {
        private readonly IGraficoService _graficoService;
        private readonly ICursoService _cursoService;

        public GraficoController(
            IGraficoService graficoService,
            ICursoService cursoService)
        {
            this._graficoService = graficoService;
            this._cursoService = cursoService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public JsonResult EntradaSaida(List<Curso> cursos, DateTime? inicial, DateTime? final)
        {
            if (inicial == null)
            {
                inicial = NowHorarioBrasilia.GetNow().AddDays(-NowHorarioBrasilia.GetNow().Day + 1);
            }

            if (final == null)
            {
                final = NowHorarioBrasilia.GetNow().AddDays(-NowHorarioBrasilia.GetNow().Day + 1).AddMonths(1).AddDays(-1);
            }

            if (cursos.Count() == 0) cursos = _cursoService.BuscarTodos();

            return Json(_graficoService.EntradasSaidas(inicial.Value, final.Value, cursos));
        }
    }
}