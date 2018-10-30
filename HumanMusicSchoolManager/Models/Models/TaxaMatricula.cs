using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.Models
{
    public class TaxaMatricula
    {
        public int? Id { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; }
    }
}
