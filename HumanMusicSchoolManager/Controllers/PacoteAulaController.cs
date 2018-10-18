using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanMusicSchoolManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace HumanMusicSchoolManager.Controllers
{
    public class PacoteAulaController : Controller
    {
        private readonly IPacoteAulaService _pacoteAulaService;

        public PacoteAulaController(IPacoteAulaService pacoteAulaService)
        {
            this._pacoteAulaService = pacoteAulaService;
        }

        public IActionResult Index()
        {
            return View(_pacoteAulaService.BuscarTodos());
        }

        [HttpGet]
        public IActionResult Form(int? pacoteAulaId)
        {
            if (pacoteAulaId == null)
            {
                return View();
            }
            else
            {
                var pacoteAula = _pacoteAulaService.BuscarPorId(pacoteAulaId.Value);
                return View(pacoteAula);
            }
        }
    }
}