using HumanMusicSchoolManager.Data;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.ServicesInterface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Components
{
    public class Resumo : ViewComponent
    {
        private readonly IRelatorioService _relatorioService;
        private readonly ApplicationDbContext _context;

        public Resumo(IRelatorioService relatorioService,
            ApplicationDbContext context)
        {
            this._relatorioService = relatorioService;
            this._context = context;
        }

        public IViewComponentResult Invoke()
        {
            var resumo = new Dictionary<object, object>
            {
                { 1, NovasMatriculas() },
                { 2, MatriculasCanceladas() },
                { 3, Demonstrativas() },
                { 4, AulasDia() }
            };

            return View(resumo);
        }

        private Dictionary<string, Object> NovasMatriculas()
        {
            Professor professor = null;
            if (User.IsInRole("Professor"))
            {
                var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                professor = _context.Professores.FirstOrDefault(p => p.Id == user.PessoaId);
            }

            var inicial = NowHorarioBrasilia.GetNow().AddDays(-NowHorarioBrasilia.GetNow().Day + 1);
            var final = NowHorarioBrasilia.GetNow().AddDays(-NowHorarioBrasilia.GetNow().Day + 1).AddMonths(1).AddDays(-1);

            var inicialAnterior = inicial.AddMonths(-1).AddDays(-inicial.AddMonths(-1).Day + 1);
            var finalAnterior = final.AddMonths(-1).AddDays(-final.AddMonths(-1).Day + 1).AddMonths(1).AddDays(-1);
            var total = !User.IsInRole("Professor") ? 
                _relatorioService.MatriculasNovas(inicial, final).Count : 
                _relatorioService.MatriculasNovas(inicial, final)
                .Where(p => p.ProfessorId == professor.Id.Value)
                .ToList().Count;
            var totalAnterior = !User.IsInRole("Professor") ? 
                _relatorioService.MatriculasNovas(inicialAnterior, finalAnterior).Count : 
                _relatorioService.MatriculasNovas(inicialAnterior, finalAnterior)
                .Where(p => p.ProfessorId == professor.Id.Value)
                .ToList().Count;
            var positivo = total >= totalAnterior ? true : false;
            float porcentagem = 0;

            if (positivo)
            {
                porcentagem = (float)(total - totalAnterior) / totalAnterior * 100;
            }
            else
            {
                porcentagem = (float)(totalAnterior - total) / totalAnterior * 100;
            }

            var ob = new Dictionary<string, object>
                {
                    { "Titulo", "Mat. Novas" },
                    { "Anterior", totalAnterior },
                    { "Total", total },
                    { "Positivo", positivo },
                    { "Porcentagem", porcentagem.ToString("##0.##") },
                    { "Link", User.IsInRole("Professor") ? "#" : "/Relatorio/MatriculasNovas" }
                };

            return ob;
        }

        private Dictionary<string, object> Demonstrativas()
        {
            Professor professor = null;
            if (User.IsInRole("Professor"))
            {
                var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                professor = _context.Professores.FirstOrDefault(p => p.Id == user.PessoaId);
            }

            var inicial = NowHorarioBrasilia.GetNow().AddDays(-NowHorarioBrasilia.GetNow().Day + 1);
            var final = NowHorarioBrasilia.GetNow().AddDays(-NowHorarioBrasilia.GetNow().Day + 1).AddMonths(1).AddDays(-1);

            var inicialAnterior = inicial.AddMonths(-1).AddDays(-inicial.AddMonths(-1).Day + 1);
            var finalAnterior = final.AddMonths(-1).AddDays(-final.AddMonths(-1).Day + 1).AddMonths(1).AddDays(-1);
            var total = !User.IsInRole("Professor") ? _relatorioService.Demostrativas(inicial, final).Count : _relatorioService.Demostrativas(inicial, final).Where(d => d.ProfessorId == professor.Id.Value).ToList().Count;
            var totalAnterior = !User.IsInRole("Professor") ? _relatorioService.Demostrativas(inicialAnterior, finalAnterior).Count : _relatorioService.Demostrativas(inicialAnterior, finalAnterior).Where(d => d.ProfessorId == professor.Id.Value).ToList().Count;
            var positivo = total >= totalAnterior ? true : false;
            float porcentagem = 0;

            if (positivo)
            {
                porcentagem = (float)(total - totalAnterior) / totalAnterior * 100;
            }
            else
            {
                porcentagem = (float)(totalAnterior - total) / totalAnterior * 100;
            }

            var ob = new Dictionary<string, object>
                {
                    { "Titulo", "Demonstrativas" },
                    { "Anterior", totalAnterior },
                    { "Total", total },
                    { "Positivo", positivo },
                    { "Porcentagem", porcentagem.ToString("##0.##") },
                    { "Link", User.IsInRole("Professor") ? "#" : "/Relatorio/Demonstrativas" }
                };

            return ob;
        }

        private Dictionary<string, object> AulasDia()
        {
            Professor professor = null;
            if (User.IsInRole("Professor"))
            {
                var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                professor = _context.Professores.FirstOrDefault(p => p.Id == user.PessoaId);
            }

            var aulas = !User.IsInRole("Professor") ? _context.Aulas.Where(a => a.Data.Date == NowHorarioBrasilia.GetNow().Date).ToList().Count : _context.Aulas.Where(a => a.Data.Date == NowHorarioBrasilia.GetNow().Date && a.ProfessorId == professor.Id.Value).ToList().Count;
            var aulasRestantes = !User.IsInRole("Professor") ? _context.Aulas.Where(a => a.Data.Date == NowHorarioBrasilia.GetNow().Date && a.AulaDada).ToList().Count : _context.Aulas.Where(a => a.Data.Date == NowHorarioBrasilia.GetNow().Date && a.ProfessorId == professor.Id.Value && a.AulaDada).ToList().Count;
            var ob = new Dictionary<string, object>
            {
                { "Titulo", "Aulas do dia" },
                { "Total", aulas },
                { "Restante", aulasRestantes },
                { "Link", "/Home/Index" }
            };

            return ob;
        }

        private Dictionary<string, object> MatriculasCanceladas()
        {
            Professor professor = null;
            if (User.IsInRole("Professor"))
            {
                var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                professor = _context.Professores.FirstOrDefault(p => p.Id == user.PessoaId);
            }

            var inicial = NowHorarioBrasilia.GetNow().AddDays(-NowHorarioBrasilia.GetNow().Day + 1);
            var final = NowHorarioBrasilia.GetNow().AddDays(-NowHorarioBrasilia.GetNow().Day + 1).AddMonths(1).AddDays(-1);

            var inicialAnterior = inicial.AddMonths(-1).AddDays(-inicial.AddMonths(-1).Day + 1);
            var finalAnterior = final.AddMonths(-1).AddDays(-final.AddMonths(-1).Day + 1).AddMonths(1).AddDays(-1);
            var total = !User.IsInRole("Professor") ?
                _relatorioService.MatriculasCanceladas(inicial, final).Count :
                _relatorioService.MatriculasCanceladas(inicial, final)
                .Where(p => p.ProfessorId == professor.Id.Value)
                .ToList().Count;
            var totalAnterior = !User.IsInRole("Professor") ?
                _relatorioService.MatriculasCanceladas(inicialAnterior, finalAnterior).Count :
                _relatorioService.MatriculasCanceladas(inicialAnterior, finalAnterior)
                .Where(p => p.ProfessorId == professor.Id.Value)
                .ToList().Count;
            var positivo = total >= totalAnterior ? true : false;
            float porcentagem = 0;

            if (positivo)
            {
                porcentagem = (float)(total - totalAnterior) / totalAnterior * 100;
            }
            else
            {
                porcentagem = (float)(totalAnterior - total) / totalAnterior * 100;
            }

            var ob = new Dictionary<string, object>
                {
                    { "Titulo", "Mat. Canceladas" },
                    { "Anterior", totalAnterior },
                    { "Total", total },
                    { "Positivo", positivo },
                    { "Porcentagem", porcentagem.ToString("##0.##") },
                    { "Link", User.IsInRole("Professor") ? "#" : "/Relatorio/MatriculasCanceladas" }
                };

            return ob;
        }
    }
}
