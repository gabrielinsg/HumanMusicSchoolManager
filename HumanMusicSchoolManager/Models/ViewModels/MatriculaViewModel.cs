using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.ViewModels
{
    public class MatriculaViewModel
    {
        public List<Matricula> Matriculas { get; set; }

        public MatriculaViewModel(List<Matricula> matriculas)
        {
            this.Matriculas = matriculas;
        }
    }
}
