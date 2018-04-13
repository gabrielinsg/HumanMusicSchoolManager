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

        [Required]
        public DateTime Data { get; set; }

        [Required]
        [Display(Name = "Presente")]
        public bool Presenca { get; set; }

        [Required]
        public string DescAtividades { get; set; }

        public int MatriculaId { get; set; }
        public Matricula Matricula { get; set; }
    }
}
