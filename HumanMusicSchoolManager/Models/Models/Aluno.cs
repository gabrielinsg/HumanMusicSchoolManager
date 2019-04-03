﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.Models
{
    public class Aluno : Pessoa
    {
        public List<Matricula> Matriculas { get; set; }
        public List<Financeiro> Financeiros { get; set; }
        [Display(Name = "Observação")]
        public string Observacao { get; set; }
        public Sexo Sexo { get; set; }
    }
}
