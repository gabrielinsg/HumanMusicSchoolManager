using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.Models
{
    public class DispSala
    {
        public int? Id { get; set; }
        public Dia Dia { get; set; }
        public int Hora { get; set; }
        public virtual Sala Sala { get; set; }
        public virtual List<Matricula> Matriculas { get; set; }
        public virtual List<Reposicao> Reposicoes { get; set; }
        public virtual List<Demostrativa> Demostrativas { get; set; }
        public bool Ativo { get; set; }

        public virtual Professor Professor { get; set; }
        public int? ProfessorId { get; set; }

        public DispSala()
        {
            if (ProfessorId != null)
                this.Professor = new Professor();
        }

    }
}

public enum Dia {
    [Display(Name = "Segunda-feira")]
    SEGUNDA = 1,
    [Display(Name = "Terça-feira")]
    TERCA = 2,
    [Display(Name = "Quarta-feira")]
    QUARTA = 3,
    [Display(Name = "Quinta-feira")]
    QUINTA = 4,
    [Display(Name = "Sexta-feira")]
    SEXTA = 5,
    [Display(Name = "Sábado")]
    SABADO = 6
}

