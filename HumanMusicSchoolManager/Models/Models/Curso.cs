using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.Models
{
    public class Curso
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Campo Nome obrigatório")]
        public string Nome { get; set; }
        public List<CursoProfessor> Professores { get; set; }
        public bool Ativo { get; set; }
        [Display(Name = "Quantidade de módulos")]
        [Range(0, int.MaxValue, ErrorMessage = "Valor Quantidade de módulos inválido")]
        [Required(ErrorMessage = "Quantidade de módulos obrigatório")]
        public int QtdModulo { get; set; }
        public List<CursoSala> Salas { get; set; }
        public List<Demostrativa> Demostrativas { get; set; }
        [Required(ErrorMessage = "Duração da aula obrigatório")]
        [Display(Name = "Duração da aula (minutos)")]
        public int DuracaoAula { get; set; }

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
