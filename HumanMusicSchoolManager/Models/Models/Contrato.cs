using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.Models
{
    public class Contrato
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "Nome do cantrato obrigatório")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Conteúdo obrigatório")]
        [Display(Name = "Conteúdo")]
        public string Conteudo { get; set; }
        public bool Ativo { get; set; }
    }
}
