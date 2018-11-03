using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanMusicSchoolManager.ServicesInterface;
using Microsoft.AspNetCore.Mvc;

namespace HumanMusicSchoolManager.Controllers
{
    public class FinanceiroController : Controller
    {
        private readonly IFinanceiroService _financeiroService;

        public FinanceiroController(IFinanceiroService financeiroService)
        {
            this._financeiroService = financeiroService;
        }

        public IActionResult Aluno(int? alunoId)
        {
            if (alunoId != null)
            {
                return View(_financeiroService.BuscarPorAluno(alunoId.Value));
            }
            else
            {
                return RedirectToAction("Aluno", "Index");
            }
        }
    }
}