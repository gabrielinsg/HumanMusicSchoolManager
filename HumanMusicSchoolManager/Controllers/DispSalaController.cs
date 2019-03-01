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
    [Authorize(Roles = "Admin, Atendimento, Vendas")]
    public class DispSalaController : Controller
    {
        private readonly IDispSalaService _dispSalaService;
        private readonly IPacoteCompraService _pacoteCompraService;
        private readonly ICursoService _cursoService;
        private readonly ISalaService _salaService;

        public DispSalaController(IDispSalaService dispSalaService,
            IPacoteCompraService pacoteCompraService,
            ICursoService cursoService,
            ISalaService salaService)
        {
            this._dispSalaService = dispSalaService;
            this._pacoteCompraService = pacoteCompraService;
            this._cursoService = cursoService;
            this._salaService = salaService;
        }

        public IActionResult Index()
        {
            return View(_dispSalaService.BuscarTodos());
        }

        public IActionResult HorariosDisponiveis(int? salaId)
        {
            var horariosDisponiveis = new HorariosDisponiveisViewModel
            {
                Cursos = _cursoService.BuscarTodos()
            };

            if (salaId == null)
            {
                horariosDisponiveis.DispSalas = _dispSalaService.HorariosDisponiveis();                
            }
            else
            {
                horariosDisponiveis.DispSalas = _dispSalaService.HorariosDisponiveisPorSala(salaId.Value);
            }

            ViewBag.Salas = _salaService.BuscarTodos().OrderBy(s => s.Nome).ToList();

            return View(horariosDisponiveis);
        }

        public IActionResult TodosHorarios(int? salaId)
        {
            var horarios = new TodosHorariosViewModel
            {
                Cursos = _cursoService.BuscarTodos()
            };

            if (salaId == null)
            {
                horarios.DispSalas = _dispSalaService.BuscarTodos();
            }
            else
            {
                horarios.DispSalas = _dispSalaService.BuscarTodosPorSala(salaId.Value);
            }

            ViewBag.Salas = _salaService.BuscarTodos().OrderBy(s => s.Nome).ToList();

            return View(horarios);
        }
    }
}