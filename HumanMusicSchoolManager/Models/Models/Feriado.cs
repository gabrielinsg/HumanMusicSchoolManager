using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.Models
{
    public class Feriado
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "Campo Nome é obrigatório")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Data inicial é obrigatória")]
        [Display(Name = "Data inicial")]
        public DateTime DataInicial { get; set; }
        [Display(Name = "Data final")]
        public DateTime? DataFinal { get; set; }
    }
}
