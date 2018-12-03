using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanMusicSchoolManager.ServicesInterface;
using Microsoft.AspNetCore.Mvc;

namespace HumanMusicSchoolManager.Controllers
{
    public class TrancamentoController : Controller
    {
        private readonly ITrancamentoService _trancamentoService;

        public TrancamentoController (ITrancamentoService trancamentoService)
        {
            this._trancamentoService = trancamentoService;
        }

        [HttpGet]
        public IActionResult Form(int? trancamentoId)
        {
            return null;
        }
    }
}