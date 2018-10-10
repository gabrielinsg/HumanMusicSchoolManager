﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.Models
{
    public class Sala
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O Campo Nome é obrigatório")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O Campo Capacidade é obrigatório")]
        public int Capacidade { get; set; }

        public bool Ativo { get; set; }

        public List<CursoSala> Cursos { get; set; }

        Sala()
        {
            this.Cursos = new List<CursoSala>();
        }

        public void IncluirCurso(Curso curso)
        {
            this.Cursos.Add(new CursoSala() { Curso = curso });
        }

        public void ExcluirCurso(Curso curso)
        {
            var cSala = this.Cursos.Where(c => c.Curso == curso).Single();
            this.Cursos.Remove(cSala);
        }
    }
}
