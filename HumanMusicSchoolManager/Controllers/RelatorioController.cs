using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
 
}