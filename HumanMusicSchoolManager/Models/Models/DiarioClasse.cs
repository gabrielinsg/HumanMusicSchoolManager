using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.Models
{
    public class DiarioClasse
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "A {0} não pode ser vazia.")]
        [Display(Name = "Data de matricula")]
        public DateTime Data { get; set; }

        [Required]
        [Display(Name = "Presente")]
        public bool Presenca { get; set; }

        [Required(ErrorMessage = "A {0} não pode ser vazia.")]
        [Display(Name = "Descrição de atividades")]
        [StringLength(500, ErrorMessage = "A {0} deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 6)]
        public string DescAtividades { get; set; }

        public int MatriculaId { get; set; }
        public Matricula Matricula { get; set; }
    }
}
