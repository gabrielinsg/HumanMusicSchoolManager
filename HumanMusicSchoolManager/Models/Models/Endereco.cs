using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.Models
{
    public class Endereco
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O Campo Nick Name é obrigatório")]
        [Display(Name = "Nick Name")]
        public string NickName { get; set; }

        [Required(ErrorMessage = "O Campo Endereço é obrigatório")]
        [Display(Name = "Logradouro")]
        public string Logradouro { get; set; }

        [Required(ErrorMessage = "O Campo Número é obrigatório")]
        [Display(Name = "Número")]
        public int Numero { get; set; }

        public String Complemento { get; set; }

        [Required(ErrorMessage = "O Campo CEP é obrigatório")]
        public string CEP { get; set; }

        [Required(ErrorMessage = "O Campo Baiiro é obrigatório")]
        public string Bairro { get; set; }
    }
}
