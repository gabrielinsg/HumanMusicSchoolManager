using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.Models
{
    public class Candidato
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Campo Nome obrigatório!")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O Campo Data de Nascimento é Obrigatório!")]
        [Display(Name = "Data de Nascimento")]
        public DateTime DataNascimento { get; set; }

        public string Email { get; set; }

        [CPF(ErrorMessage = "CPF inválido")]
        public string CPF { get; set; }

        [Display(Name = "Telefone")]
        public string Tel { get; set; }

        [Required(ErrorMessage = "O Campo celular é obrigatório!")]
        [Display(Name = "Celular")]
        public string Cel { get; set; }

        public Sexo Sexo { get; set; }

        public List<Demostrativa> Demostrativas { get; set; }
    }
}
