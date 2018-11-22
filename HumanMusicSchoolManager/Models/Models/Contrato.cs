using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.Models
{
    public class Contrato
    {
        public int? Id { get; set; }
        public string Nome { get; set; }
        public string Conteudo { get; set; }
        public bool Ativo { get; set; }
    }
}
