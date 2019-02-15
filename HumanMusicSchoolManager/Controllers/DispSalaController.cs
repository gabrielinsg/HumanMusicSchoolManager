using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanMusicSchoolManager.Models.ViewModels;
using HumanMusicSchoolManager.Services;
using HumanMusicSchoolManager.ServicesInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HumanMusicSchoolManager.Controllers
{
    [Authorize]
    public class DispSalaController : Controller
    {
        private readonly IDispSalaService _dispSalaService;
        private readonly IPacoteCompraService _pacoteCompraService;
        private readonly ICursoService _cursoService;

        public DispSalaController(IDispSalaService dispSalaService,
            IPacoteCompraService pacoteCompraService,
            ICursoService cursoService)
        {
            this._dispSalaService = dispSalaService;
            this._pacoteCompraService = pacoteCompraService;
            this._cursoService = cursoService;
        }

        public IActionResult Index()
        {
            return View(_dispSalaService.BuscarTodos());
        }

        public IActionResult HorariosDisponiveis()
        {
            var horariosDisponiveis = new HorariosDisponiveisViewModel
            {
                DispSalas = _dispSalaService.HorariosDisponiveis(),
                Cursos = _cursoService.BuscarTodos()
            };
            return View(horariosDisponiveis);
        }
    }
}