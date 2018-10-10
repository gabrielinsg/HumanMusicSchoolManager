using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.Models
{
    public class Curso
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }
        public List<CursoProfessor> Professores { get; set; }
        public bool Ativo { get; set; }
        public int QtdModulo { get; set; }
        public List<CursoSala> Salas { get; set; }

        public Curso()
        {
            this.Professores = new List<CursoProfessor>();
            this.Salas = new List<CursoSala>();
        }

        public void IncluirProfessor(Professor professor)
        {
            this.Professores.Add(new CursoProfessor() { Professor = professor });
        }
    }
}
