using System;
using System.Linq;
using HumanMusicSchoolManager.ServicesInterface;
using Microsoft.AspNetCore.Mvc;

namespace HumanMusicSchoolManager.Controllers
{
    public class AdminController : Controller
    {
        private readonly IPacoteCompraService _pacoteCompraService;

        public AdminController(IPacoteCompraService pacoteCompraService)
        {
            this._pacoteCompraService = pacoteCompraService;
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
    }
}