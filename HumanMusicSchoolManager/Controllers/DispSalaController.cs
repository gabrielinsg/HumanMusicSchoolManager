using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanMusicSchoolManager.ServicesInterface;
using Microsoft.AspNetCore.Mvc;

namespace HumanMusicSchoolManager.Controllers
{
    public class DispSalaController : Controller
    {
        private readonly IDispSalaService _dispSalaService;
        private readonly IPacoteCompraService _pacoteCompraService;

        public DispSalaController(IDispSalaService dispSalaService,
            IPacoteCompraService pacoteCompraService)
        {
            this._dispSalaService = dispSalaService;
            this._pacoteCompraService = pacoteCompraService;
        }

        public IActionResult Index()
        {
            return View(_dispSalaService.BuscarTodos());
        }
    }
}