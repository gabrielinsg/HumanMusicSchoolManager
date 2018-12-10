using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.ViewModels
{
    public class CancelarPacoteViewModel
    {
        public PacoteCompra PacoteCompra { get; set; }
        public decimal Multa { get; set; }
        public decimal Desconto { get; set; }
        [Display(Name = "Forma de Pagamento")]
        public FormaPagamento FormaPagamento { get; set; }

        public CancelarPacoteViewModel() { }
        public CancelarPacoteViewModel(PacoteCompra pacoteCompra)
        {
            this.PacoteCompra = pacoteCompra;
        }
    }
}
