using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.Models
{
    public class PacoteCompra
    {
        public int? Id { get; set; }

        [Required]
        public PacoteAula PacoteAula { get; set; }

        [Required]
        public Matricula Matricula { get; set; }

        [Required]
        public DateTime DataCompra { get; set; }
    }
}
