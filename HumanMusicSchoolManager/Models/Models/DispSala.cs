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
        public Sala Sala { get; set; }
        public List<Matricula> Matriculas { get; set; }

        public Professor Professor { get; set; }

        public DispSala()
        {
            this.Professor = new Professor();
        }
    }
}

public enum Dia {
    [Display(Name = "Segunda-feira")]
    SEGUNDA = 2,
    [Display(Name = "Terça-feira")]
    TERCA = 3,
    [Display(Name = "Quarta-feira")]
    QUARTA = 4,
    [Display(Name = "Quinta-feira")]
    QUINTA = 5,
    [Display(Name = "Sexta-feira")]
    SEXTA = 6,
    [Display(Name = "Sábado")]
    SABADO = 7
}

