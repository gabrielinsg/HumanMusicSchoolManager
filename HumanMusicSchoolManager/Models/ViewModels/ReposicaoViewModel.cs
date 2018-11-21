using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.ViewModels
{
    public class ReposicaoViewModel
    {
        public Chamada Chamada { get; set; }
        public DispSala DispSala { get; set; }
        public List<DispSala> DispSalas { get; set; }
        public List<Curso> Cursos { get; set; }
        public Reposicao Reposicao { get; set; }
        [Display(Name = "Dia da Aula")]
        [Required(ErrorMessage = "Selecione um dia para aula de reposição")]
        public DateTime DiaAula { get; set; }
    }
}
