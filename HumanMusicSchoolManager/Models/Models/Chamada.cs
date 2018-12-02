using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.Models
{
    public class Chamada
    {
        public int? Id { get; set; }

        public string Observacao { get; set; }

        public int PacoteCompraId { get; set; }
        public PacoteCompra PacoteCompra { get; set; }

        public int AulaId { get; set; }
        public Aula Aula { get; set; }

        public bool? Presenca { get; set; }

        public Reposicao Reposicao { get; set; }
    }
}
