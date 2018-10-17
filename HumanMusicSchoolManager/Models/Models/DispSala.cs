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

        public Professor Professor { get; set; }
    }
}

public enum Dia {
    DOMINGO = 1,
    SEGUNDA = 2,
    TERCA = 3,
    QUARTA = 4,
    QUINTA = 5,
    SEXTA = 6,
    SABADO = 7
}

