using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanMusicSchoolManager.Models.ViewModels;
using HumanMusicSchoolManager.ServicesInterface;
using Microsoft.AspNetCore.Mvc;

namespace HumanMusicSchoolManager.Controllers
{
    public class PacoteCompraController : Controller
    {
        private readonly IPacoteCompraService _pacoteCompraService;
        private readonly IMatriculaService _matriculaService;
        private readonly IPacoteAulaService _pacoteAulaService;

        public PacoteCompraController(IPacoteCompraService pacoteCompraService, 
            IMatriculaService matriculaService,
            IPacoteAulaService pacoteAulaService)
        {
            this._pacoteCompraService = pacoteCompraService;
            this._matriculaService = matriculaService;
            this._pacoteAulaService = pacoteAulaService;
        }

        public IActionResult Form(int? matriculaId, int? pacoteAulaId)
        {
            if (matriculaId != null)
            {
                var pacoteCompraViewModel = new PacoteCompraViewModel()
                {
                    Matricula = _matriculaService.BuscarPorId(matriculaId.Value),
                    PacotesAula = _pacoteAulaService.BuscarTodos()
                };

                if (pacoteAulaId != null)
                {
                    pacoteCompraViewModel.PacoteAula = _pacoteAulaService.BuscarPorId(pacoteAulaId.Value);
                }

                return View(pacoteCompraViewModel);
            }
            else
            {
                return RedirectToAction("Aluno", "Index");
            }
        }
    }
}