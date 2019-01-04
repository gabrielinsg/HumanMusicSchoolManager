using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Display(Name = "Forma de Pagamento")]
        public FormaPagamento FormaPagamento { get; set; }
        public DateTime Vencimento { get; set; }
        [Display(Name = "Primeira Aula")]
        public DateTime PrimeiraAula { get; set; }
        [Required(ErrorMessage = "Valor obrigatório")]
        public Decimal Valor { get; set; }
        [Required(ErrorMessage = "Qtd Aula obrigatório")]
        [Display(Name = "Qtd Aulas")]
        public int QtdAula { get; set; }


        public PacoteCompraViewModel()
        {
            this.PacoteAula = new PacoteAula();
        }
    }
}
