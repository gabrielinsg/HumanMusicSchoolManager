using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.Models
{
    public class Pessoa
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O Campo nome é obrigatório")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }
        [Required]
        [Display(Name = "Ativo")]
        public bool Ativo { get; set; }

        [Display(Name = "E-mail")]
        [EmailAddress]
        public string Email { get; set; }
        public List<CursoProfessor> Cursos { get; set; }
    }
}
