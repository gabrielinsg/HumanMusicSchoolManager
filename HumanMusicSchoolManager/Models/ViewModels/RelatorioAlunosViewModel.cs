using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.ViewModels
{
    public class RelatorioAlunosViewModel
    {
        public List<Aluno> Alunos { get; set; }
        public int TotalAlunosMatriculadso { get
            {
                return Alunos.Count();
            }
        }
        public int TotalMatriculas { get
            {
                var total = 0;
                foreach (var aluno in Alunos)
                {
                    total += aluno.Matriculas.Where(m => m.DispSalaId != null).ToList().Count();
                }
                return total;
            }
        }
    }
}
