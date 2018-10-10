using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.Models
{
    public class RespFinanceiro : Pessoa
    {
        public List<Matricula> Matriculas { get; set; }

        public RespFinanceiro()
        {
            this.Matriculas = new List<Matricula>();
        }

        public void AdicionarMatricula(Matricula matricula)
        {
            this.Matriculas.Add(matricula);
        }

        public void RemoverMatricula(Matricula matricula)
        {
            this.Matriculas.Remove(matricula);
        }
    }
}
