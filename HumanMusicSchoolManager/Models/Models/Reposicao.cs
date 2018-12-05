using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.Models
{
    public class Reposicao
    {
        public int? Id { get; set; }
        public int ChamadaId { get; set; }
        public Chamada Chamada { get; set; }
        public int? DispSalaId { get; set; }
        public DispSala DispSala { get; set; }
        public string Motivo { get; set; }
    }
}
