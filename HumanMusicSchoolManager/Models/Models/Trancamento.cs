using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.Models
{
    public class Trancamento
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Data inicial obrigatório")]
        [Display(Name = "Data inicial")]
        public DateTime DataInicial { get; set; }

        [Required(ErrorMessage = "Data final obrigatório")]
        [Display(Name = "Data final")]
        public DateTime DataFinal { get; set; }

        public int PacoteCompraId { get; set; }
        public PacoteCompra PacoteCompra { get; set; }
    }
}
