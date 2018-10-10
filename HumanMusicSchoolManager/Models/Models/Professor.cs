﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.Models
{
    public class Professor : Pessoa
    {
        [Required(ErrorMessage = "Campo Salário Obrigatório")]
        [Display(Name = "Salário / hora")]
        [Range(0, double.MaxValue, ErrorMessage = "Valor inválido")]
        public decimal Salario { get; set; }
        public List<CursoProfessor> Cursos { get; set; }

        public Professor()
        {
            this.Cursos = new List<CursoProfessor>();
        }

        public void IncluiCurso(Curso curso)
        {
            this.Cursos.Add(new CursoProfessor() { Curso = curso });
        }

        public void RemoveCurso(Curso curso)
        {
            var cProfessor = this.Cursos.Where(c => c.Curso == curso).Single();
            this.Cursos.Remove(cProfessor);
        }
    }
}
