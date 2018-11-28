using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.Models
{
    public class Pessoa
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "O Campo nome é obrigatório")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }
        [Required]
        [Display(Name = "Ativo")]
        public bool Ativo { get; set; }

        [Required(ErrorMessage = "O Campo CPF é obrigatório")]
        [CPF(ErrorMessage = "CPF inválido")]
        public String CPF { get; set; }

        [Display(Name = "E-mail")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O Campo Dada de Nascimento é obrigatório")]
        [Display(Name = "Data de Nascimento")]
        [DataType(DataType.Date, ErrorMessage = "Formato de Data incorreta")]
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "O Campo RG é obrigatório")]
        public String RG { get; set; }

        [Display(Name = "Telefone")]
        public string Tel { get; set; }

        [Display(Name = "Celular")]
        public string Cel { get; set; }

        public int EnderecoId { get; set; }
        public Endereco Endereco { get; set; }
    }
}
