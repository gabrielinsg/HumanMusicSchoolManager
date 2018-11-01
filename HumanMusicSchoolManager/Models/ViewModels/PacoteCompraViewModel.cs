using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.ViewModels
{
    public class PacoteCompraViewModel
    {
        public Matricula Matricula { get; set; }
        public List<PacoteAula> PacotesAula { get; set; }
        public PacoteCompra PacoteCompra { get; set; }
        public PacoteAula PacoteAula { get; set; }

        public PacoteCompraViewModel()
        {
            this.PacoteAula = new PacoteAula();
        }
    }
}
