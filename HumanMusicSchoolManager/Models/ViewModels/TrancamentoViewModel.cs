using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.ViewModels
{
    public class TrancamentoViewModel
    {
        public PacoteCompra PacoteCompra { get; set; }
        public Trancamento Trancamento { get; set; }

        public TrancamentoViewModel() { }
        public TrancamentoViewModel(Trancamento trancamento, PacoteCompra pacoteCompra)
        {
            this.Trancamento = trancamento;
            this.PacoteCompra = pacoteCompra;
        }
        public TrancamentoViewModel(PacoteCompra pacoteCompra)
        {
            this.PacoteCompra = pacoteCompra;
        }
    }
}
