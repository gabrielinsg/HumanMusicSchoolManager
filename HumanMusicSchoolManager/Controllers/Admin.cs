using System;
using System.Linq;
using HumanMusicSchoolManager.Data;
using HumanMusicSchoolManager.ServicesInterface;
using Microsoft.AspNetCore.Mvc;

namespace HumanMusicSchoolManager.Controllers
{
    public class AdminController : Controller
    {
        private readonly IPacoteCompraService _pacoteCompraService;
        private readonly ApplicationDbContext _context;

        public AdminController(IPacoteCompraService pacoteCompraService,
            ApplicationDbContext context)
        {
            this._pacoteCompraService = pacoteCompraService;
            this._context = context;
        }
        public IActionResult PacoteCompra()
        {
            //var pacotesComprar = _pacoteCompraService.BuscarTodos();
            //foreach (var pacoteCompra in pacotesComprar)
            //{
            //    var data = new DateTime();
            //    if (pacoteCompra.Matricula.PacoteCompras.Count <= 1)
            //    {
            //        data = pacoteCompra.Matricula.DataMatricula;
            //    }
            //    else
            //    {
            //        data = pacoteCompra.Financeiros.OrderBy(f => f.DataVencimento).FirstOrDefault().DataVencimento;
            //        if (data == null)
            //        {
            //            data = pacoteCompra.Financeiros.OrderBy(f => f.DataVencimento).FirstOrDefault().UltimaAlteracao;
            //        }
            //    }

            //    if (pacoteCompra.DataCompra.Month != pacoteCompra.Matricula.DataMatricula.Month)
            //    {
            //        data = data.AddMonths(-1);
            //    }


            //    pacoteCompra.DataCompra = data;
            //    _pacoteCompraService.Alterar(pacoteCompra);
            //}
            return null;
        }

        public IActionResult Professores()
        {
            var professores = _context.Professores.ToList();
            foreach (var professor in professores)
            {
                var rMatricula = _context.RelatorioMatriculas
                    .Where(m => m.Descricao.Contains("Matricula") && m.Descricao.Contains(professor.Nome)).ToList();
                foreach (var relatorio in rMatricula)
                {
                    var matricula = _context.Matriculas.FirstOrDefault(m => m.Id == relatorio.MatriculaId);
                    matricula.ProfessorId = professor.Id.Value;
                    _context.Update(matricula);
                    _context.SaveChanges();
                }
            }

            return null;
        }
    }
}