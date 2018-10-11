using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.ViewModels
{
    public class SalaViewModel
    {
        public List<Sala> Salas { get; set; }
        public List<Curso> Cursos { get; set; }

        public SalaViewModel(List<Sala> salas, List<Curso> cursos)
        {
            this.Salas = salas;
            this.Cursos = cursos;
        }
    }
}
