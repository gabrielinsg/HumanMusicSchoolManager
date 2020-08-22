using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.Models
{
    public class RelatorioMatricula
    {
        public int Id { get; set; }

        public int PessoaId { get; set; }
        public virtual Pessoa Pessoa { get; set; }
        public DateTime Data { get; set; }
        public string Descricao { get; set; }
        public int MatriculaId { get; set; }
        public virtual Matricula Matricula { get; set; }
    }
}
