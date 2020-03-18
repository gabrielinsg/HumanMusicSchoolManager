using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HumanMusicSchoolManager.Models;
using Microsoft.AspNetCore.Authorization;
using HumanMusicSchoolManager.Data;
using HumanMusicSchoolManager.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace HumanMusicSchoolManager.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            this._context = context;
        }

        public IActionResult Index()
        {
            Professor professor = null;
            if (User.IsInRole("Professor"))
            {
                var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                professor = _context.Professores.FirstOrDefault(p => p.Id == user.PessoaId);
            }

            var aulas = !User.IsInRole("Professor") ?
                _context.Aulas
                .Include(a => a.Professor)
                .Include(a => a.Sala)
                .Include(a => a.Chamadas)
                .ThenInclude(c => c.PacoteCompra)
                .ThenInclude(pc => pc.Matricula)
                .ThenInclude(m => m.Aluno)
                .Include(a => a.Demostrativas)
                .ThenInclude(d => d.Candidato)
                .Include(a => a.Chamadas)
                .ThenInclude(c => c.PacoteCompra)
                .ThenInclude(pc => pc.PacoteAula)
                .Where(a => a.Data.Date == NowHorarioBrasilia.GetNow().Date).ToList() :
                _context.Aulas
                .Include(a => a.Professor)
                .Include(a => a.Sala)
                .Include(a => a.Chamadas)
                .ThenInclude(c => c.PacoteCompra)
                .ThenInclude(pc => pc.Matricula)
                .ThenInclude(m => m.Aluno)
                .Include(a => a.Demostrativas)
                .ThenInclude(d => d.Candidato)
                .Include(a => a.Chamadas)
                .ThenInclude(c => c.PacoteCompra)
                .ThenInclude(pc => pc.PacoteAula)
                .Where(a => a.Data.Date == NowHorarioBrasilia.GetNow().Date && a.ProfessorId == professor.Id.Value).ToList();

            return View(aulas);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
    }
}
