﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.Models
{
    public class Aluno : Pessoa
    {
        [Required]
        public int RM { get; set; }
        public List<Matricula> Matriculas { get; set; }
    }
}
