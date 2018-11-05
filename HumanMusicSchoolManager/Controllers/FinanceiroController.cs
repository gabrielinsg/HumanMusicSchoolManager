using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.ServicesInterface;
using Microsoft.AspNetCore.Mvc;

namespace HumanMusicSchoolManager.Controllers
{
    public class FinanceiroController : Controller
    {
        private readonly IFinanceiroService _financeiroService;
        private readonly IAlunoService _alunoService;

        public FinanceiroController(IFinanceiroService financeiroService,
            IAlunoService alunoService)
        {
            this._financeiroService = financeiroService;
            this._alunoService = alunoService;
        }

        public IActionResult Financeiro(int? alunoId)
        {
            if (alunoId != null)
            {
                return View(_alunoService.BuscarPorId(alunoId.Value));
            }
            else
            {
                return RedirectToAction("Aluno", "Index");
            }
        }
    }
}