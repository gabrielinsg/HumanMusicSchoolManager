using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.Models
{
    public class Vale
    {
        public int Id { get; set; }
        public int PessoaId { get; set; }
        public virtual Pessoa Pessoa { get; set; }
        [Display(Name = "Observação")]
        public string Observacao { get; set; }
        [Required(ErrorMessage = "Valor obrigatório.")]
        public decimal Valor { get; set; }
        [Required(ErrorMessage = "Data obrigatória.")]
        public DateTime Data { get; set; }
    }
}
